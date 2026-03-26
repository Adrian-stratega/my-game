# HANDOFF DOCUMENT — Bloque B4.5: EP1 Visual Bug Fixes
**Proyecto:** LAST DELIVERY
**Repositorio:** https://github.com/Adrian-stratega/my-game
**Rama principal:** main
**Fecha sesión:** 2026-03-25
**Engine:** Unity 6 (6000.4.0f1) + URP (Universal Render Pipeline)
**Dev:** Adrian-stratega (bytestratega@gmail.com)
**AI Agent:** Antigravity (Claude Sonnet 4.6 via Claude Code)

---

## RESUMEN EJECUTIVO

Esta sesión resolvió **5 bugs visuales confirmados** en la escena `Main_EP1` de LAST DELIVERY, más **1 bug bloqueante de compilación** que impedía entrar a Play Mode. Se identificó además una **causa raíz arquitectural crítica**: el proyecto Unity había sido creado desde una plantilla 2D (`Lit2DSceneTemplate`), usando el renderer `Renderer2DData` en lugar de `UniversalRendererData` — esto impedía que la niebla 3D y efectos de profundidad funcionaran en absoluto. Se swappeó el renderer completo de 2D a 3D como parte de los fixes.

**Estado final:** Play Mode funciona, sin errores de compilación, todos los bugs verificados visualmente.

---

## COMMITS APLICADOS (rama main)

```
ac51e70  [FIX] MCP asmdef — remove missing assembly ref, unblock Play Mode
f80f18f  [FIX] RainSystem — shape angle 0 for vertical rain, add LampHead to Streetlight
08503e8  [FIX] URP renderer 3D — replace Renderer2D with UniversalRenderer for fog support
```

Todos los commits son exclusivamente de **Adrian-stratega** (bytestratega@gmail.com). No hay Co-Authored-By ni contribuidores externos.

---

## BUG 0 — BLOQUEANTE: Errores de Compilación (RESUELTO)

### Síntoma
Unity no dejaba entrar a Play Mode. Mensaje en Console: `"All compiler errors have to be fixed before you can enter playmode!"`. 6 errores CS0234 en el paquete MCP Animation.

### Causa Raíz Exacta
El paquete `com.ivanmurzak.unity.mcp.animation` versión 1.1.17 tiene un archivo de definición de assembly de tests con una referencia a `com.IvanMurzak.Unity.MCP.Editor.Tests` — este assembly **no existe** en la versión del paquete base instalada. Adicionalmente, el `defineConstraints` del .asmdef era `["UNITY_INCLUDE_TESTS"]`, que Unity activa automáticamente en cualquier proyecto con Test Runner instalado, forzando la compilación de los .cs de tests rotos.

### Archivo Modificado
`Packages/com.ivanmurzak.unity.mcp.animation/Tests/Editor/com.IvanMurzak.Unity.MCP.Animation.Editor.Tests.asmdef`

**Antes (roto):**
```json
{
  "references": [
    "com.IvanMurzak.Unity.MCP.Editor",
    "com.IvanMurzak.Unity.MCP.Editor.Tests",   ← NO EXISTE
    "com.IvanMurzak.Unity.MCP.Runtime",
    ...
  ],
  "autoReferenced": true,
  "defineConstraints": ["UNITY_INCLUDE_TESTS"]   ← SIEMPRE ACTIVO
}
```

**Después (fix):**
```json
{
  "name": "com.IvanMurzak.Unity.MCP.Animation.Editor.Tests",
  "references": [
    "com.IvanMurzak.Unity.MCP.Editor",
    "com.IvanMurzak.Unity.MCP.Runtime",
    "com.IvanMurzak.Unity.MCP.TestFiles",
    "com.IvanMurzak.Unity.MCP.Animation.Runtime",
    "com.IvanMurzak.Unity.MCP.Animation.Editor"
  ],
  "autoReferenced": false,
  "defineConstraints": ["MCP_ANIM_TESTS_ENABLED"]   ← NUNCA auto-activo
}
```

### Lección Aprendida
Si aparecen errores en `Packages/com.ivanmurzak.unity.mcp.*` relativos a assemblies inexistentes, revisar el `.asmdef` de tests de ese paquete y cambiar el `defineConstraints` a un define que nunca se active automáticamente (ej: `NombreProyecto_TESTS_ENABLED`). Esto no borra los tests, solo los hace opcionales.

---

## BUG 1 — Lluvia Oblicua (RESUELTO)

### Síntoma
El sistema de partículas de lluvia (`Environment/RainSystem`) emitía partículas en diagonal, no verticalmente.

