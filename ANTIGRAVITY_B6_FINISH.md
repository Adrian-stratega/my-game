# ANTIGRAVITY — CONTEXTO COMPLETO + PROMPT PERFECTO PARA TERMINAR B6
# Para: Asistente Perplexity → Antigravity (Gemini CLI + Unity MCP)
# Creado: 2026-03-26 por Claude Code (análisis completo del proyecto real)

---

## ⚠️ LEER ESTO PRIMERO (Perplexity: este es tu contexto)

Este documento fue generado por **Claude Code** leyendo el estado REAL del proyecto en Unity.
No es una estimación. Es el estado exacto verificado con las herramientas MCP.

B6 está **casi terminado**. Los scripts están escritos y compilando. La UI está construida en escena.
Hay **2 problemas exactos** que bloquean el B6. El prompt de abajo los resuelve ambos.

---

## 📊 ESTADO REAL DE B6 (verificado 2026-03-26)

### ✅ YA EXISTE Y ESTÁ CORRECTO

| Elemento | Ruta en escena / assets | Estado |
|---|---|---|
| `RatingController.cs` | `Assets/_Game/Scripts/IndiGame/Phone/` | ✅ Correcto |
| `AppNotificationController.cs` | `Assets/_Game/Scripts/IndiGame/Phone/` | ✅ Correcto |
| `GameOverController.cs` | `Assets/_Game/Scripts/IndiGame/UI/` | ✅ Correcto |
| `NotificationData.cs` | `Assets/_Game/Scripts/IndiGame/Phone/` | ✅ Correcto |
| `NotificationType.cs` | `Assets/_Game/Scripts/IndiGame/Phone/` | ✅ INFO/SUCCESS/WARNING/CRITICAL |
| Panel_Inicio | `UI/PhoneCanvas/PhonePanel/Panel_Inicio` | ✅ UI construida: Logo, Subtítulo, Separator, RatingRow (5 estrellas + texto), StatusText, Spacer, StartButton |
| RatingController wired | Sobre el GO Panel_Inicio | ✅ stars[0-4] + ratingText + phonePanelFlash wired vía SerializedObject |
| NotificationBanner | `UI/PhoneCanvas/PhonePanel/NotificationBanner` | ✅ IconText, MessageText, TimestampText |
| AppNotificationController wired | Sobre el GO NotificationBanner | ✅ bannerRoot + bannerBackground + iconText + messageText + timestampText + dismissButton wired |
| 10 Notification SOs | `Assets/_Game/ScriptableObjects/Notifications/` | ✅ Notif_Welcome, Notif_Delivery1_Complete, Notif_Delivery2_Complete, Notif_NoAddress, Notif_ClientWaiting, Notif_EnterHouse, Notif_GoToBasement, Notif_BasementInsist, Notif_WeKnow, Notif_LastDelivery |

### ❌ PROBLEMAS QUE BLOQUEAN B6 (exactamente 2)

**PROBLEMA 1 — GameOverPanel en jerarquía incorrecta**

```
ACTUAL (MAL):
UI/
  PhoneCanvas/          ← PhoneCanvas se desactiva cuando el teléfono se cierra
    PhonePanel/
    GameOverPanel/      ← AQUÍ ESTÁ: se queda hierActive=false cuando se cierra el teléfono
                           → GameOverController.Instance == null en Play Mode

CORRECTO:
UI/
  PhoneCanvas/          ← se puede desactivar, no importa
    PhonePanel/
  GameOverCanvas/       ← Canvas nuevo, SIEMPRE activo, separado
    GameOverPanel/      ← AQUÍ DEBE ESTAR: siempre activeInHierarchy=true
```

**PROBLEMA 2 — Dos TestRatingTrigger.cs en conflicto**

Existen dos versiones:
- `Assets/_Game/Scripts/IndiGame/Tests/TestRatingTrigger.cs` — ANTIGUA, usa `Input.GetKeyDown` (legacy Input System) → BORRAR
- `Assets/_Game/Scripts/IndiGame/Phone/TestRatingTrigger.cs` — NUEVA, usa `Keyboard.current` (new Input System, R/Y/N/M keys) → MANTENER

