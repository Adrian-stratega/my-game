# B9 — Entorno: Interior del Coche (Escena de Tránsito)

**Objetivo:** Construir el interior del coche donde Marco pasa entre entregas. Primera sensación de "atrapado" y normalidad cómoda.
**Herramientas:** Blender MCP para geometría, Unity MCP para importación y setup.
**Commit esperado:** `B9-COMPLETE: interior coche, bolsa QuickRun, lluvia parabrisas`

---

## FASE 1 — Contexto narrativo

El coche es el hub de tránsito. Marco se sienta, espera la ruta GPS, ve la lluvia en el parabrisas.
- Vista: primera persona desde asiento del conductor
- Solo visible el interior (no exterior del coche — ahorra polígonos)
- Atmósfera: cálida (luces de dashboard ambarinas), lluvia exterior, música ambiental

---

## FASE 2 — Crear geometría con Blender MCP

### 2.1 Componentes del coche interior (crear todos en Blender)

**Dashboard (tablero):**
- Plano curvo de ~1.5m × 0.6m ligeramente inclinado hacia el conductor
- Material base: plástico oscuro `#1a1a1a`
- 8-10 indicadores circulares pequeños (cilindros aplanados) con Point Lights `#d4a853` (ámbar)
- Un indicador naranja de advertencia `#FF6B35` (el de gasolina, siempre encendido)

**Volante:**
- Toro (anillo) de radio mayor 0.18m, radio menor 0.02m
- Columna de dirección: cilindro fino
- Material: plástico oscuro

**Asiento del pasajero:**
- Cuboide básico con subdivisiones para simular acolchado
- Material: tela oscura `#2a2a2a`
- Encima: bolsa de delivery (ver 2.2)

**Parabrisas:**
- Plano inclinado ~15° hacia atrás, ~1.2m × 0.8m
- Material: vidrio semitransparente (MAT_GlassWindow de B8)

**Techo interior:**
- Plano flat con liner de tela gris oscuro

### 2.2 Bolsa de delivery QuickRun
- Mochila/bolsa térmica low-poly (cuboide redondeado)
- Logo QuickRun: texto en relieve o sprite aplicado como decal
- Material: tela oscura con logo naranja `#FF6B35`
- Export: FBX, Forward=-Z, Up=Y, escala Apply=1.0

### 2.3 Export
- Export todos los elementos como `CarInterior.fbx` a `Assets/_External/Blender/Raw/`
- Escala: 1 unidad Blender = 1 metro Unity

---

## FASE 3 — Importar y organizar en Unity

```
assets-refresh → detectar los FBX importados
assets-find: t:Model name:CarInterior
assets-move: mover a Assets/_Game/Prefabs/3D/CarInterior.fbx
```

### 3.1 Crear prefab CarInterior en escena

```csharp
using UnityEngine;
using UnityEditor;

public class B9Builder
{
    public static string Build()
    {
        var sb = new System.Text.StringBuilder();

        // Crear GO raíz para el coche
        var carRoot = new GameObject("CarInterior");
        carRoot.transform.position = Vector3.zero;
        carRoot.SetActive(false); // empieza inactivo — DeliveryManager lo activa

        // Posición de cámara del conductor: en el centro izquierdo
        var driverPos = new GameObject("DriverSeatPosition");
        driverPos.transform.SetParent(carRoot.transform, false);
        driverPos.transform.localPosition = new Vector3(-0.35f, 0f, 0f);

        // Si el FBX fue importado, instanciarlo
        string fbxPath = "Assets/_Game/Prefabs/3D/CarInterior.fbx";
        var model = AssetDatabase.LoadAssetAtPath<GameObject>(fbxPath);
        if (model != null)
        {
            var inst = (GameObject)PrefabUtility.InstantiatePrefab(model, carRoot.transform);
            sb.AppendLine("CarInterior.fbx instanciado ✓");
        }
        else
        {
            sb.AppendLine("WARN: CarInterior.fbx no encontrado — crear geometría placeholder");
            // Placeholder: cubo simple
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = "CarInterior_Placeholder";
            cube.transform.SetParent(carRoot.transform, false);
            cube.transform.localScale = new Vector3(1.5f, 1.2f, 2.5f);
        }

        // Point Lights de dashboard
        for (int i = 0; i < 3; i++)
        {
            var lightGO = new GameObject($"DashLight_{i}");
            lightGO.transform.SetParent(carRoot.transform, false);
            lightGO.transform.localPosition = new Vector3(-0.3f + i * 0.3f, -0.15f, 0.6f);
            var light = lightGO.AddComponent<Light>();
            light.type = LightType.Point;
            light.range = 0.3f;
            light.intensity = 0.5f;
            light.color = new Color(0.83f, 0.67f, 0.33f); // #d4a853
        }
        sb.AppendLine("Dashboard lights ✓");

        // Añadir a la escena bajo Environment
        var env = GameObject.Find("Environment");
        if (env != null) carRoot.transform.SetParent(env.transform, false);

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();
        sb.AppendLine("CarInterior GO creado bajo Environment ✓");
        sb.AppendLine("=== B9 BUILDER COMPLETO ===");
        return sb.ToString();
    }
}
```

---

## FASE 4 — Sistema de lluvia en parabrisas

### Script RainOnWindshield.cs (simple, sin física)
**Ruta:** `Assets/_Game/Scripts/IndiGame/World/RainOnWindshield.cs`

```csharp
using UnityEngine;

namespace IndiGame.World
{
    public class RainOnWindshield : MonoBehaviour
    {
        [SerializeField] private ParticleSystem rainParticles;
        [SerializeField] private Material windshieldMaterial;
        [SerializeField] private float distortionSpeed = 0.5f;

        private float offset = 0f;

        void Update()
        {
            if (windshieldMaterial == null) return;
            offset += Time.deltaTime * distortionSpeed;
            windshieldMaterial.SetFloat("_DistortionOffset", offset % 1f);
        }

        public void SetRainIntensity(float intensity)
        {
            if (rainParticles == null) return;
            var emission = rainParticles.emission;
            emission.rateOverTime = intensity * 50f;
        }
    }
}
```

### Particle System de lluvia en parabrisas
Crear en Blender MCP o directamente en Unity:
- Posición: en frente del parabrisas, proyectando hacia el cristal
- Rate over time: 30
- Tamaño: 0.01-0.03 (gotas pequeñas)
- Velocidad: hacia abajo y ligeramente hacia atrás
- Material: Billboard blanco semitransparente
- Simulación en World Space

---

## FASE 5 — Verificación

```
screenshot-scene-view → seleccionar CarInterior, ver geometría
```
- CarInterior existe bajo Environment (activeSelf=false)
- Dashboard lights encendidos (3 puntos de luz ámbar)
- Parabrisas visible y semitransparente

```
screenshot-game-view → activar CarInterior temporalmente para ver
```

---

## FASE 6 — Commit

```
git add -A
git commit -m "B9-COMPLETE: CarInterior placeholder+lights, RainOnWindshield, bolsa QuickRun setup"
git push origin main
```

## Checklist B9
- [ ] CarInterior GO existe bajo Environment (activeSelf=false)
- [ ] 3 dashboard Point Lights ámbar
- [ ] DriverSeatPosition marker existe
- [ ] RainOnWindshield script compila sin errores
- [ ] CarInterior.fbx en Assets/_Game/Prefabs/3D/ (o placeholder si Blender MCP no disponible)
- [ ] 0 errores en consola