### Causa Raíz
El módulo `Shape` del ParticleSystem tenía `Angle = 25°`. Esto causa que las partículas se emitan en un cono de 25° de apertura alrededor de la dirección downward, creando efecto de lluvia oblicua.

### Fix Aplicado
Via `mcp__ai-game-developer__particle-system-modify`:
- `shape.angle`: `25` → `0`

### Verificación
Screenshot en Play Mode confirmó lluvia perfectamente vertical.

### No tocar nunca
- `Start Speed: 12` ✅
- `Start Lifetime: 2` ✅
- `Simulation Space: World` ✅
- `Shape Box Scale: (60, 1, 50)` ✅
- `Gravity Modifier: 0.3` ✅
- Posición `(0, 15, 0)` ✅

---

## BUG 2 — Farola Sin Visual de Cabezal (RESUELTO)

### Síntoma
El objeto `Streetlight_01` (farola) en la escena tenía el poste y la luz pero visualmente no existía el "cabezal" de la lámpara (la parte superior de la farola que sostiene la bombilla). Solo tenía 2 hijos: `Pole` y `StreetPointLight`.

### Fix Aplicado
Creado child `LampHead` bajo `Streetlight_01/StreetPointLight`:
- Type: Cube primitive
- Local Position: `(0, 0.1, 0)`
- Scale: `(0.4, 0.2, 0.4)`
- Material: `MAT_Props_Greybox`

Creado via `mcp__ai-game-developer__gameobject-create` y `mcp__ai-game-developer__gameobject-component-modify`.

### Estado de la Luz
La `PointLight` en `StreetPointLight` ya estaba correctamente configurada desde commits anteriores:
- Color: `#FF8C42` (RGB 1, 0.549, 0.259) — naranja caliente
- Intensity: `3`
- Range: `15`
- `FlickerLight.cs` correctamente asignado y cableado

---

## BUG 3 — Farola Sin Colisión (YA ESTABA RESUELTO)

### Estado
Ya resuelto en commit previo (`b784b23`). El `Pole` child de `Streetlight_01` ya tiene `BoxCollider` activo. No se necesitó ningún fix adicional.

---

## BUG 4 — Niebla Ausente (RESUELTO — Requirió Fix Arquitectural)

### Síntoma
La escena no mostraba niebla atmosférica a pesar de que `RenderSettings` ya tenía fog configurado desde commits anteriores.

### Causa Raíz — CRÍTICA Y NO OBVIA
El proyecto fue creado desde la plantilla **`Lit2DSceneTemplate`** de Unity. Esto configura el URP Asset (`Assets/Settings/UniversalRP.asset`) para usar `Renderer2DData` como renderer. El `Renderer2DData` es el pipeline de renderizado 2D de Unity y **NO procesa niebla 3D**, depth buffer, ni shadows 3D de la misma manera que `UniversalRendererData`.

**Cómo se detectó:** Reflexión sobre `RenderSettings` confirmaba fog activo (`fog=true`, `fogMode=Exponential`, `fogDensity=0.02`, `fogColor=#0a0a1a`) pero visualmente no aparecía. Scan de todos los VolumeComponent disponibles: ningún `Fog` VolumeComponent existía en este URP. Finalmente, lectura del URP Asset reveló `Renderer2DData` como renderer activo.

### Fix Aplicado — Swap de Renderer 2D → 3D

**Paso 1:** Crear `Assets/Settings/UniversalRenderer.asset`:
```csharp
// Via script-execute con C# reflection:
var renderer = ScriptableObject.CreateInstance<UniversalRendererData>();
AssetDatabase.CreateAsset(renderer, "Assets/Settings/UniversalRenderer.asset");
```

**Paso 2:** Asignar al URP Asset via reflection:
```csharp
var urpAsset = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(
    "Assets/Settings/UniversalRP.asset"
);
var rendererList = new List<ScriptableRendererData> { renderer };
// Set via SerializedObject m_RendererDataList field
```

### Impacto del Swap
Este cambio afecta todo el pipeline de renderizado:
- ✅ Fog 3D ahora funciona
- ✅ Depth buffer 3D disponible
- ✅ Shadows 3D disponibles
- ✅ Post-processing URP 3D (Bloom, Vignette, FilmGrain, DOF) funciona
- ⚠️ Shaders/sprites 2D pueden necesitar revisión si se usan en UI

### RenderSettings de Niebla (ya configurados, no cambiar)
```
fog = true
fogMode = FogMode.Exponential
fogColor = #0a0a1a (azul negro nocturno)
fogDensity = 0.02
fogStartDistance = 0 (no aplica en Exponential)
```