**PROBLEMA 3 — TestRatingTrigger no está en la escena**
El .cs existe pero el componente no está añadido a ningún GO de la escena. Debe añadirse a `Managers` para que funcione en Play Mode.

---

## 🎮 JERARQUÍA ACTUAL DE LA ESCENA (Main_EP1)

```
Main Camera (inactivo)
Managers/
  GameManager          ← singleton, DontDestroyOnLoad
  EventManager         ← singleton, DontDestroyOnLoad
  AmbienceSource
  SceneSetup
Environment/
  Directional Light
  Ground
  Building_Front/
    Window_0..5
  Sidewalk
  Dumpster
  Streetlight_01/
    Pole
    StreetPointLight/LampHead
  RainSystem
  Global Volume
Characters/            ← vacío
UI/
  PhoneCanvas/
    PhonePanel/
      Panel_Chat       (activeSelf=false)
      Panel_Mapa       (activeSelf=false)
      Panel_Inicio     (activeSelf=false — PhoneTabController lo activa en Start)
        LogoText, SubtitleText, Separator, RatingRow/Star1-5/RatingValueText, StatusText, Spacer, StartButton/ButtonLabel
      Panel_Galeria    (activeSelf=true)
      BottomNav/
        Tab_Inicio, Tab_Mapa, Tab_Chat, Tab_Galeria/GaleriaBadge/BadgeText
      NotificationBanner/
        IconText, MessageText, TimestampText
      PhoneFlash       (activeSelf=false)
      Panel_Fullscreen (activeSelf=false)
    GameOverPanel/     ← ⚠️ ESTÁ AQUÍ, MAL UBICADO
      GameOverLogo, MainMessage, SubMessage, Spacer, BackToMenuButton/ButtonLabel
[Player]/
  Player/
    MainCamera
```

---

## 📋 SCRIPTS CLAVE — API PÚBLICA

```csharp
// RatingController — en Panel_Inicio
RatingController.Instance?.SetRating(float newRating);   // 0.0 – 5.0
RatingController.Instance?.AddRating(float delta);        // delta positivo o negativo
RatingController.Instance?.GetRating();                   // float actual
// Si rating llega a 0.0 → llama automáticamente GameOverController.Instance?.ShowGameOver()

// AppNotificationController — en NotificationBanner
AppNotificationController.Instance?.ShowNotification(NotificationType.SUCCESS, "texto");
AppNotificationController.Instance?.ShowNotification(NotificationData data);
// Tipos: INFO (azul), SUCCESS (verde), WARNING (naranja), CRITICAL (rojo pulsante, requiere dismiss)

// GameOverController — en GameOverPanel (a mover)
GameOverController.Instance?.ShowGameOver();  // fade in pantalla negra corporativa
// Fade dura 0.8s, botón aparece 3s después

// TestRatingTrigger (temporal, en Managers)
// R = AddRating(-0.5f) | Y = AddRating(+0.5f)
// N = CRITICAL notification | M = SUCCESS notification
```

---

## 🔧 EL PROMPT PERFECTO PARA ANTIGRAVITY

> **Instrucción para Perplexity:** Copia el bloque de abajo EXACTAMENTE y pásalo a Antigravity.
> No lo parafrasees. No lo reduzcas. Cada sección es necesaria.

---

