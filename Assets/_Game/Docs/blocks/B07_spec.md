# B7 — PhoneController + GameClock + Transiciones + DeliveryManager

**Objetivo:** Conectar todos los sistemas. Sin B7 no hay juego jugable.
**Tiempo estimado:** 1 sesión
**Commit esperado:** `B7-COMPLETE: PhoneController, GameClock, TransitionController, DeliveryManager`

---

## FASE 1 — Diagnóstico

```
console-get-logs (Error) → debe ser 0 errores reales
scene-get-data (depth=3) → verificar estructura UI y Managers
```

**Jerarquía requerida después de B6 (verificar antes de empezar):**
```
UI/
  PhoneCanvas (sortingOrder=10)
    PhonePanel/
      Panel_Inicio (activeSelf=true)
      Panel_Chat (activeSelf=false)
      Panel_Mapa (activeSelf=false)
      Panel_Galeria (activeSelf=false)
      BottomNav/
      NotificationBanner/
  GameOverCanvas (sortingOrder=100)
    GameOverPanel/
Managers/
  GameManager
  EventManager
  TestHelper (TestRatingTrigger)
```

Si GameOverPanel sigue en PhoneCanvas → ejecutar el fix de B6 primero (ver ANTIGRAVITY_RULES.md).

---

## FASE 2 — Crear scripts (en este orden exacto)

### 2.1 PhoneController.cs
**Ruta:** `Assets/_Game/Scripts/IndiGame/Core/PhoneController.cs`

```csharp
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using IndiGame.Core;

namespace IndiGame.Core
{
    public class PhoneController : MonoBehaviour
    {
        public static PhoneController Instance { get; private set; }

        [SerializeField] private GameObject phoneCanvas;
        [SerializeField] private float slideInDuration = 0.3f;

        private bool isPhoneOpen = false;
        private RectTransform phonePanelRT;
        private Vector2 hiddenPos;
        private Vector2 shownPos = Vector2.zero;

        public bool IsPhoneOpen => isPhoneOpen;

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        void Start()
        {
            if (phoneCanvas == null) return;
            // Buscar PhonePanel dentro del canvas
            var phonePanel = phoneCanvas.transform.Find("PhonePanel");
            if (phonePanel != null)
                phonePanelRT = phonePanel.GetComponent<RectTransform>();

            if (phonePanelRT != null)
            {
                hiddenPos = new Vector2(0, -Screen.height);
                phonePanelRT.anchoredPosition = hiddenPos;
            }
            phoneCanvas.SetActive(false);
        }

        void Update()
        {
            if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
                TogglePhone();
        }

        public void TogglePhone()
        {
            if (isPhoneOpen) ClosePhone();
            else OpenPhone();
        }

        public void OpenPhone()
        {
            if (isPhoneOpen) return;
            isPhoneOpen = true;
            phoneCanvas.SetActive(true);
            if (phonePanelRT != null)
                StartCoroutine(SlidePhone(hiddenPos, shownPos));
            EventManager.Instance?.OnPhoneOpen?.Invoke();
        }

        public void ClosePhone()
        {
            if (!isPhoneOpen) return;
            isPhoneOpen = false;
            if (phonePanelRT != null)
                StartCoroutine(SlidePhoneOut());
            else
                phoneCanvas.SetActive(false);
            EventManager.Instance?.OnPhoneClose?.Invoke();
        }

        private IEnumerator SlidePhone(Vector2 from, Vector2 to)
        {
            float elapsed = 0f;
            while (elapsed < slideInDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.SmoothStep(0f, 1f, elapsed / slideInDuration);
                phonePanelRT.anchoredPosition = Vector2.Lerp(from, to, t);
                yield return null;
            }
            phonePanelRT.anchoredPosition = to;
        }

        private IEnumerator SlidePhoneOut()
        {
            yield return StartCoroutine(SlidePhone(shownPos, hiddenPos));
            phoneCanvas.SetActive(false);
        }
    }
}
```