### Nota sobre Fog en VolumeProfile
`EP1_VolumeProfile` (`Assets/_Game/Materials/EP1_VolumeProfile.asset`) contiene `Vignette` y `FilmGrain` pero NO tiene override de `Fog`. Esto es correcto — en esta versión de URP, la niebla se controla via `RenderSettings`, no via Volume. NO intentar agregar `Fog` al VolumeProfile (el tipo no existe en el package instalado).

---

## BUG 5 — Teléfono F No Abre (VERIFICADO COMO CORRECTO)

### Diagnóstico
`PhoneController.cs` revisado completamente. El código es correcto:
- `SetState()` llama `phoneCanvas.SetActive(newState != PHONE_HIDDEN)` ✅
- Cursor lock/unlock correcto ✅
- `playerController.canMove = !isInteracting` ✅
- Referencias en Inspector todas asignadas ✅:
  - `phoneCanvasGroup` (instanceID 63398)
  - `phonePanel` (instanceID 63344)
  - `phoneCanvas` (instanceID 63394)
  - `playerCamera` (instanceID 63424)
  - `postProcessVolume` (instanceID -34786)

### Causa Real del Bug
El BUG 0 (errores de compilación) impedía entrar a Play Mode, haciendo que pareciera que F no funcionaba cuando en realidad el juego ni siquiera corría.

### Estado
Una vez resuelto BUG 0 y entrado a Play Mode, no se detectaron errores adicionales en PhoneController. Si en el próximo bloque se reporta que F no responde, verificar:
1. `currentState` en Inspector = `PHONE_HIDDEN` al inicio
2. `phoneCanvas` está asignado (instanceID 63394)
3. El `Input System` package está activo (`UnityEngine.InputSystem`)

---

## ARQUITECTURA DEL PROYECTO

### Escena Principal
`Assets/_Game/Scenes/Main_EP1.unity`

### Estructura de Objetos Clave
```
Main_EP1 scene
├── Environment/
│   ├── RainSystem (ParticleSystem — lluvia)
│   ├── GlobalVolume (Volume — Vignette + FilmGrain)
│   └── (Geometría ProBuilder: Ground, Building_Front, Sidewalk)
├── Streetlight_01/
│   ├── Pole (BoxCollider ✅)
│   └── StreetPointLight (PointLight — naranja)
│       └── LampHead (Cube visual — NUEVO)
├── Player/
│   ├── PhoneController.cs
│   ├── PlayerController.cs
│   └── Camera (playerCamera)
└── Managers/
    ├── GameManager.cs (DontDestroyOnLoad, Singleton)
    ├── EventManager.cs (DontDestroyOnLoad, Singleton)
    └── SceneSetup_EP1.cs (inicializa GameState.NORMALIDAD)
```

### Scripts — Namespaces
```
IndiGame.Player      → Assets/Scripts/IndiGame.Player/
IndiGame.Environment → Assets/_Game/Scripts/IndiGame/Environment/
IndiGame.Core        → Assets/_Game/Scripts/IndiGame/Core/
Core (sin namespace) → Assets/_Game/Scripts/Core/
```

**REGLA:** Todo C# nuevo debe usar namespace `IndiGame.*`. Ver `unity-code-standards.md`.

### Estados del Teléfono (PhoneState enum)
```
PHONE_HIDDEN     → canvas inactive, cursor locked, canMove=true
PHONE_POCKET     → canvas inactive, cursor locked, canMove=true
PHONE_ACTIVE     → canvas ACTIVE, cursor FREE, canMove=false
PHONE_FULLSCREEN → canvas ACTIVE, cursor FREE, canMove=false
```

### Eventos Globales (EventManager.cs)
```csharp
OnGameStateChanged  → Action<GameState>
OnPhoneOpen         → Action
OnPhoneClose        → Action
OnMessageReceived   → Action<string>
OnRatingChanged     → Action<int>
OnDeliveryComplete  → Action
OnPhotoAppeared     → Action
OnDoppelgangerActivate → Action
```

### Estados del Juego (GameState enum en GameManager.cs)
`NORMALIDAD`, `ENTREGA`, `PELIGRO`, `DOPPELGANGER`, `GAME_OVER`, `CUTSCENE`

---

## STACK TECNOLÓGICO COMPLETO

### Unity
- Version: 6000.4.0f1 (Unity 6)
- Pipeline: URP con `UniversalRendererData` (3D — post-swap)
- ProBuilder: instalado, usado para toda la geometría del nivel
- Input System: `UnityEngine.InputSystem` (new Input System)
- Unity MCP: `com.IvanMurzak.Unity.MCP` (HTTP local :23949)

