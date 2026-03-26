# B8 — Arte: Materiales, Paleta Global y Volume Profiles

**Objetivo:** Definir y aplicar la paleta visual ANTES de construir entornos. Sin B8 todo se ve gris y genérico.
**Commit esperado:** `B8-COMPLETE: materiales URP, paleta global, 5 Volume Profiles`

---

## FASE 1 — Diagnóstico

```
console-get-logs (Error) → 0 errores
assets-find: t:VolumeProfile → ver cuántos hay (probablemente 0)
assets-find: t:Material name:MAT_ → ver si ya hay materiales del proyecto
```

---

## FASE 2 — Crear materiales URP/Lit

Usar `assets-material-create` para cada material. Luego `assets-modify` para configurar las propiedades.

### Materiales a crear en `Assets/_Game/Materials/`

| Nombre | Base Color | Roughness | Metallic | Emissive |
|--------|-----------|-----------|----------|----------|
| MAT_WetAsphalt | #1a1a1a | 0.9 | 0.0 | - |
| MAT_Concrete | #c8c8c0 | 0.85 | 0.0 | - |
| MAT_WoodFloor | #8b5a2b | 0.7 | 0.0 | - |
| MAT_WallPaint | #e8e4da | 0.95 | 0.0 | - |
| MAT_DarkWall | #1a2a4a | 0.8 | 0.0 | - |
| MAT_StreetLight | #FF6B35 | 0.3 | 0.8 | #FF6B35 intensity 2.0 |
| MAT_GlassWindow | #88aabb | 0.1 | 0.0 | Alpha=0.3 (Surface: Transparent) |
| MAT_CarpetOld | #5a4a3a | 1.0 | 0.0 | - |
| MAT_Ground | #2a2a2a | 0.95 | 0.0 | - |
| MAT_SkyNight | #0d0d1a | 1.0 | 0.0 | - |

**Shader para todos (excepto Glass):** `Universal Render Pipeline/Lit`
**Shader Glass:** `Universal Render Pipeline/Lit` con Surface Type = Transparent

Crear con script-execute usando `AssetDatabase.CreateAsset`:

```csharp
using System;
using UnityEngine;
using UnityEditor;

public class B8MaterialBuilder
{
    public static string Build()
    {
        var sb = new System.Text.StringBuilder();
        string folder = "Assets/_Game/Materials";
        if (!AssetDatabase.IsValidFolder(folder))
            AssetDatabase.CreateFolder("Assets/_Game", "Materials");

        var shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) return "ERROR: URP/Lit shader no encontrado";

        // Define: name, baseColor hex, roughness, metallic, emissiveHex, emissiveIntensity
        var mats = new (string name, string hex, float rough, float metal, string emHex, float emInt)[]
        {
            ("MAT_WetAsphalt",  "#1a1a1a", 0.9f,  0f, null, 0f),
            ("MAT_Concrete",    "#c8c8c0", 0.85f, 0f, null, 0f),
            ("MAT_WoodFloor",   "#8b5a2b", 0.7f,  0f, null, 0f),
            ("MAT_WallPaint",   "#e8e4da", 0.95f, 0f, null, 0f),
            ("MAT_DarkWall",    "#1a2a4a", 0.8f,  0f, null, 0f),
            ("MAT_StreetLight", "#FF6B35", 0.3f,  0.8f, "#FF6B35", 2f),
            ("MAT_CarpetOld",   "#5a4a3a", 1.0f,  0f, null, 0f),
            ("MAT_Ground",      "#2a2a2a", 0.95f, 0f, null, 0f),
            ("MAT_SkyNight",    "#0d0d1a", 1.0f,  0f, null, 0f),
        };

        foreach (var (name, hex, rough, metal, emHex, emInt) in mats)
        {
            string path = $"{folder}/{name}.mat";
            var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat == null)
            {
                mat = new Material(shader);
                AssetDatabase.CreateAsset(mat, path);
            }
            Color baseColor;
            ColorUtility.TryParseHtmlString(hex, out baseColor);
            mat.SetColor("_BaseColor", baseColor);
            mat.SetFloat("_Smoothness", 1f - rough);
            mat.SetFloat("_Metallic", metal);
            if (emHex != null)
            {
                Color emColor;
                ColorUtility.TryParseHtmlString(emHex, out emColor);
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", emColor * emInt);
                mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.BakedEmissive;
            }
            EditorUtility.SetDirty(mat);
            sb.AppendLine($"{name} ✓");
        }

        // MAT_GlassWindow con transparencia
        string glassPath = $"{folder}/MAT_GlassWindow.mat";
        var glass = AssetDatabase.LoadAssetAtPath<Material>(glassPath);
        if (glass == null) { glass = new Material(shader); AssetDatabase.CreateAsset(glass, glassPath); }
        Color glassColor;
        ColorUtility.TryParseHtmlString("#88aabb", out glassColor);
        glassColor.a = 0.3f;
        glass.SetColor("_BaseColor", glassColor);
        glass.SetFloat("_Surface", 1f); // Transparent
        glass.SetFloat("_Smoothness", 0.9f);
        glass.renderQueue = 3000;
        glass.SetOverrideTag("RenderType", "Transparent");
        EditorUtility.SetDirty(glass);
        sb.AppendLine("MAT_GlassWindow (Transparent) ✓");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        sb.AppendLine("=== B8 MATERIALS COMPLETO ===");
        return sb.ToString();
    }
}
```

---

## FASE 3 — Crear 5 Volume Profiles

Crear en `Assets/_Game/VolumeProfiles/`:

```csharp
using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class B8VolumeBuilder
{
    public static string Build()
    {
        var sb = new System.Text.StringBuilder();
        string folder = "Assets/_Game/VolumeProfiles";
        if (!AssetDatabase.IsValidFolder(folder))
            AssetDatabase.CreateFolder("Assets/_Game", "VolumeProfiles");

        // (name, temperature, contrast, saturation, grainIntensity, vignetteIntensity)
        var profiles = new (string name, float temp, float contrast, float sat, float grain, float vignette)[]
        {
            ("VolumeProfile_Normalidad",   20f,  -5f,  0f,  0.1f, 0.15f),
            ("VolumeProfile_Anomalia",      0f,   0f,  0f,  0.3f, 0.25f),
            ("VolumeProfile_Infiltracion", -20f,  0f, -10f, 0.5f, 0.35f),
            ("VolumeProfile_Revelacion",   -30f, 20f,  0f,  0.7f, 0.4f),
            ("VolumeProfile_Final",         20f,  0f, -30f, 0.2f, 0.2f),
        };

        foreach (var (name, temp, contrast, sat, grain, vignette) in profiles)
        {
            string path = $"{folder}/{name}.asset";
            var vp = AssetDatabase.LoadAssetAtPath<VolumeProfile>(path);
            if (vp == null)
            {
                vp = ScriptableObject.CreateInstance<VolumeProfile>();
                AssetDatabase.CreateAsset(vp, path);
            }

            // Color Adjustments
            ColorAdjustments ca;
            if (!vp.TryGet(out ca)) ca = vp.Add<ColorAdjustments>(false);
            ca.colorFilter.overrideState = false;
            ca.contrast.overrideState = true; ca.contrast.value = contrast;
            ca.saturation.overrideState = true; ca.saturation.value = sat;

            // White Balance
            WhiteBalance wb;
            if (!vp.TryGet(out wb)) wb = vp.Add<WhiteBalance>(false);
            wb.temperature.overrideState = true; wb.temperature.value = temp;

            // Film Grain
            FilmGrain fg;
            if (!vp.TryGet(out fg)) fg = vp.Add<FilmGrain>(false);
            fg.intensity.overrideState = true; fg.intensity.value = grain;
            fg.type.overrideState = true; fg.type.value = FilmGrainLookup.Thin1;

            // Vignette
            Vignette vig;
            if (!vp.TryGet(out vig)) vig = vp.Add<Vignette>(false);
            vig.intensity.overrideState = true; vig.intensity.value = vignette;
            vig.smoothness.overrideState = true; vig.smoothness.value = 0.5f;

            EditorUtility.SetDirty(vp);
            sb.AppendLine($"{name} ✓");
        }

        AssetDatabase.SaveAssets();
        sb.AppendLine("=== B8 VOLUME PROFILES COMPLETO ===");
        return sb.ToString();
    }
}
```

---

## FASE 4 — Aplicar materiales a la escena actual

```csharp
using UnityEngine;
using UnityEditor;

public class B8ApplyMaterials
{
    public static string Apply()
    {
        var sb = new System.Text.StringBuilder();
        string folder = "Assets/_Game/Materials";

        Material Load(string n) => AssetDatabase.LoadAssetAtPath<Material>($"{folder}/{n}.mat");

        // Ground
        var ground = GameObject.Find("Environment/Ground");
        if (ground != null) { var r = ground.GetComponent<Renderer>(); if (r != null) r.sharedMaterial = Load("MAT_Ground"); sb.AppendLine("Ground ✓"); }

        // Building_Front
        var building = GameObject.Find("Environment/Building_Front");
        if (building != null) { var r = building.GetComponent<Renderer>(); if (r != null) r.sharedMaterial = Load("MAT_Concrete"); sb.AppendLine("Building_Front ✓"); }

        // Windows
        for (int i = 0; i < 6; i++)
        {
            var win = GameObject.Find($"Environment/Building_Front/Window_{i}");
            if (win != null) { var r = win.GetComponent<Renderer>(); if (r != null) r.sharedMaterial = Load("MAT_GlassWindow"); }
        }
        sb.AppendLine("Windows (glass) ✓");

        // Streetlight
        var lampHead = GameObject.Find("Environment/Streetlight_01/StreetPointLight/LampHead");
        if (lampHead != null) { var r = lampHead.GetComponent<Renderer>(); if (r != null) r.sharedMaterial = Load("MAT_StreetLight"); sb.AppendLine("LampHead emissive ✓"); }

        AssetDatabase.SaveAssets();
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();
        sb.AppendLine("=== B8 APPLY COMPLETO ===");
        return sb.ToString();
    }
}
```

---

## FASE 5 — Verificación

```
screenshot-scene-view → verificar que la escena se ve con los materiales aplicados
screenshot-game-view → verificar look nocturno con paleta oscura
```

Buscar visualmente:
- Suelo: negro/oscuro (MAT_Ground)
- Edificio: gris concreto (MAT_Concrete)
- Ventanas: azul semitransparente (MAT_GlassWindow)
- LampHead: naranja brillante emisivo (MAT_StreetLight)

---

## FASE 6 — Commit

```
git add -A
git commit -m "B8-COMPLETE: 10 materiales URP, 5 Volume Profiles narrativos, paleta visual aplicada a escena"
git push origin main
```

## Checklist B8
- [ ] 10 materiales en Assets/_Game/Materials/
- [ ] 5 VolumeProfiles en Assets/_Game/VolumeProfiles/
- [ ] MAT_GlassWindow es transparente (Surface=Transparent)
- [ ] MAT_StreetLight tiene emissive naranja visible
- [ ] Materiales aplicados a Ground, Building, Windows, LampHead
- [ ] Screenshot muestra paleta oscura correcta
- [ ] 0 errores de compilación