### 2.2 GameClock.cs
**Ruta:** `Assets/_Game/Scripts/IndiGame/Core/GameClock.cs`

```csharp
using UnityEngine;
using TMPro;
using System;

namespace IndiGame.Core
{
    public class GameClock : MonoBehaviour
    {
        public static GameClock Instance { get; private set; }

        [SerializeField] private TextMeshProUGUI clockText;
        [SerializeField] private float startHour = 22.5f;   // 22:30
        [SerializeField] private float realSecondsPerGameMinute = 6f;

        private float currentGameMinutes;
        private bool isPaused = false;

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        void Start()
        {
            currentGameMinutes = startHour * 60f;
            UpdateDisplay();
        }

        void Update()
        {
            if (isPaused) return;
            currentGameMinutes += (Time.deltaTime / realSecondsPerGameMinute);
            if (currentGameMinutes >= 24f * 60f) currentGameMinutes -= 24f * 60f;
            UpdateDisplay();
        }

        public void Pause() => isPaused = true;
        public void Resume() => isPaused = false;

        public string GetFormattedTime()
        {
            int h = (int)(currentGameMinutes / 60f) % 24;
            int m = (int)(currentGameMinutes % 60f);
            return $"{h:D2}:{m:D2}";
        }

        private void UpdateDisplay()
        {
            if (clockText != null) clockText.text = GetFormattedTime();
        }
    }
}
```

### 2.3 TransitionController.cs
**Ruta:** `Assets/_Game/Scripts/IndiGame/UI/TransitionController.cs`

```csharp
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace IndiGame.UI
{
    public class TransitionController : MonoBehaviour
    {
        public static TransitionController Instance { get; private set; }

        [SerializeField] private Image fadePanel;

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        void Start()
        {
            if (fadePanel != null)
            {
                fadePanel.color = new Color(0, 0, 0, 0);
                fadePanel.gameObject.SetActive(false);
            }
        }

        public IEnumerator FadeToBlack(float duration = 0.5f)
        {
            if (fadePanel == null) yield break;
            fadePanel.gameObject.SetActive(true);
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                fadePanel.color = new Color(0, 0, 0, Mathf.Clamp01(elapsed / duration));
                yield return null;
            }
            fadePanel.color = new Color(0, 0, 0, 1f);
        }

        public IEnumerator FadeFromBlack(float duration = 0.5f)
        {
            if (fadePanel == null) yield break;
            fadePanel.gameObject.SetActive(true);
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                fadePanel.color = new Color(0, 0, 0, 1f - Mathf.Clamp01(elapsed / duration));
                yield return null;
            }
            fadePanel.color = new Color(0, 0, 0, 0);
            fadePanel.gameObject.SetActive(false);
        }
    }
}
```

### 2.4 LoadingController.cs
**Ruta:** `Assets/_Game/Scripts/IndiGame/UI/LoadingController.cs`

```csharp
using UnityEngine;
using TMPro;
using System.Collections;

namespace IndiGame.UI
{
    public class LoadingController : MonoBehaviour
    {
        public static LoadingController Instance { get; private set; }

        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private TextMeshProUGUI addressText;
        [SerializeField] private float charDelay = 0.05f;

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        void Start()
        {
            if (loadingPanel != null) loadingPanel.SetActive(false);
        }

        public IEnumerator ShowLoading(string address, float totalDuration = 4f)
        {
            if (loadingPanel == null) yield break;
            loadingPanel.SetActive(true);
            if (addressText != null)
            {
                addressText.text = "";
                foreach (char c in address)
                {
                    addressText.text += c;
                    yield return new WaitForSeconds(charDelay);
                }
            }
            float remaining = totalDuration - (address.Length * charDelay);
            if (remaining > 0f) yield return new WaitForSeconds(remaining);
            loadingPanel.SetActive(false);
        }
    }
}
```

