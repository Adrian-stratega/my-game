# B11 — Audio: Ambientes, Música y Sonidos UI

**Objetivo:** Implementar la capa sonora completa. El audio ES el 50% del horror en este juego.
**Herramientas:** ElevenLabs MCP (SFX, música), Freesound MCP (ambientes CC0), Unity MCP (implementación).
**Commit esperado:** `B11-COMPLETE: AudioManager, ambientes, SFX notificaciones, música adaptativa`

---

## FASE 1 — Arquitectura de audio

### Script AudioManager.cs
**Ruta:** `Assets/_Game/Scripts/IndiGame/Core/AudioManager.cs`

```csharp
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IndiGame.Core
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Music")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource ambienceSource;

        [Header("SFX Pool")]
        [SerializeField] private AudioSource[] sfxPool;
        private int poolIndex = 0;

        [Header("Clips")]
        [SerializeField] private AudioClip musicNormalidad;
        [SerializeField] private AudioClip musicAnomalia;
        [SerializeField] private AudioClip ambRain;
        [SerializeField] private AudioClip ambCityNight;
        [SerializeField] private AudioClip sfxNotifPing;
        [SerializeField] private AudioClip sfxNotifCritical;
        [SerializeField] private AudioClip sfxNotifSuccess;
        [SerializeField] private AudioClip sfxPhoneOpen;
        [SerializeField] private AudioClip sfxPhoneClose;
        [SerializeField] private AudioClip sfxInterphone;

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        void Start()
        {
            PlayAmbience(ambRain, 0.4f);
            PlayAmbience(ambCityNight, 0.15f, true);
            PlayMusic(musicNormalidad, 0.25f);
        }

        public void PlaySFX(AudioClip clip, float volume = 1f)
        {
            if (clip == null || sfxPool == null || sfxPool.Length == 0) return;
            var src = sfxPool[poolIndex % sfxPool.Length];
            poolIndex++;
            src.clip = clip;
            src.volume = volume;
            src.Play();
        }

        public void PlayNotifPing()   => PlaySFX(sfxNotifPing, 0.8f);
        public void PlayNotifCritical() => PlaySFX(sfxNotifCritical, 1f);
        public void PlayNotifSuccess()  => PlaySFX(sfxNotifSuccess, 0.9f);
        public void PlayPhoneOpen()     => PlaySFX(sfxPhoneOpen, 0.6f);
        public void PlayPhoneClose()    => PlaySFX(sfxPhoneClose, 0.6f);
        public void PlayInterphone()    => PlaySFX(sfxInterphone, 1f);

        public void PlayMusic(AudioClip clip, float volume = 0.3f)
        {
            if (musicSource == null || clip == null) return;
            musicSource.clip = clip; musicSource.loop = true;
            musicSource.volume = volume; musicSource.Play();
        }

        public void PlayAmbience(AudioClip clip, float volume = 0.5f, bool additive = false)
        {
            if (ambienceSource == null || clip == null) return;
            if (!additive) { ambienceSource.clip = clip; ambienceSource.loop = true; ambienceSource.volume = volume; ambienceSource.Play(); }
        }

        public IEnumerator CrossfadeMusic(AudioClip newClip, float duration = 2f)
        {
            if (musicSource == null) yield break;
            float startVol = musicSource.volume;
            float elapsed = 0f;
            while (elapsed < duration / 2f)
            {
                elapsed += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(startVol, 0f, elapsed / (duration / 2f));
                yield return null;
            }
            musicSource.clip = newClip; musicSource.Play();
            elapsed = 0f;
            while (elapsed < duration / 2f)
            {
                elapsed += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(0f, startVol, elapsed / (duration / 2f));
                yield return null;
            }
        }
    }
}
```

---

## FASE 2 — Obtener assets de audio

### 2.1 Lluvia ambiente (Freesound CC0)

Buscar en Freesound MCP:
```
Buscar: "heavy rain night city ambient loop"
Filtros: license=CC0, duration<120s
Seleccionar el de mayor duración y mejor calidad (mínimo 44.1kHz)
Descargar → mover a Assets/_Game/Audio/SFX/Ambient/SFX_rain_heavy_loop.wav
```

### 2.2 Ciudad noche (Freesound CC0)

```
Buscar: "city night ambience distant traffic loop"
Filtros: license=CC0, duration<120s
Descargar → Assets/_Game/Audio/SFX/Ambient/SFX_city_night_loop.wav
```

