# B10 — Entorno: Exterior Edificio Normal (Entregas 1 y 2)

**Objetivo:** El espacio exterior donde Marco hace las dos primeras entregas. Debe sentirse como una noche normal de trabajo, antes de que todo se tuerza.
**Herramientas:** Blender MCP para geometría, ProBuilder para suelo extendido.
**Commit esperado:** `B10-COMPLETE: exterior edificio, interphone, ProximityTrigger, InteractionSystem`

---

## FASE 1 — Contexto

El exterior ya tiene: suelo, Building_Front (6 ventanas), Streetlight, RainSystem.
B10 añade: interphone interactivo, sidewalk ampliado, iluminación de portal, sistema de interacción de proximidad, y la variante de color para la Entrega 2.

---

## FASE 2 — Scripts de interacción

### 2.1 InteractionSystem.cs (sistema base para todos los interactuables)
**Ruta:** `Assets/_Game/Scripts/IndiGame/World/InteractionSystem.cs`

```csharp
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace IndiGame.World
{
    public class InteractionSystem : MonoBehaviour
    {
        public static InteractionSystem Instance { get; private set; }

        [SerializeField] private float interactionRange = 2.5f;
        [SerializeField] private TextMeshProUGUI interactionPrompt;
        [SerializeField] private Camera playerCamera;

        private IInteractable currentTarget;

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        void Update()
        {
            CheckForInteractable();
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                TryInteract();
        }

        private void CheckForInteractable()
        {
            if (playerCamera == null) return;
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            currentTarget = null;
            if (interactionPrompt != null) interactionPrompt.gameObject.SetActive(false);

            if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
            {
                var interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    currentTarget = interactable;
                    if (interactionPrompt != null)
                    {
                        interactionPrompt.text = interactable.GetPrompt();
                        interactionPrompt.gameObject.SetActive(true);
                    }
                }
            }
        }

        private void TryInteract()
        {
            currentTarget?.Interact();
        }
    }

    public interface IInteractable
    {
        string GetPrompt();
        void Interact();
    }
}
```

### 2.2 InterphoneInteractable.cs
**Ruta:** `Assets/_Game/Scripts/IndiGame/World/InterphoneInteractable.cs`

```csharp
using UnityEngine;
using IndiGame.Core;

namespace IndiGame.World
{
    public class InterphoneInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private int deliveryIndex = 0;
        [SerializeField] private Light ledLight;

        private int pressCount = 0;
        private bool delivered = false;

        public string GetPrompt() => delivered ? "" : "[E] Llamar al interphone";

        public void Interact()
        {
            if (delivered) return;
            pressCount++;
            if (ledLight != null) ledLight.enabled = true;

            if (pressCount == 1)
            {
                // Primera pulsación: sin respuesta (un poco raro)
                AppNotificationController.NotifyDelivery(deliveryIndex, "INFO");
            }
            else if (pressCount >= 2)
            {
                // Segunda pulsación: confirmación
                delivered = true;
                AppNotificationController.NotifyDelivery(deliveryIndex, "SUCCESS");
                EventManager.Instance?.OnDeliveryCompleted?.Invoke(deliveryIndex);
                if (ledLight != null) ledLight.color = new Color(0f, 1f, 0f, 1f); // verde
            }
        }
    }
}
```

**Nota:** `AppNotificationController.NotifyDelivery` es un método estático helper a añadir en B10 si no existe.

### 2.3 ProximityTrigger.cs
**Ruta:** `Assets/_Game/Scripts/IndiGame/World/ProximityTrigger.cs`

```csharp
using UnityEngine;
using UnityEngine.Events;

namespace IndiGame.World
{
    public class ProximityTrigger : MonoBehaviour
    {
        [SerializeField] private float triggerRadius = 5f;
        [SerializeField] private bool triggerOnce = true;
        [SerializeField] private UnityEvent onEnter;
        [SerializeField] private UnityEvent onExit;

        private Transform player;
        private bool isInside = false;
        private bool hasTriggered = false;

        void Start()
        {
            var playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO == null) playerGO = GameObject.Find("[Player]");
            if (playerGO != null) player = playerGO.transform;
        }

        void Update()
        {
            if (player == null) return;
            if (triggerOnce && hasTriggered) return;
            float dist = Vector3.Distance(transform.position, player.position);
            bool inside = dist <= triggerRadius;
            if (inside && !isInside)
            {
                isInside = true;
                hasTriggered = true;
                onEnter?.Invoke();
            }
            else if (!inside && isInside)
            {
                isInside = false;
                onExit?.Invoke();
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, triggerRadius);
        }
    }
}
```

---

## FASE 3 — Añadir EventManager.OnDeliveryCompleted

Verificar si existe en EventManager. Si no, añadirlo:

```
script-read: Assets/_Game/Scripts/Core/EventManager.cs
```

Si no tiene `OnDeliveryCompleted`, añadirlo con script-update-or-create:
```csharp
// Añadir en EventManager (dentro de la clase):
public System.Action<int> OnDeliveryCompleted;
```

---

## FASE 4 — Construir escena exterior

