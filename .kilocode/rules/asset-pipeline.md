---
trigger: always_on
---

# asset-pipeline.md
# LAST DELIVERY — Pipeline de Assets

## Paleta oficial del proyecto (B8 en adelante)
#0d0d1a — Negro profundo (background)
#1a2a4a — Azul noche (dark UI, superficies)
#FF6B35 — Naranja caliente (accent principal, luces)
#e94560 — Rojo amenaza (peligro, alertas críticas)
#f0f0f0 — Blanco fantasma (texto, UI)

## Audio

### ElevenLabs MCP (voz, SFX, música)
Usar skills: elevenlabs-sfx.md | elevenlabs-music.md | elevenlabs-voice.md
- Voz de Marco: Audio/Voice/Marco/
- SFX puntuales: Audio/SFX/
- Música: Audio/Music/
- Generar siempre 3–4 variaciones y seleccionar la mejor.
- Formato de salida: WAV, 44.1kHz, 16-bit mínimo.

### Freesound MCP (ambientes CC0)
Usar skills: freesound-search.md | freesound-download.md
- SOLO licencia CC0. Rechazar cualquier resultado que no sea CC0, sin excepciones.
- Loops ambientales: Audio/SFX/Ambient/
- Filtrar siempre por: license="Creative Commons 0", duration<120s para loops.
- Al descargar: mover con Desktop Commander a la ruta correcta.
- Ejecutar Unity MCP assets-refresh después de mover.

### Rutas de audio
Assets/_Game/Audio/SFX/          — efectos puntuales
Assets/_Game/Audio/SFX/Ambient/  — loops ambientales
Assets/_Game/Audio/Music/        — música generativa
Assets/_Game/Audio/Voice/Marco/  — líneas de voz

## Modelos 3D

### Blender MCP
Usar skills: blender-scene.md | blender-props.md | blender-materials-uv.md |
             blender-export-unity.md | blender-lighting-blockout.md | blender-retopo-lowpoly.md
- Para: arquitectura, entornos, greyboxing, geometría de nivel.
- Export: FBX, Forward -Z, Up Y, escala Apply = 1.0.
- Destino: Assets/_External/Blender/Raw/ → luego mover a _Game/Prefabs/3D/

### Meshy AI (solo desde web manual)
Usar skill: 3d-assets-governance.md
- NO tiene API en free tier. Usar solo desde interfaz web.
- Licencia: CC BY 4.0 — atribución OBLIGATORIA en créditos del juego.
- Usar para: props secundarios únicamente (bolsas, vasos, objetos genéricos).
- Descargar FBX/GLB → mover con Desktop Commander a Assets/_External/Meshy/Raw/
- Registrar en 3d_library_log.csv SIEMPRE antes de usar en una escena.

### Texturas PBR (orden de búsqueda obligatorio)
1. Poly Haven (https://polyhaven.com/textures) — CC0, descargar manual
2. ambientCG (https://ambientcg.com) — CC0, manual
3. Nano Banana Pro 2 — generar con nano-textures.md skill
4. Stability AI web — solo si las anteriores no tienen lo necesario
Destino: Assets/_Game/Textures/PBR/

### Concept art y referencias visuales
- Nano Banana Pro 2 (integrado en Antigravity): nano-concept-art.md
- Stability AI web: backup
- Destino referencias: Assets/_Game/Textures/Concept/

### UI app QuickRun
- Stitch MCP: stitch-ui.md (400+ daily credits)
- Output: HTML/Tailwind/tokens → referencia visual para Unity Canvas
- Paleta B8 obligatoria en todos los mockups

## Convenciones de naming (obligatorias)
Props:        PRP_[NombreCamelCase]_LOD[0-2]   ej: PRP_DeliveryBag_LOD0
Static Mesh:  SM_[NombreCamelCase]             ej: SM_BuildingFacade
Texturas:     T_[Nombre]_[Albedo|Normal|ORM]   ej: T_WetAsphalt_Albedo
Materiales:   MAT_[NombreCamelCase]            ej: MAT_WetAsphalt
Audio SFX:    SFX_[nombre_snake_case]          ej: SFX_phone_notification_ping
Audio Music:  MUS_[nombre_snake_case]          ej: MUS_normalidad_loop
Audio Voice:  VOX_[nombre_snake_case]          ej: VOX_marco_llegada_01

## Log de assets externos (obligatorio)
Archivo: Assets/_Game/Docs/3d_library_log.csv
Columnas: asset_name, source, license, attribution_text, date_added, used_in_scene

Registrar TODO asset que venga de Meshy, Poly Haven, ambientCG, Freesound o cualquier fuente externa.
Sin registro en el log = asset NO puede usarse en una escena.

## Workflow de incorporación de cualquier asset
1. Obtener asset (Blender MCP / Meshy web / Freesound MCP / Poly Haven manual)
2. Mover a carpeta _External/ correspondiente con Desktop Commander
3. Registrar en 3d_library_log.csv
4. Procesar si necesario (retopo, UV, escala)
5. Mover a _Game/ carpeta final
6. Unity MCP assets-refresh
7. Crear Prefab con nombre correcto
8. Commit [ASSET]
9. Marcar ✅ en Notion