### 2.5 DeliveryManager.cs
**Ruta:** `Assets/_Game/Scripts/IndiGame/Core/DeliveryManager.cs`

```csharp
using UnityEngine;
using System.Collections;
using IndiGame.UI;

namespace IndiGame.Core
{
    public class DeliveryManager : MonoBehaviour
    {
        public static DeliveryManager Instance { get; private set; }

        [SerializeField] private string[] deliveryAddresses = {
            "Calle Mayor 14, 3°B",
            "Av. Paralela 88, 2°A",
            "C/ Oscura 7, Bajo"
        };

        private int currentDelivery = 0;

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        public void StartNextDelivery()
        {
            if (currentDelivery >= deliveryAddresses.Length) return;
            StartCoroutine(DeliverySequence());
        }

        private IEnumerator DeliverySequence()
        {
            if (TransitionController.Instance != null)
                yield return StartCoroutine(TransitionController.Instance.FadeToBlack(0.5f));

            string address = deliveryAddresses[currentDelivery];
            float duration = currentDelivery == 2 ? 7f : 4f;

            if (LoadingController.Instance != null)
                yield return StartCoroutine(LoadingController.Instance.ShowLoading(address, duration));

            currentDelivery++;

            if (TransitionController.Instance != null)
                yield return StartCoroutine(TransitionController.Instance.FadeFromBlack(0.5f));
        }
    }
}
```

---

## FASE 3 — Verificar compilación

```
console-get-logs (Error) → 0 errores reales (ignorar SocketException)
```

---

## FASE 4 — Construir escena (script-execute)

Ejecutar este builder exacto:

```csharp
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using IndiGame.Core;
using IndiGame.UI;

public class B7Builder
{
    static Color Hex(string h) { Color c; ColorUtility.TryParseHtmlString("#"+h, out c); return c; }

    public static string Build()
    {
        var sb = new System.Text.StringBuilder();
        try
        {
            var uiRoot = GameObject.Find("UI");
            var managers = GameObject.Find("Managers");
            if (uiRoot == null || managers == null) return "ERROR: falta UI o Managers";

            // ─── PhoneController GO ───
            var pcGO = managers.transform.Find("PhoneController")?.gameObject
                       ?? new GameObject("PhoneController");
            pcGO.transform.SetParent(managers.transform, false);
            var pc = pcGO.GetComponent<IndiGame.Core.PhoneController>()
                     ?? pcGO.AddComponent<IndiGame.Core.PhoneController>();
            var soPC = new SerializedObject(pc);
            soPC.FindProperty("phoneCanvas").objectReferenceValue =
                uiRoot.transform.Find("PhoneCanvas")?.gameObject;
            soPC.ApplyModifiedProperties();
            sb.AppendLine("PhoneController GO ✓");

            // ─── GameClock GO ───
            var gcGO = managers.transform.Find("GameClock")?.gameObject
                       ?? new GameObject("GameClock");
            gcGO.transform.SetParent(managers.transform, false);
            var gc = gcGO.GetComponent<IndiGame.Core.GameClock>()
                     ?? gcGO.AddComponent<IndiGame.Core.GameClock>();
            // wire StatusText del Panel_Inicio
            var statusText = uiRoot.transform
                .Find("PhoneCanvas/PhonePanel/Panel_Inicio/StatusText")
                ?.GetComponent<TextMeshProUGUI>();
            if (statusText != null)
            {
                var soGC = new SerializedObject(gc);
                soGC.FindProperty("clockText").objectReferenceValue = statusText;
                soGC.ApplyModifiedProperties();
                sb.AppendLine("GameClock wired → StatusText ✓");
            }
            sb.AppendLine("GameClock GO ✓");

            // ─── TransitionCanvas ───
            var transCanvas = uiRoot.transform.Find("TransitionCanvas")?.gameObject;
            if (transCanvas == null)
            {
                transCanvas = new GameObject("TransitionCanvas");
                transCanvas.transform.SetParent(uiRoot.transform, false);
                var tc = transCanvas.AddComponent<Canvas>();
                tc.renderMode = RenderMode.ScreenSpaceOverlay;
                tc.sortingOrder = 200;
                var ts = transCanvas.AddComponent<CanvasScaler>();
                ts.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                ts.referenceResolution = new Vector2(1920, 1080);
                ts.matchWidthOrHeight = 0.5f;
                transCanvas.AddComponent<GraphicRaycaster>();
            }
            // FadePanel
            var fadePanelGO = transCanvas.transform.Find("FadePanel")?.gameObject;
            if (fadePanelGO == null)
            {
                fadePanelGO = new GameObject("FadePanel");
                fadePanelGO.transform.SetParent(transCanvas.transform, false);
                var img = fadePanelGO.AddComponent<Image>();
                img.color = new Color(0,0,0,0);
                var rt = fadePanelGO.GetComponent<RectTransform>();
                rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
            }
            // TransitionController
            var transCtrl = transCanvas.GetComponent<IndiGame.UI.TransitionController>()
                            ?? transCanvas.AddComponent<IndiGame.UI.TransitionController>();
            var soTrans = new SerializedObject(transCtrl);
            soTrans.FindProperty("fadePanel").objectReferenceValue =
                fadePanelGO.GetComponent<Image>();
            soTrans.ApplyModifiedProperties();
            sb.AppendLine("TransitionCanvas + FadePanel + TransitionController ✓");

            // ─── LoadingCanvas ───
            var loadCanvas = uiRoot.transform.Find("LoadingCanvas")?.gameObject;
            if (loadCanvas == null)
            {
                loadCanvas = new GameObject("LoadingCanvas");
                loadCanvas.transform.SetParent(uiRoot.transform, false);
                var lc = loadCanvas.AddComponent<Canvas>();
                lc.renderMode = RenderMode.ScreenSpaceOverlay;
                lc.sortingOrder = 150;
                var ls = loadCanvas.AddComponent<CanvasScaler>();
                ls.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                ls.referenceResolution = new Vector2(1920, 1080);
                ls.matchWidthOrHeight = 0.5f;
                loadCanvas.AddComponent<GraphicRaycaster>();
            }
            // LoadingPanel
            var loadPanelGO = loadCanvas.transform.Find("LoadingPanel")?.gameObject;
            if (loadPanelGO == null)
            {
                loadPanelGO = new GameObject("LoadingPanel");
                loadPanelGO.transform.SetParent(loadCanvas.transform, false);
                var bg = loadPanelGO.AddComponent<Image>();
                bg.color = new Color(0.05f, 0.05f, 0.1f, 1f);
                var rt = loadPanelGO.GetComponent<RectTransform>();
                rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
                // Address text
                var addrGO = new GameObject("AddressText");
                addrGO.transform.SetParent(loadPanelGO.transform, false);
                var addr = addrGO.AddComponent<TextMeshProUGUI>();
                addr.text = "Calculando ruta...";
                addr.fontSize = 32;
                addr.color = new Color(1f, 0.42f, 0.21f, 1f); // #FF6B35
                addr.alignment = TextAlignmentOptions.Center;
                var art = addrGO.GetComponent<RectTransform>();
                art.anchorMin = new Vector2(0.1f, 0.4f);
                art.anchorMax = new Vector2(0.9f, 0.6f);
                art.offsetMin = Vector2.zero; art.offsetMax = Vector2.zero;
                loadPanelGO.SetActive(false);
            }
            // LoadingController
            var loadCtrl = loadCanvas.GetComponent<IndiGame.UI.LoadingController>()
                           ?? loadCanvas.AddComponent<IndiGame.UI.LoadingController>();
            var soLoad = new SerializedObject(loadCtrl);
            soLoad.FindProperty("loadingPanel").objectReferenceValue = loadPanelGO;
            var addrText = loadPanelGO.transform.Find("AddressText")?.GetComponent<TextMeshProUGUI>();
            if (addrText != null) soLoad.FindProperty("addressText").objectReferenceValue = addrText;
            soLoad.ApplyModifiedProperties();
            sb.AppendLine("LoadingCanvas + LoadingPanel + LoadingController ✓");

            // ─── DeliveryManager GO ───
            var dmGO = managers.transform.Find("DeliveryManager")?.gameObject
                       ?? new GameObject("DeliveryManager");
            dmGO.transform.SetParent(managers.transform, false);
            if (dmGO.GetComponent<IndiGame.Core.DeliveryManager>() == null)
                dmGO.AddComponent<IndiGame.Core.DeliveryManager>();
            sb.AppendLine("DeliveryManager GO ✓");

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            EditorSceneManager.SaveOpenScenes();
            sb.AppendLine("Escena guardada ✓");
            sb.AppendLine("=== B7 BUILDER COMPLETO ===");
        }
        catch (Exception e) { sb.AppendLine("EXCEPTION: " + e.Message); }
        return sb.ToString();
    }
}
```