```
B6-FINISH — Terminar Rating System + Notificaciones + Game Over

CONTEXTO: B6 está casi terminado. Los scripts compilan, Panel_Inicio y NotificationBanner están construidos y wired. Hay 2 bugs que bloquean la verificación. Debes arreglarlos, verificar TODO en Play Mode, y hacer el commit.

═══════════════════════════════════════════════════
FASE 1 — DIAGNÓSTICO (ejecuta todo antes de tocar nada)
═══════════════════════════════════════════════════

1.1 Leer jerarquía actual:
    scene-get-data (depth=6, includeRootGameObjects=true)

1.2 Verificar cero errores de compilación:
    console-get-logs (type=Error)
    → Si hay errores: leerlos, corregir scripts, esperar compilación, repetir
    → Si 0 errores: continuar

1.3 Leer los scripts (para entender el wiring actual):
    script-read: Assets/_Game/Scripts/IndiGame/UI/GameOverController.cs
    script-read: Assets/_Game/Scripts/IndiGame/Phone/RatingController.cs

═══════════════════════════════════════════════════
FASE 2 — ARREGLAR SCRIPTS (2 acciones)
═══════════════════════════════════════════════════

2.1 BORRAR el TestRatingTrigger ANTIGUO (usa legacy Input):
    script-delete: Assets/_Game/Scripts/IndiGame/Tests/TestRatingTrigger.cs

    → Esperar a que Unity compile (puede tardar 10-20s)
    → console-get-logs (Error) → confirmar 0 errores

2.2 Verificar que el TestRatingTrigger NUEVO existe y está correcto:
    script-read: Assets/_Game/Scripts/IndiGame/Phone/TestRatingTrigger.cs
    → Debe usar Keyboard.current (new Input System)
    → R = AddRating(-0.5) | Y = AddRating(+0.5) | N = CRITICAL notif | M = SUCCESS notif

═══════════════════════════════════════════════════
FASE 3 — ARREGLAR ESCENA (script-execute Builder)
═══════════════════════════════════════════════════

3.1 Ejecutar este Builder exacto con script-execute:

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using IndiGame.Phone;
using IndiGame.UI;

public class B6FinishBuilder
{
    static Color Hex(string h) {
        Color c = Color.white;
        ColorUtility.TryParseHtmlString("#" + h, out c);
        return c;
    }

    public static string Build()
    {
        var sb = new System.Text.StringBuilder();
        try
        {
            // ══════════════════════════════════════════════
            // FIX 1: Mover GameOverPanel fuera de PhoneCanvas
            // ══════════════════════════════════════════════

            // Encontrar los GOs necesarios
            var uiRoot = GameObject.Find("UI");
            if (uiRoot == null) return "ERROR: No se encontró el GO 'UI'";

            var phoneCanvas = uiRoot.transform.Find("PhoneCanvas")?.gameObject;
            if (phoneCanvas == null) return "ERROR: No se encontró 'UI/PhoneCanvas'";

            var gameOverPanel = phoneCanvas.transform.Find("GameOverPanel")?.gameObject;
            if (gameOverPanel == null) return "ERROR: No se encontró 'UI/PhoneCanvas/GameOverPanel'";

            // Crear un nuevo Canvas "GameOverCanvas" bajo UI (sibling de PhoneCanvas)
            var gameOverCanvas = new GameObject("GameOverCanvas");
            gameOverCanvas.transform.SetParent(uiRoot.transform, false);

            // Configurar el Canvas
            var canvas = gameOverCanvas.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100; // Por encima de todo

            // Añadir CanvasScaler igual al PhoneCanvas
            var scaler = gameOverCanvas.AddComponent<UnityEngine.UI.CanvasScaler>();
            scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;

            // Añadir GraphicRaycaster
            gameOverCanvas.AddComponent<UnityEngine.UI.GraphicRaycaster>();

            sb.AppendLine("GameOverCanvas creado ✓");

            // Mover GameOverPanel bajo el nuevo Canvas
            gameOverPanel.transform.SetParent(gameOverCanvas.transform, false);

            // Asegurar que GameOverPanel ocupa toda la pantalla (stretch fill)
            var rt = gameOverPanel.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
            }

            sb.AppendLine("GameOverPanel movido a UI/GameOverCanvas ✓");

            // ══════════════════════════════════════════════
            // FIX 2: Re-wirear GameOverController via SerializedObject
            // (el wiring previo puede haber quedado desconectado)
            // ══════════════════════════════════════════════

            var goc = gameOverPanel.GetComponent<IndiGame.UI.GameOverController>();
            if (goc == null) return "ERROR: GameOverController no encontrado en GameOverPanel";

            var so = new SerializedObject(goc);

            // gameOverPanel → CanvasGroup en el propio GO
            var cg = gameOverPanel.GetComponent<CanvasGroup>() ?? gameOverPanel.AddComponent<CanvasGroup>();
            so.FindProperty("gameOverPanel").objectReferenceValue = cg;

            // textos — buscarlos en hijos
            var logoText = gameOverPanel.transform.Find("GameOverLogo")?.GetComponent<TMPro.TextMeshProUGUI>();
            var mainMsg  = gameOverPanel.transform.Find("MainMessage")?.GetComponent<TMPro.TextMeshProUGUI>();
            var subMsg   = gameOverPanel.transform.Find("SubMessage")?.GetComponent<TMPro.TextMeshProUGUI>();
            var btn      = gameOverPanel.transform.Find("BackToMenuButton")?.GetComponent<UnityEngine.UI.Button>();

            if (logoText != null) so.FindProperty("logoText").objectReferenceValue = logoText;
            if (mainMsg  != null) so.FindProperty("mainMessage").objectReferenceValue = mainMsg;
            if (subMsg   != null) so.FindProperty("subMessage").objectReferenceValue = subMsg;
            if (btn      != null) so.FindProperty("backToMenuButton").objectReferenceValue = btn;

            so.ApplyModifiedProperties();
            sb.AppendLine("GameOverController wired ✓");

            // Verificar que HideInstant funciona (alpha=0 en editor)
            cg.alpha = 0f;
            cg.interactable = false;
            cg.blocksRaycasts = false;
            sb.AppendLine("CanvasGroup oculto (alpha=0) ✓");

            // ══════════════════════════════════════════════
            // FIX 3: Añadir TestRatingTrigger a la escena
            // ══════════════════════════════════════════════

            var managers = GameObject.Find("Managers");
            if (managers == null) return "ERROR: No se encontró el GO 'Managers'";

            var testHelper = managers.transform.Find("TestHelper")?.gameObject;
            if (testHelper == null)
            {
                testHelper = new GameObject("TestHelper");
                testHelper.transform.SetParent(managers.transform, false);
                sb.AppendLine("TestHelper GO creado ✓");
            }

            var existingTRT = testHelper.GetComponent<IndiGame.Phone.TestRatingTrigger>();
            if (existingTRT == null)
            {
                testHelper.AddComponent<IndiGame.Phone.TestRatingTrigger>();
                sb.AppendLine("TestRatingTrigger añadido a Managers/TestHelper ✓");
            }
            else
            {
                sb.AppendLine("TestRatingTrigger ya existía en Managers/TestHelper ✓");
            }

            // ══════════════════════════════════════════════
            // Guardar escena
            // ══════════════════════════════════════════════
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();
            sb.AppendLine("Escena guardada ✓");

            sb.AppendLine("═══════════════════════════════════════════");
            sb.AppendLine("B6 BUILDER COMPLETADO");
            sb.AppendLine("GameOverPanel: UI/GameOverCanvas/GameOverPanel");
            sb.AppendLine("TestRatingTrigger: Managers/TestHelper");
        }
        catch (Exception e)
        {
            sb.AppendLine("EXCEPTION: " + e.Message);
            sb.AppendLine(e.StackTrace);
        }
        return sb.ToString();
    }
}

3.2 Tomar screenshot para verificar visualmente:
    screenshot-game-view
    → La pantalla debe mostrar el entorno (edificio, lluvia, farola)
    → GameOverPanel NO debe ser visible (está oculto con alpha=0)

═══════════════════════════════════════════════════
FASE 4 — VERIFICACIÓN EN PLAY MODE
═══════════════════════════════════════════════════

4.1 Entrar en Play Mode:
    script-execute: EditorApplication.isPlaying = true;
    → Esperar 5 segundos para que Unity inicialice

4.2 Verificar que todos los singletons existen:
script-execute:
using System.Text;
using IndiGame.Phone;
using IndiGame.UI;
using IndiGame.Core;

public class B6Verify {
    public static string Check() {
        var sb = new StringBuilder();
        sb.AppendLine("=== SINGLETONS ===");
        sb.AppendLine($"RatingController.Instance: {(RatingController.Instance != null ? "OK" : "NULL ❌")}");
        sb.AppendLine($"AppNotificationController.Instance: {(AppNotificationController.Instance != null ? "OK" : "NULL ❌")}");
        sb.AppendLine($"GameOverController.Instance: {(GameOverController.Instance != null ? "OK" : "NULL ❌")}");
        sb.AppendLine($"GameManager.Instance: {(GameManager.Instance != null ? "OK" : "NULL ❌")}");
        sb.AppendLine($"EventManager.Instance: {(EventManager.Instance != null ? "OK" : "NULL ❌")}");

        sb.AppendLine("\n=== GAMEOVERCONTROLLER CHECK ===");
        var goc = GameOverController.Instance;
        if (goc != null) {
            sb.AppendLine($"  GO: {goc.gameObject.name} active={goc.gameObject.activeSelf} hierActive={goc.gameObject.activeInHierarchy}");
        }

        sb.AppendLine("\n=== RATING CHECK ===");
        var rc = RatingController.Instance;
        if (rc != null) {
            sb.AppendLine($"  Rating actual: {rc.GetRating()}");
        }
        return sb.ToString();
    }
}

    → TODOS deben ser "OK"
    → GameOverController.Instance hierActive DEBE SER TRUE
    → Si alguno es NULL: leer la sección de PROBLEMAS FRECUENTES al final

4.3 Test de Rating (bajar y subir):
script-execute:
public class B6TestRating {
    public static string Test() {
        var sb = new System.Text.StringBuilder();
        var rc = IndiGame.Phone.RatingController.Instance;
        if (rc == null) return "ERROR: RatingController null";

        float antes = rc.GetRating();
        rc.AddRating(-1.0f);
        float despues = rc.GetRating();
        rc.AddRating(1.0f); // restaurar

        sb.AppendLine($"Rating antes: {antes} → después de -1.0: {despues}");
        sb.AppendLine(despues == (antes - 1.0f) ? "✅ Rating math CORRECTO" : "❌ Rating math INCORRECTO");
        return sb.ToString();
    }
}

4.4 Test de Notificación SUCCESS:
script-execute:
public class B6TestNotifSuccess {
    public static string Test() {
        var anc = IndiGame.Phone.AppNotificationController.Instance;
        if (anc == null) return "ERROR: AppNotificationController null";
        anc.ShowNotification(IndiGame.Phone.NotificationType.SUCCESS, "¡Entrega completada! El cliente te dio 5 estrellas 🎉");
        return "ShowNotification(SUCCESS) llamado ✓";
    }
}
    → Tomar screenshot: debe verse el banner verde deslizándose desde arriba

4.5 Test de Notificación CRITICAL:
script-execute:
public class B6TestNotifCritical {
    public static string Test() {
        var anc = IndiGame.Phone.AppNotificationController.Instance;
        if (anc == null) return "ERROR: AppNotificationController null";
        anc.ShowNotification(IndiGame.Phone.NotificationType.CRITICAL, "Marco. Sabemos que ya lo sabes. Completa el turno.");
        return "ShowNotification(CRITICAL) llamado ✓";
    }
}
    → Tomar screenshot: banner rojo pulsante visible, requiere dismiss

4.6 Test de Game Over:
script-execute:
public class B6TestGameOver {
    public static string Test() {
        var goc = IndiGame.UI.GameOverController.Instance;
        if (goc == null) return "ERROR: GameOverController null";
        goc.ShowGameOver();
        return "ShowGameOver() llamado ✓ — pantalla debe aparecer en 0.8s";
    }
}
    → Esperar 2 segundos
    → Tomar screenshot: pantalla negra con "Cuenta desactivada" visible en rojo

4.7 Tomar screenshot final del Game View con Game Over visible.

4.8 Verificar console sin errores:
    console-get-logs (type=Error, lastMinutes=3)
    → 0 errores (ignorar SocketException/HubConnection — son transitorios del MCP)

4.9 Salir de Play Mode:
    script-execute: EditorApplication.isPlaying = false;
    → Esperar 3 segundos

═══════════════════════════════════════════════════
FASE 5 — COMMIT B6
═══════════════════════════════════════════════════

5.1 Configurar git:
    (via desktop-commander o script-execute con System.Diagnostics.Process)
    git config user.name "Adrian-stratega"
    git config user.email "bytestratega@gmail.com"

5.2 Stage y commit:
    git add -A
    git commit -m "B6-COMPLETE: Rating system, notification controller, QuickRun home, 10 notification SOs, GameOver screen"
    git push origin main

5.3 Reportar el SHA del commit al usuario.

═══════════════════════════════════════════════════
CHECKLIST B6 (verificar ✅ cada punto antes del commit)
═══════════════════════════════════════════════════

□ 1. RatingController.Instance != null en Play Mode
□ 2. AppNotificationController.Instance != null en Play Mode
□ 3. GameOverController.Instance != null en Play Mode (hierActive=true)
□ 4. Rating 5.0 visible en Panel_Inicio (5 estrellas naranjas)
□ 5. Rating baja correctamente con AddRating(-0.5f)
□ 6. Shake + flash rojo al bajar rating
□ 7. Banner SUCCESS (verde) visible y se desliza desde arriba
□ 8. Banner CRITICAL (rojo pulsante) requiere dismiss
□ 9. Cola de notificaciones: 3 en secuencia se muestran en orden
□ 10. Game Over se activa cuando rating llega a 0
□ 11. Pantalla Game Over: fondo negro, "Cuenta desactivada" en rojo, aparece con fade
□ 12. Botón "Volver" aparece 3s después del fade
□ 13. 0 errores en consola (no contar SocketException)
□ 14. Escena guardada (.unity modificado en git)
□ 15. Commit con SHA correcto

═══════════════════════════════════════════════════
PROBLEMAS FRECUENTES Y SOLUCIONES
═══════════════════════════════════════════════════

▶ Instance == null después de Play Mode
   Causa: El GO tiene activeInHierarchy=false cuando Awake() se ejecuta
   Fix: Verificar con Resources.FindObjectsOfTypeAll<T>() dónde está el componente
        Si está bajo PhoneCanvas → mover a GameOverCanvas (el builder ya lo hace)

▶ CS0234 / CS0246 al script-execute
   Causa: Unity no compiló aún el script modificado
   Fix: console-get-logs (Error), esperar 20s, reintentar

▶ "GameOverCanvas" ya existe al re-ejecutar el builder
   Fix: El builder busca el GO existente y no duplica; si hay error, ajustar

▶ SocketException / HubConnection en consola
   IGNORAR. Son reconexiones transitorias del MCP. No son errores del juego.

▶ FindProperty devuelve null
   Causa: El nombre del field no coincide con el privado en C#
   Nombres exactos en GameOverController:
   - "gameOverPanel" → CanvasGroup
   - "logoText" → TextMeshProUGUI
   - "mainMessage" → TextMeshProUGUI
   - "subMessage" → TextMeshProUGUI
   - "backToMenuButton" → Button
   - "fadeDuration" → float (default 0.8f)
   - "buttonDelay" → float (default 3.0f)
```