```csharp
using UnityEngine;
using UnityEditor;
using IndiGame.World;

public class B10Builder
{
    public static string Build()
    {
        var sb = new System.Text.StringBuilder();

        // ─── Interphone en pared del edificio ───
        var building = GameObject.Find("Environment/Building_Front");
        if (building == null) { sb.AppendLine("WARN: Building_Front no encontrado"); }
        else
        {
            var interphone = new GameObject("Interphone");
            interphone.transform.SetParent(building.transform, false);
            // Posición: en la pared, al lado de la entrada
            interphone.transform.localPosition = new Vector3(0.6f, -0.5f, 0.05f);

            // Geometría placeholder (cubo pequeño)
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = "InterphoneBody";
            cube.transform.SetParent(interphone.transform, false);
            cube.transform.localScale = new Vector3(0.15f, 0.25f, 0.04f);
            var mat = AssetDatabase.LoadAssetAtPath<Material>("Assets/_Game/Materials/MAT_DarkWall.mat");
            if (mat != null) cube.GetComponent<Renderer>().sharedMaterial = mat;

            // LED light
            var ledGO = new GameObject("LED");
            ledGO.transform.SetParent(interphone.transform, false);
            ledGO.transform.localPosition = new Vector3(0f, 0.05f, 0.03f);
            var led = ledGO.AddComponent<Light>();
            led.type = LightType.Point;
            led.range = 0.3f;
            led.intensity = 1f;
            led.color = Color.red;
            led.enabled = false;

            // InterphoneInteractable
            var script = interphone.AddComponent<InterphoneInteractable>();
            var so = new UnityEditor.SerializedObject(script);
            so.FindProperty("ledLight").objectReferenceValue = led;
            so.ApplyModifiedProperties();

            // Collider para raycast
            var col = interphone.AddComponent<BoxCollider>();
            col.size = new Vector3(0.2f, 0.3f, 0.1f);

            sb.AppendLine("Interphone creado ✓");
        }

        // ─── ProximityTrigger en la puerta ───
        var triggerGO = new GameObject("DoorProximityTrigger");
        var env = GameObject.Find("Environment");
        if (env != null) triggerGO.transform.SetParent(env.transform, false);
        triggerGO.transform.position = new Vector3(0f, 0f, 2f); // frente a la puerta
        var trigger = triggerGO.AddComponent<ProximityTrigger>();
        var soT = new UnityEditor.SerializedObject(trigger);
        soT.FindProperty("triggerRadius").floatValue = 3f;
        soT.ApplyModifiedProperties();
        sb.AppendLine("DoorProximityTrigger creado ✓");

        // ─── InteractionSystem en Player ───
        var player = GameObject.Find("[Player]/Player");
        if (player != null && player.GetComponent<InteractionSystem>() == null)
        {
            var isys = player.AddComponent<InteractionSystem>();
            // Wire playerCamera
            var cam = player.transform.Find("MainCamera")?.GetComponent<Camera>();
            if (cam != null)
            {
                var soIS = new UnityEditor.SerializedObject(isys);
                soIS.FindProperty("playerCamera").objectReferenceValue = cam;
                soIS.ApplyModifiedProperties();
            }
            sb.AppendLine("InteractionSystem añadido al Player ✓");
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();
        sb.AppendLine("=== B10 BUILDER COMPLETO ===");
        return sb.ToString();
    }
}
```

---

## FASE 5 — Interacción prompt UI

Crear un TextMeshProUGUI para el prompt de interacción encima del crosshair:
- Parent: PhoneCanvas o un nuevo HUDCanvas (sortingOrder=5)
- Posición: centro de pantalla, ligeramente abajo del centro
- Texto: "[E] Llamar al interphone" / "[E] Examinar"
- Color: #f0f0f0, font size 20, outline negro

Wirear con `InteractionSystem.interactionPrompt`.

---

## FASE 6 — Verificación

```
// En Play Mode:
public class B10Verify { public static string Run() {
    var sb = new System.Text.StringBuilder();
    sb.AppendLine("InteractionSystem: " + (IndiGame.World.InteractionSystem.Instance != null ? "OK" : "NULL ❌"));
    var interphone = GameObject.Find("Interphone");
    sb.AppendLine("Interphone en escena: " + (interphone != null ? "OK" : "NULL ❌"));
    return sb.ToString();
}}
```

Caminar hacia el interphone → debe aparecer el prompt "[E] Llamar al interphone".
Presionar E → LED rojo enciende.
Presionar E de nuevo → LED cambia a verde, notificación SUCCESS.

---

## FASE 7 — Commit

```
git add -A
git commit -m "B10-COMPLETE: InteractionSystem, InterphoneInteractable, ProximityTrigger, Interphone en edificio"
git push origin main
```

## Checklist B10
- [ ] InteractionSystem compila y está en el Player
- [ ] ProximityTrigger compila
- [ ] InterphoneInteractable compila
- [ ] Interphone GameObject en la escena con BoxCollider
- [ ] Acercarse al interphone muestra prompt UI
- [ ] E presionado 1 vez: LED rojo
- [ ] E presionado 2 veces: LED verde + notificación SUCCESS
- [ ] 0 errores en consola
