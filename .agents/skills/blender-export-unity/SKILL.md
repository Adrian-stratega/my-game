---
name: blender-export-unity
description: >
  Export Blender scenes and props to Unity-ready FBX/GLB using best
  practices from 2026 Blender-to-Unity workflows. Apply transforms,
  enforce correct axis settings (Forward -Z, Up Y), smooth shading, and
  place files into engine-friendly folders.
---

# Blender Export to Unity Skill

## Goal

Handle the **export** of Blender content to Unity using clean, repeatable settings.

This includes:

- Applying scale and rotation correctly.
- Using proper axis settings for Unity.
- Choosing FBX/GLB formats and options that preserve hierarchy and materials.
- Saving files into consistent project folders.

---

## When to use this skill

Use this skill when:

- A scene or prop in Blender is ready to be brought into Unity.
- The user asks to “export this environment/prop to Unity”.
- You need to ensure consistent export settings across the project.

Do **not** use this skill to:

- Create or modify geometry (use scene/props skills).
- Handle texturing or material creation inside Unity.

---

## Export format and axis settings

Default export format:

- **FBX** (recommended for Unity).
- GLB/GLTF can be used if explicitly requested, but FBX is the standard.

Axis conventions:

- Forward = **-Z**
- Up = **Y**

Scale:

- Apply object scale so that exported scale is **1.0**.
- Apply rotation so orientation is neutral (0,0,0).

Before export:

- Select only the relevant objects/collections (no reference geometry).
- Apply:
  - Location (if needed for local objects)
  - Rotation
  - Scale

---

## FBX export checklist

For standard static meshes (environments, props):

- Apply transforms:
  - In Blender: `Ctrl+A` → Rotation & Scale.
- FBX options:
  - Apply Unit: ON (if needed, but ensure final scale is 1 in Unity).
  - Forward: `-Z Forward`
  - Up: `Y Up`
  - Apply Scale: `FBX All` or equivalent recommended setting.
  - Include:
    - Mesh
    - (Optionally) Armatures if needed, but for static scenes usually only Mesh.
  - Smoothing:
    - Set to “Face” or “Normals Only” per best practice; ensure correct smoothing in Unity.
  - Leaf bones:
    - Disabled if not needed.

---

## Export paths (Unity project structure)

Use clear, consistent paths inside the Unity project (relative):

- Scenes / environments:
  - `Assets/_Game/Scenes/3D/SCN_<SceneName>.fbx`
    - Ej: `Assets/_Game/Scenes/3D/SCN_Street_Anomalous.fbx`
- Props:
  - `Assets/_Game/Prefabs/3D/Props/PRP_<Name>.fbx`
    - Ej: `Assets/_Game/Prefabs/3D/Props/PRP_DeliveryBag.fbx`
- Shared sets (if any):
  - `Assets/_Game/Prefabs/3D/Sets/<SetName>.fbx`

The exported FBX file names must match the root collection or main object name when possible.

---

## Export workflow example

For an environment:

1. In Blender:
   - Ensure the scene has a root collection named `SCN_Street_Anomalous`.
   - Place all exportable geometry in that collection or a child collection.
   - Remove or disable reference objects (player dummies, cameras, etc.) from the export selection.

2. Apply transforms:
   - Select all relevant objects.
   - Apply rotation and scale.

3. Export:
   - Use FBX exporter with:
     - Selected objects only.
     - Forward = -Z, Up = Y.
     - Apply scale appropriately.

4. Save as:
   - `Assets/_Game/Scenes/3D/SCN_Street_Anomalous.fbx`

For a prop:

1. Ensure prop object (or collection) is named `PRP_<Name>`.
2. Pivot is correctly set.
3. Apply transforms.
4. Export selected as FBX to:
   - `Assets/_Game/Prefabs/3D/Props/PRP_<Name>.fbx`

---

## Interaction with Unity MCP

After export:

- The Unity MCP (ai-game-developer) can:
  - Import and place the FBX in a scene.
  - Create prefabs from the imported FBX.
  - Assign materials from the project’s palette.

This export skill does **not** perform Unity operations; it only ensures exported assets are clean and predictable.

---

## Constraints

- Never export reference or helper objects (player dummies, cameras, lights) unless explicitly requested.
- Always apply rotation and scale before export.
- Keep export settings consistent across the project; do not randomly change axis or scale settings.
- Ensure file names and folder paths follow the agreed conventions.