---

## 🛠️ CÓMO GENERAR PROMPTS PERFECTOS PARA ANTIGRAVITY (guía para Perplexity)

Para cada bloque futuro (B7, B8, etc.), el prompt para Antigravity debe tener:

### Estructura obligatoria del prompt

```
1. CONTEXTO: qué ya existe (scripts, GOs, componentes wired)
2. OBJETIVO: qué debe existir al FINALIZAR el bloque
3. FASE 1 - ANÁLISIS: scene-get-data + console-get-logs antes de tocar nada
4. FASE 2 - SCRIPTS: crear/actualizar .cs uno por uno + verificar 0 errores entre scripts
5. FASE 3 - ESCENA: script-execute Builder completo (un único Builder con todo)
6. FASE 4 - PLAY MODE: verificar singletons + llamar métodos directamente + screenshots
7. FASE 5 - COMMIT: git config + git add + git commit + git push
8. CHECKLIST: lista de ítems binarios (□) para cada entregable
```

### Reglas que Antigravity SIEMPRE debe seguir

1. **Nunca escribir solo scripts — siempre construir la escena también**
   El workflow es: escribir .cs → compilar → script-execute Builder para costruir UI y wirear

2. **Usar SerializedObject para wirear [SerializeField] privados**
   ```csharp
   var so = new SerializedObject(miComponente);
   so.FindProperty("nombrePrivadoExacto").objectReferenceValue = miRef;
   so.ApplyModifiedProperties();
   ```