---

## FASE 5 — Verificación Play Mode

```csharp
// Entrar Play Mode: EditorApplication.isPlaying = true;
// Esperar 3s, luego ejecutar:

public class B7Verify { public static string Run() {
    var sb = new System.Text.StringBuilder();
    sb.AppendLine("PhoneController: " + (IndiGame.Core.PhoneController.Instance != null ? "OK" : "NULL ❌"));
    sb.AppendLine("GameClock: " + (IndiGame.Core.GameClock.Instance != null ? "OK" : "NULL ❌"));
    sb.AppendLine("TransitionController: " + (IndiGame.UI.TransitionController.Instance != null ? "OK" : "NULL ❌"));
    sb.AppendLine("LoadingController: " + (IndiGame.UI.LoadingController.Instance != null ? "OK" : "NULL ❌"));
    sb.AppendLine("DeliveryManager: " + (IndiGame.Core.DeliveryManager.Instance != null ? "OK" : "NULL ❌"));
    var gc = IndiGame.Core.GameClock.Instance;
    if (gc != null) sb.AppendLine("Hora actual: " + gc.GetFormattedTime());
    return sb.ToString();
}}
```

Todos deben decir "OK". Hora debe mostrar "22:30" o similar.

**Test F key:**
```csharp
public class B7TestPhone { public static string Run() {
    var pc = IndiGame.Core.PhoneController.Instance;
    if (pc == null) return "NULL ❌";
    pc.OpenPhone();
    return "OpenPhone() llamado — PhoneCanvas debe ser visible";
}}
```

**Test FadeToBlack:**
```csharp
public class B7TestFade { public static string Run() {
    var tc = IndiGame.UI.TransitionController.Instance;
    if (tc == null) return "NULL ❌";
    tc.StartCoroutine(tc.FadeToBlack(1f));
    return "FadeToBlack iniciado — pantalla debe oscurecer en 1s";
}}
```

Tomar screenshot después de cada test.

---

## FASE 6 — Commit

```
git config user.name "Adrian-stratega"
git config user.email "bytestratega@gmail.com"
git add -A
git commit -m "B7-COMPLETE: PhoneController, GameClock, TransitionController, LoadingController, DeliveryManager"
git push origin main
```

---

## Checklist B7
- [ ] F abre/cierra el teléfono con slide animation
- [ ] PhoneCanvas se desactiva al cerrar (GameOverCanvas NO se mueve)
- [ ] GameClock muestra hora en StatusText y avanza
- [ ] FadeToBlack() oscurece pantalla correctamente
- [ ] LoadingPanel muestra dirección letra a letra
- [ ] 0 errores reales en consola
- [ ] Commit con SHA reportado
