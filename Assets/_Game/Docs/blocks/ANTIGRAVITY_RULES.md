# LAST DELIVERY — Reglas de Antigravity para todos los bloques

## Identidad
Eres el agente de desarrollo de LAST DELIVERY, un juego de terror psicológico en Unity 6 (URP 3D).
Lee el spec del bloque que te indiquen y ejecútalo completamente, sin saltarte pasos.

## Stack técnico
- Unity 6 (6000.4.0f1) + URP 3D (UniversalRenderer)
- Input System nuevo (Keyboard.current, NOT Input.GetKey)
- Namespaces: IndiGame.Core | IndiGame.UI | IndiGame.Phone | IndiGame.World
- Todos los Managers: DontDestroyOnLoad, Singleton pattern
- Todos los singletons: `public static T Instance { get; private set; }`

## Arquitectura Canvas (NO cambiar nunca)
| Canvas | SortingOrder | Contenido |
|--------|-------------|-----------|
| PhoneCanvas | 10 | PhonePanel (teléfono visible) |
| GameOverCanvas | 100 | GameOverPanel (independiente del teléfono) |
| LoadingCanvas | 150 | LoadingPanel |
| TransitionCanvas | 200 | FadePanel (negro puro) |

**CRÍTICO:** GameOverPanel NO puede estar dentro de PhoneCanvas. Si se desactiva PhoneCanvas, el GameOver debe seguir visible.

## Workflow por bloque (SIEMPRE en este orden)
1. `console-get-logs (Error)` → confirmar 0 errores antes de empezar
2. `scene-get-data` → verificar jerarquía actual
3. Leer scripts existentes relevantes con `script-read`
4. Crear scripts con `script-update-or-create`
5. Esperar compilación → `console-get-logs (Error)` → 0 errores
6. Construir escena con `script-execute` (SIEMPRE usar SerializedObject para wirear [SerializeField])
7. Verificar en Play Mode con `script-execute` (singletons, funciones clave)
8. `screenshot-game-view` para evidencia visual
9. Salir de Play Mode
10. `git commit` vía desktop-commander o script-execute con Process

## Reglas de script-execute
- La clase DEBE tener un método estático que retorne string
- NUNCA usar top-level statements
- Para wirear campos privados [SerializeField]: `new SerializedObject(comp)` + `FindProperty("fieldName")` + `ApplyModifiedProperties()`
- Si script-execute falla con CS error: leer el error completo, corregir, reintentar

## Git
- user.name: Adrian-stratega
- user.email: bytestratega@gmail.com
- Rama: main
- Formato de commit: `B[N]-COMPLETE: descripción corta`
- NUNCA usar Co-Authored-By ni mencionar IA en commits

## Errores frecuentes a evitar
- `Coroutine couldn't be started because GO is inactive` → el GO padre estaba inactivo cuando se llamó StartCoroutine. Asegurar activeInHierarchy=true antes.
- `PhoneController not found` → este script se crea en B7. No referenciarlo antes.
- `SocketException / HubConnection` en consola → IGNORAR, son reconexiones MCP transitorias.
- `trajectory converted to zero chat messages` → el prompt era demasiado largo. Dividir en fases.

## Paleta de colores oficial
- `#0d0d1a` Negro profundo (background)
- `#1a2a4a` Azul noche (UI dark)
- `#FF6B35` Naranja caliente (accent principal)
- `#e94560` Rojo amenaza (peligro, alertas)
- `#f0f0f0` Blanco fantasma (texto)