3. **Verificar en Play Mode con script-execute, no solo visualmente**
   Llamar directamente los métodos públicos del sistema que se acaba de construir.

4. **SocketException en consola = ignorar siempre**
   Son reconexiones del MCP. No afectan al juego.

5. **Si script-execute falla con CS0234**: Unity no compiló aún. Esperar 20s y reintentar.

6. **Git siempre con**:
   ```
   git config user.name "Adrian-stratega"
   git config user.email "bytestratega@gmail.com"
   ```

7. **Namespaces del proyecto**:
   - `IndiGame.Core` — GameManager, EventManager, GameState
   - `IndiGame.Phone` — todo lo de la app del teléfono
   - `IndiGame.UI` — GameOverController y futuros UI controllers
   - `IndiGame.Environment` — FlickerLight, etc.

8. **FindObjectOfType NO funciona con GOs inactivos**. Usar:
   ```csharp
   var all = Resources.FindObjectsOfTypeAll<MiScript>();
   ```

---

## 🗺️ ANÁLISIS DE LA HOJA DE RUTA B7–B28 (mejoras sugeridas)

### B7 — Pantalla de Loading + GameClock + Transiciones

**Problema en el spec actual:** Falta especificar dónde vive TransitionController en la jerarquía y cómo interactúa con DeliveryManager. No hay PhoneController definido (el script que abre/cierra el teléfono con F no existe todavía en el proyecto).