### 2.3 SFX notificaciones (ElevenLabs MCP)

Generar 3 variaciones de cada uno y seleccionar la mejor:

**Ping de notificación INFO/SUCCESS (breve, neutro/positivo):**
```
Prompt: "A short clean UI notification ping sound, mobile app style, pleasant D4 to A4 ascending tone, 0.3 seconds"
```
→ Guardar como: `Assets/_Game/Audio/SFX/SFX_notif_ping.wav`

**Ping notificación CRITICAL (disonante, inquietante):**
```
Prompt: "A dissonant unsettling phone notification sound, B-flat minor chord, disturbing but subtle, 0.5 seconds"
```
→ Guardar como: `Assets/_Game/Audio/SFX/SFX_notif_critical.wav`

**Confirmación SUCCESS (cálido, positivo):**
```
Prompt: "A warm success confirmation chime, delivery app style, two ascending notes G4 to C5, satisfying, 0.4 seconds"
```
→ Guardar como: `Assets/_Game/Audio/SFX/SFX_notif_success.wav`

**Phone open/close (slide mecánico):**
```
Prompt: "A smooth UI slide sound for a phone appearing on screen, soft whoosh with slight mechanical click, 0.3 seconds"
```
→ `SFX_phone_open.wav` y variante reversa para `SFX_phone_close.wav`

### 2.4 Música (ElevenLabs MCP)

**Música de normalidad (estado inicial):**
```
Prompt: "Ambient electronic music, gig economy worker night shift, lo-fi beats with distant city rain, melancholic but calm, 90 BPM, 2 minutes loop"
```
→ `Assets/_Game/Audio/Music/MUS_normalidad_loop.wav`

**Música de anomalía (se activa en estado 2+):**
```
Prompt: "Unsettling ambient horror music, dissonant piano notes with deep drone, psychological thriller mood, slow 60 BPM, tension building, 2 minutes loop"
```
→ `Assets/_Game/Audio/Music/MUS_anomalia_loop.wav`

---

## FASE 3 — Construir AudioManager en escena

```csharp
using UnityEngine;
using UnityEditor;
using IndiGame.Core;

public class B11Builder
{
    public static string Build()
    {
        var sb = new System.Text.StringBuilder();
        var managers = GameObject.Find("Managers");
        if (managers == null) return "ERROR: no Managers";

        // AudioManager GO
        var amGO = managers.transform.Find("AudioManager")?.gameObject ?? new GameObject("AudioManager");
        amGO.transform.SetParent(managers.transform, false);
        var am = amGO.GetComponent<IndiGame.Core.AudioManager>() ?? amGO.AddComponent<IndiGame.Core.AudioManager>();
        var so = new SerializedObject(am);

        // MusicSource
        var musicSrc = amGO.transform.Find("MusicSource")?.GetComponent<AudioSource>();
        if (musicSrc == null)
        {
            var msGO = new GameObject("MusicSource");
            msGO.transform.SetParent(amGO.transform, false);
            musicSrc = msGO.AddComponent<AudioSource>();
            musicSrc.loop = true; musicSrc.spatialBlend = 0f; musicSrc.volume = 0.25f;
        }
        so.FindProperty("musicSource").objectReferenceValue = musicSrc;

        // AmbienceSource
        var ambSrc = amGO.transform.Find("AmbienceSource")?.GetComponent<AudioSource>();
        if (ambSrc == null)
        {
            var asGO = new GameObject("AmbienceSource");
            asGO.transform.SetParent(amGO.transform, false);
            ambSrc = asGO.AddComponent<AudioSource>();
            ambSrc.loop = true; ambSrc.spatialBlend = 0f; ambSrc.volume = 0.4f;
        }
        so.FindProperty("ambienceSource").objectReferenceValue = ambSrc;

        // SFX Pool (4 sources)
        var poolArr = so.FindProperty("sfxPool");
        poolArr.arraySize = 4;
        for (int i = 0; i < 4; i++)
        {
            var sfxGO = amGO.transform.Find($"SFX_{i}")?.gameObject ?? new GameObject($"SFX_{i}");
            sfxGO.transform.SetParent(amGO.transform, false);
            var sfxSrc = sfxGO.GetComponent<AudioSource>() ?? sfxGO.AddComponent<AudioSource>();
            sfxSrc.spatialBlend = 0f;
            poolArr.GetArrayElementAtIndex(i).objectReferenceValue = sfxSrc;
        }

        // Wire audio clips (si existen los archivos)
        string audio = "Assets/_Game/Audio";
        AudioClip Load(string p) => AssetDatabase.LoadAssetAtPath<AudioClip>(p);

        var clips = new (string field, string path)[]
        {
            ("ambRain",          $"{audio}/SFX/Ambient/SFX_rain_heavy_loop.wav"),
            ("ambCityNight",     $"{audio}/SFX/Ambient/SFX_city_night_loop.wav"),
            ("sfxNotifPing",     $"{audio}/SFX/SFX_notif_ping.wav"),
            ("sfxNotifCritical", $"{audio}/SFX/SFX_notif_critical.wav"),
            ("sfxNotifSuccess",  $"{audio}/SFX/SFX_notif_success.wav"),
            ("sfxPhoneOpen",     $"{audio}/SFX/SFX_phone_open.wav"),
            ("sfxPhoneClose",    $"{audio}/SFX/SFX_phone_close.wav"),
            ("musicNormalidad",  $"{audio}/Music/MUS_normalidad_loop.wav"),
            ("musicAnomalia",    $"{audio}/Music/MUS_anomalia_loop.wav"),
        };

        foreach (var (field, path) in clips)
        {
            var clip = Load(path);
            if (clip != null) { so.FindProperty(field).objectReferenceValue = clip; sb.AppendLine($"{field} wired ✓"); }
            else sb.AppendLine($"WARN: {field} no encontrado en {path} — wirear manualmente");
        }

        so.ApplyModifiedProperties();
        sb.AppendLine("AudioManager GO configurado ✓");

        // Eliminar AmbienceSource duplicada (si quedó de B4/B5)
        var oldAmb = managers.transform.Find("AmbienceSource")?.gameObject;
        if (oldAmb != null && oldAmb != amGO) { UnityEngine.Object.DestroyImmediate(oldAmb); sb.AppendLine("Old AmbienceSource eliminado ✓"); }

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();
        sb.AppendLine("=== B11 BUILDER COMPLETO ===");
        return sb.ToString();
    }
}
```

