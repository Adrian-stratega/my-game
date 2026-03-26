using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using IndiGame.Core;

namespace IndiGame.Phone
{
    /// <summary>
    /// Gestiona la galería de fotos del teléfono de Marco.
    /// Las fotos aparecen solas (OnPhotoAppeared), revelando que Marco lleva semanas en el mismo turno.
    /// </summary>
    public class GalleryController : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private PhotoData[] photos;

        [Header("UI References")]
        [SerializeField] private Transform gridContent;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private GameObject emptyStatePanel;
        [SerializeField] private FullscreenViewer fullscreenViewer;

        [Header("Badge")]
        [SerializeField] private GameObject galleryBadge;
        [SerializeField] private TextMeshProUGUI badgeText;

        [Header("Flash Overlay")]
        [SerializeField] private Image flashOverlay;

        [Header("Thumbnail Prefab")]
        [SerializeField] private GameObject thumbnailPrefab;

        [Header("Audio")]
        [SerializeField] private AudioSource cameraAudio;

        private int badgeCount;
        private readonly List<PhotoData> revealedPhotos = new List<PhotoData>();

        // ─────────────────────────────────────────────
        //  Lifecycle
        // ─────────────────────────────────────────────

        private void Awake()
        {
            // Beep programático de cámara si no hay clip asignado
            if (cameraAudio != null && cameraAudio.clip == null)
                cameraAudio.clip = CreateBeepClip(880f, 0.12f);

            // Flash inicia oculto
            if (flashOverlay != null)
                flashOverlay.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            if (EventManager.Instance != null)
                EventManager.Instance.OnPhotoAppeared += HandlePhotoAppeared;

            // Resetear badge al abrir la galería
            ResetBadge();
            RefreshEmptyState();
        }

        private void OnDisable()
        {
            if (EventManager.Instance != null)
                EventManager.Instance.OnPhotoAppeared -= HandlePhotoAppeared;
        }

        // ─────────────────────────────────────────────
        //  Event Handler
        // ─────────────────────────────────────────────

        private void HandlePhotoAppeared(int _)
        {
            PhotoData next = GetNextUnrevealedPhoto();
            if (next == null)
            {
                Debug.Log("[GalleryController] Todas las fotos ya han aparecido.");
                return;
            }

            next.isRevealed = true;
            revealedPhotos.Add(next);

            IncrementBadge();
            StartCoroutine(PhotoAppearedSequence(next));
        }

        private IEnumerator PhotoAppearedSequence(PhotoData data)
        {
            yield return StartCoroutine(FlashRoutine());

            if (cameraAudio != null)
                cameraAudio.Play();

            RefreshEmptyState();
            SpawnThumbnail(data, animate: true);
        }

        // ─────────────────────────────────────────────
        //  Flash
        // ─────────────────────────────────────────────

        private IEnumerator FlashRoutine()
        {
            if (flashOverlay == null) yield break;

            flashOverlay.gameObject.SetActive(true);
            flashOverlay.color = Color.white;
            yield return new WaitForSeconds(0.1f);

            float t = 0f;
            while (t < 0.4f)
            {
                t += Time.deltaTime;
                flashOverlay.color = new Color(1f, 1f, 1f, 1f - (t / 0.4f));
                yield return null;
            }

            flashOverlay.gameObject.SetActive(false);
        }

        // ─────────────────────────────────────────────
        //  Thumbnail
        // ─────────────────────────────────────────────

        private void SpawnThumbnail(PhotoData data, bool animate)
        {
            if (thumbnailPrefab == null || gridContent == null) return;

            GameObject thumb = Instantiate(thumbnailPrefab, gridContent);

            var photoImg = thumb.transform.Find("PhotoImage")?.GetComponent<Image>();
            if (photoImg != null)
            {
                photoImg.sprite = data.photo;
                photoImg.color = data.photo != null ? Color.white : new Color(0.05f, 0.05f, 0.1f, 1f);
            }

            var tsText = thumb.transform.Find("TimestampOverlay/TimestampText")?.GetComponent<TextMeshProUGUI>();
            if (tsText != null) tsText.text = data.timestamp;

            var btn = thumb.GetComponent<Button>();
            if (btn != null)
            {
                var capturedData = data;
                btn.onClick.AddListener(() => fullscreenViewer?.Open(capturedData));
            }

            if (animate)
                StartCoroutine(AnimateBadge(galleryBadge != null ? galleryBadge.transform : null,
                    StartCoroutine(ScaleInThumbnail(thumb.transform))));
        }

        private IEnumerator AnimateBadge(Transform badgeT, Coroutine thumbCoroutine)
        {
            // Animar badge: scale 0 → 1 en 0.2s
            if (badgeT == null) yield break;
            badgeT.localScale = Vector3.zero;
            badgeT.gameObject.SetActive(true);
            float t = 0f;
            while (t < 0.2f)
            {
                t += Time.deltaTime;
                badgeT.localScale = Vector3.one * Mathf.SmoothStep(0f, 1f, t / 0.2f);
                yield return null;
            }
            badgeT.localScale = Vector3.one;
        }

        private IEnumerator ScaleInThumbnail(Transform t)
        {
            t.localScale = Vector3.zero;
            float elapsed = 0f;
            const float dur = 0.3f;
            while (elapsed < dur)
            {
                elapsed += Time.deltaTime;
                float p = elapsed / dur;
                // Ease out back
                float c1 = 1.70158f;
                float c3 = c1 + 1f;
                float scale = 1f + c3 * Mathf.Pow(p - 1f, 3f) + c1 * Mathf.Pow(p - 1f, 2f);
                t.localScale = Vector3.one * Mathf.Max(0f, scale);
                yield return null;
            }
            t.localScale = Vector3.one;
        }

        // ─────────────────────────────────────────────
        //  Badge
        // ─────────────────────────────────────────────

        private void IncrementBadge()
        {
            badgeCount++;
            UpdateBadgeUI();
        }

        private void ResetBadge()
        {
            badgeCount = 0;
            UpdateBadgeUI();
        }

        private void UpdateBadgeUI()
        {
            if (galleryBadge == null) return;
            galleryBadge.SetActive(badgeCount > 0);
            if (badgeText != null) badgeText.text = badgeCount.ToString();
        }

        // ─────────────────────────────────────────────
        //  Helpers
        // ─────────────────────────────────────────────

        private void RefreshEmptyState()
        {
            if (emptyStatePanel != null)
                emptyStatePanel.SetActive(revealedPhotos.Count == 0);
        }

        private PhotoData GetNextUnrevealedPhoto()
        {
            if (photos == null) return null;
            foreach (var p in photos)
                if (p != null && !p.isRevealed) return p;
            return null;
        }

        private static AudioClip CreateBeepClip(float frequency, float duration)
        {
            int sampleRate = 44100;
            int samples = Mathf.RoundToInt(sampleRate * duration);
            float[] data = new float[samples];
            for (int i = 0; i < samples; i++)
            {
                float t = (float)i / sampleRate;
                float envelope = 1f - (t / duration);
                data[i] = envelope * Mathf.Sin(2f * Mathf.PI * frequency * t) * 0.4f;
            }
            AudioClip clip = AudioClip.Create("CameraBeep", samples, 1, sampleRate, false);
            clip.SetData(data, 0);
            return clip;
        }
    }
}