**Añadir al spec de B7:**
- `PhoneController.cs` (si no existe): gestiona show/hide del PhoneCanvas con F. **Este script es crucial** — es lo que causa que PhoneCanvas se desactive.
- `GameClock.cs` debe disparar `EventManager.OnGameStateChanged` cuando avanza el tiempo narrativo.
- `TransitionController` debe vivir en `Managers/` (siempre activo).
- El canvas de fade (para FadeToBlack) debe ser un Canvas separado con sortingOrder alto (200+), igual que GameOverCanvas.

### B8 — Arte y Materiales

**Sin problemas mayores.** El spec es sólido. Recordar usar la paleta oficial:
- `#0d0d1a` negro profundo | `#1a2a4a` azul noche | `#FF6B35` naranja | `#e94560` rojo | `#f0f0f0` blanco

### B9–B13 — Entornos

**Advertencia:** Los entornos de B12 y B13 son muy ambiciosos para un solo bloque. Considera si conviene dividir B12 en "construir geometría" (Blender MCP) y "añadir sistema de estados" (Unity) en subpasos. No es bloqueante, solo a tener en cuenta.

### B15 — Props

**Problema en el spec:** Meshy AI free tier (100 créditos/mes) es insuficiente para todos los props listados. El spec tiene 9 props en Meshy. Solución práctica:
- Usar Meshy solo para bolsa de delivery, casco de moto e interphone (los más únicos)
- El resto (cajita, vaso, foto, control, cenicero, plato) → Poly Haven o primitivas Unity estilizadas con materiales de B8