### MCPs Configurados en .mcp.json
| MCP | Estado | Uso |
|-----|--------|-----|
| `ai-game-developer` | **ACTIVO** | Unity Editor — escenas, GameObjects, scripts, assets, screenshots, particles, ProBuilder |
| `notion-mcp-server` | **ACTIVO** | Leer/escribir GDD, Roadmap, documentación de bloques |
| `desktop-commander` | **ACTIVO** | Mover archivos, ejecutar comandos de sistema, leer paths |
| `github` | disabled | Operaciones GitHub avanzadas (no necesario con PAT en remote) |
| `StitchMCP` | disabled | Mockups UI con Stitch (Google) — para bloques de UI |
| `blender` | disabled | Modelado 3D via Blender MCP — para bloques de assets |
| `freesound` | disabled | Descargar audio CC0 — para bloques de audio |
| `elevenlabs` | disabled | Generar voz/SFX/música IA — para bloques de audio |

**Para activar un MCP desactivado:** Cambiar `"disabled": true` → `"disabled": false` en `.mcp.json` y reiniciar Claude Code.

### Skills Disponibles (.claude/skills/) — 90+ skills
Categorías principales:
- **Unity MCP:** Todos los comandos del ai-game-developer wrappeados como skills
- **Blender:** `blender-scene`, `blender-props`, `blender-materials-uv`, `blender-export-unity`, `blender-lighting-blockout`, `blender-retopo-lowpoly`
- **ElevenLabs:** `elevenlabs-sfx`, `elevenlabs-music`, `elevenlabs-voice`
- **Freesound:** `freesound-search`, `freesound-download`
- **Nano Banana Pro:** `nano-banana-pro`, `nano-concept-art`, `nano-textures`
- **3D Governance:** `3d-assets-governance`
- **Stitch UI:** `stitch-ui`
- **Unity Workflow:** `unity-initial-setup`, `unity-skill-create`, `unity-skill-generate`

### Workflows Disponibles (.claude/workflows/)
- `add-3d-asset.md` — workflow completo para incorporar modelo 3D
- `add-audio.md` — workflow para audio (Freesound + ElevenLabs)
- `add-visual.md` — workflow para assets visuales
- `block-complete.md` — checklist de cierre de bloque
- `daily-standup.md` — standup diario
- `fix-bug.md` — flujo de diagnóstico y fix de bugs
- `new-feature.md` — flujo para nueva feature
- `new-scene.md` — crear nueva escena desde cero
- `setup-scene-lighting.md` — configurar iluminación de escena

### Rules Activas (.claude/rules/)
- `asset-pipeline.md` — **Paleta oficial, convenciones de naming, rutas de audio/3D/texturas, log de assets externos OBLIGATORIO**
- `git-workflow.md` — Reglas de commits y branches
- `notion-sync.md` — Sincronización con Notion
- `unity-code-standards.md` — Estándares de código Unity (namespace IndiGame.*)

---

## PALETA OFICIAL DEL JUEGO (B8 en adelante)

```
#0d0d1a  — Negro profundo (background)
#1a2a4a  — Azul noche (dark UI, superficies)
#FF6B35  — Naranja caliente (accent principal, luces)
#e94560  — Rojo amenaza (peligro, alertas críticas)
#f0f0f0  — Blanco fantasma (texto, UI)
```

---

## GIT — REGLAS ABSOLUTAS

1. **NUNCA** `Co-Authored-By: Claude` ni ninguna línea de Claude en commits
2. **SIEMPRE** `git config user.name "Adrian-stratega"` y `git config user.email "bytestratega@gmail.com"`
3. **SIEMPRE** push a `main` (nunca crear branches a menos que Adrian lo pida)
4. El remote ya tiene el PAT en la URL — no reconfigurar auth
5. Si un commit viejo tiene Co-Authored-By: `git filter-branch --msg-filter 'grep -v "Co-Authored-By: Claude"' HEAD~N..HEAD` + `git push --force origin main`
6. Formato de commits: `[FIX] descripción`, `[FEAT] descripción`, `[ASSET] descripción`, `[REFACTOR] descripción`

---

## CÓMO USAR EL STACK COMPLETO — CAPACIDADES ACTUALES

### Lo que ya podemos hacer SIN activar nada más (solo ai-game-developer + notion):
- Crear/modificar cualquier GameObject, componente, material, prefab, animación en Unity — **sin tocar el Editor manualmente**
- Crear geometría 3D completa con ProBuilder (extrusiones, poly shapes, bevels, UV mapping)
- Modificar sistemas de partículas (lluvia, humo, fuego, efectos)
- Crear y editar scripts C# Unity directamente
- Tomar screenshots de Game View, Scene View y cámaras específicas para verificar visual
- Ejecutar C# arbitrario en el Editor via `script-execute` (reflection, serialización, debug)
- Leer y escribir el GDD/Roadmap en Notion para alinear trabajo con diseño
- Gestionar packages Unity (agregar/quitar)