---

## FASE 4 — Integrar audio con sistemas existentes

### Añadir llamadas de audio en PhoneController (B7):
```csharp
// En OpenPhone(): AudioManager.Instance?.PlayPhoneOpen();
// En ClosePhone(): AudioManager.Instance?.PlayPhoneClose();
```

### Añadir en AppNotificationController:
```csharp
// En ShowNotification(), según tipo:
// INFO/WARNING: AudioManager.Instance?.PlayNotifPing();
// SUCCESS: AudioManager.Instance?.PlayNotifSuccess();
// CRITICAL: AudioManager.Instance?.PlayNotifCritical();
```

Modificar los scripts B7 y B6 para añadir estas llamadas.

---

## FASE 5 — Verificación en Play Mode

```csharp
public class B11Verify { public static string Run() {
    var sb = new System.Text.StringBuilder();
    sb.AppendLine("AudioManager: " + (IndiGame.Core.AudioManager.Instance != null ? "OK" : "NULL ❌"));
    var am = IndiGame.Core.AudioManager.Instance;
    if (am != null) { am.PlayNotifPing(); sb.AppendLine("PlayNotifPing() llamado — debe sonar"); }
    return sb.ToString();
}}
```

- Entrar Play Mode → debe escucharse lluvia + ciudad de fondo
- Ejecutar verify → debe sonar el ping
- Abrir teléfono con F → debe sonar el slide
- Cerrar → otro slide

---

## FASE 6 — Commit

```
git add -A
git commit -m "B11-COMPLETE: AudioManager, lluvia CC0, ciudad CC0, SFX notificaciones, música normalidad/anomalía"
git push origin main
```

## Checklist B11
- [ ] AudioManager singleton en Managers/AudioManager
- [ ] MusicSource + AmbienceSource + SFX Pool (4 sources) configurados
- [ ] En Play Mode: lluvia se escucha automáticamente
- [ ] PlayNotifPing() suena
- [ ] PlayPhoneOpen/Close() integrado en PhoneController
- [ ] Notificaciones disparan audio según tipo
- [ ] 0 errores en consola
- [ ] Archivos audio en rutas correctas (Assets/_Game/Audio/...)