### B16–B18 — Narrativa

**Advertencia:** `NarrativeDirector.cs` y `HouseAnomalyDirector.cs` son los scripts más complejos del proyecto. Antes de implementarlos, el GDD Sección 5 debe estar completamente escrito en Notion con los diálogos exactos y los timings en segundos. Si no están escritos, Antigravity no puede implementarlos bien.

**Recomendación:** Antes de B16, crear un bloque B15.5 (o añadir al final de B15) que documente en Notion todos los ConversationSequence SOs con sus textos exactos.

### B19 — Sistema de Decisiones

**El spec menciona 8 decisiones binarias "documentadas en GDD Sección 3"**. Verificar que realmente están documentadas antes de empezar este bloque. Es la columna vertebral del gameplay.

### B20 — Doppelgänger

**Punto crítico para el viral:** Las 3 animaciones (`idle_eating`, `turn_to_look`, `smile_idle`) son lo más importante del juego. Meshy AI exporta el modelo pero NO las animaciones. Opciones:
1. Animaciones manuales simples con Unity Animation window (solo rotaciones)
2. Mixamo (gratis, requiere cuenta Adobe) para `idle_eating` base
3. Animar directamente en Blender MCP

El spec dice "Meshy AI: modelo + animaciones" — Meshy no genera animaciones en free tier. Corregir esto en el spec.