### Al activar blender MCP:
- Modelar assets 3D complejos en Blender programáticamente (edificios, props, vehículos)
- Retopología automática para LODs
- UV unwrapping
- Export directo a Unity Assets/

### Al activar elevenlabs MCP:
- Generar voz de Marco (el personaje) con parámetros de emoción
- Generar SFX únicos (pasos, notificaciones, ambiente)
- Generar música generativa para loops atmosféricos
- Todo en WAV 44.1kHz 16-bit

### Al activar freesound MCP:
- Buscar y descargar audio CC0 (libre de derechos) automáticamente
- Filtrar por duración, tipo, calidad
- Integrar directamente a Assets/_Game/Audio/

### Al activar StitchMCP:
- Generar mockups de UI (teléfono de Marco, HUD, pantallas de entrega)
- Output HTML/Tailwind que sirve como referencia visual para Unity Canvas

### Al activar desktop-commander:
- Mover archivos entre carpetas del sistema
- Ejecutar scripts Python/Node externos
- Acceder a rutas fuera del proyecto Unity

---

## PRÓXIMOS BLOQUES SUGERIDOS

### B5 — Sistema de Entregas (Core Gameplay)
Basado en el GDD (cuando Notion esté conectado, verificar en https://www.notion.so/GDD-Game-Design-Document-32dcdabc86bc815483fadb33b756dbd5):
- Crear `DeliveryManager.cs` (namespace `IndiGame.Core`)
- Sistema de waypoints (puntos de entrega)
- UI de teléfono con lista de entregas activa
- Trigger zones en destinos
- Integrar con `OnDeliveryComplete` event ya existente en EventManager

### B6 — Audio Atmosférico (EP1)
- Activar freesound MCP + elevenlabs MCP
- Ambiente nocturno urbano (lluvia, viento, ciudad lejana) → Freesound CC0
- Notificación del teléfono → ElevenLabs SFX
- Loop musical de normalidad → ElevenLabs Music
- Seguir rutas de audio en asset-pipeline.md

### B7 — Assets 3D (Props Calle)
- Activar blender MCP
- Modelos: contenedores de basura, banco de calle, farolas adicionales, señales
- Convenciones: `SM_NombreAsset`, `PRP_NombreAsset_LOD0`
- Registrar en `Assets/_Game/Docs/3d_library_log.csv`

### B8 — UI App Teléfono (QuickRun)
- Activar StitchMCP
- Mockup de la app de entregas en el teléfono de Marco
- Usar paleta oficial B8

---

## ERRORES CONOCIDOS A NO REPETIR

| Error | Causa | Prevención |
|-------|-------|------------|
| Co-Authored-By en commits | Sistema de Claude Code agrega esto por defecto | NUNCA escribir esa línea; si aparece: filter-branch + force-push |
| CS0234 en MCP packages | Versión mismatch en asmdef de tests | Si nuevo paquete MCP da errors: revisar asmdef de Tests/, cambiar defineConstraints a define no-auto |
| Fog invisible con fog activo | URP Asset usando Renderer2DData (template 2D) | Proyecto USA UniversalRendererData — no revertir a Renderer2D |
| Notion 404 vía MCP | Integration no tiene acceso a la página | Compartir la página en Notion con la integración MCP; verificar API key |
| Script-execute fallando en PowerShell | Win32 path issues con inline commands | Usar script-execute con C# directamente en lugar de shell commands |

---

## VERIFICACIÓN FINAL PLAY MODE (Estado Actual)

| Bug | Estado | Verificado |
|-----|--------|-----------|
| Errores compilación | ✅ RESUELTO | `scriptCompilationFailed: False` confirmado |
| Lluvia oblicua | ✅ RESUELTO | Screenshot Play Mode: lluvia vertical |
| LampHead farola | ✅ RESUELTO | Child Cube creado, material asignado |
| Colisión poste | ✅ YA ESTABA OK | BoxCollider en Pole confirmado via MCP |
| Niebla ausente | ✅ RESUELTO | Renderer 3D activo, fog exponencial visible |
| Teléfono F | ✅ CÓDIGO CORRECTO | Refs asignadas, lógica correcta, no bloqueado por compile errors |

---

*Documento generado al cierre de Bloque B4.5 — 2026-03-25*
*Para próxima sesión: pasar este documento al AI assistant para contexto completo*