### B21–B23 — Audio

**Freesound MCP disponible y funcional** (configurado en .mcp.json). Filtrar siempre: `license="Creative Commons 0"`. El workflow de Freesound MCP es:
1. `freesound-search` con licencia CC0
2. `freesound-download`
3. Mover con `desktop-commander move_file` a la ruta correcta
4. `assets-refresh` en Unity MCP

### B25 — UI Polish

El spec dice "animación de apertura del teléfono: slide desde abajo con bounce". Esta animación actualmente NO existe — el teléfono simplemente aparece/desaparece. B25 deberá añadir esto al script que controla PhoneCanvas (que se crea en B7).

### B28 — Lanzamiento

**Muy bien pensado.** Un ajuste: el **trailer corto de TikTok** es el activo más importante. Planificar durante B26 (QA) capturar el clip exacto del doppelgänger girando. No dejarlo para B28.

---

## 📁 ESTRUCTURA DE CARPETAS RELEVANTE

```
Assets/_Game/
  Scripts/
    Core/              → GameManager, EventManager, SceneSetup_EP1
    Phone/             → AppNotification (viejo), ChatController, MapController, MapMarker
    IndiGame/
      Core/            → SceneSetup_EP1
      Phone/           → RatingController, AppNotificationController, NotificationData,
                          NotificationType, GalleryController, FullscreenViewer,
                          PhoneTabController, TestRatingTrigger (temporal)
      UI/              → GameOverController
      Environment/     → FlickerLight
      Tests/           → (vacío tras borrar TestRatingTrigger antiguo)
  ScriptableObjects/
    Notifications/     → 10 SOs de notificación
  Scenes/
    Main_EP1.unity
  Audio/SFX/ Music/ Voice/Marco/
  Prefabs/3D/ Props/
  Textures/PBR/ Concept/
```

---

## ✅ DEFINICIÓN DE DONE — B6

B6 está completo cuando:
1. `git log` muestra el commit `B6-COMPLETE: ...`
2. Abrir Unity → dar Play → presionar F (abre teléfono) → Tab Inicio muestra 5 estrellas naranjas
3. Presionar R diez veces → Rating llega a 0 → pantalla "Cuenta desactivada" aparece con fade negro
4. Presionar N → banner rojo pulsante aparece en top del teléfono
5. Console: 0 errores rojos
```
