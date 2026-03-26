---
name: blender-lighting-blockout
description: >
  Use Blender to create simple lighting blockouts for scenes, focusing on
  mood and readability rather than final game lighting. Provide reference
  setups that help guide Unity's final lighting and post-processing.
---

# Blender Lighting Blockout Skill

## Goal

Create **lighting blockouts** in Blender to:

- Explore mood and contrast.
- Check readability and silhouette in key areas.
- Provide reference for final lighting in Unity.

This is **not** final game lighting; it is a visual reference.

---

## When to use this skill

Use this skill when:

- A scene’s geometry is mostly done and you want to test lighting ideas.
- You need quick previews of “how it might feel” before setting up Unity lights.
- You want to communicate lighting intent (where to place main lights, accents, etc.).

Do **not** use this skill for:

- Production-quality game lighting or baked lightmaps in Unity.
- Detailed performance tuning of lights.

---

## Lighting principles

Focus on:

- **Readability**:
  - Player paths visible.
  - Interactable objects not lost in darkness.
- **Mood**:
  - Horror, tension, safety, etc., depending on scene.
- **Composition**:
  - Key light directions and contrast between lit and shadowed areas.

Use simple light types:

- Area lights, point lights, spotlights as needed.
- Avoid complex setups with too many lights.

---

## Typical setups

For exterior night street:

- One or two main street lamps (Area/Spot + Point) near the player path.
- Dim skylight or world background for subtle ambient.
- Avoid pure black shadows; keep minimal bounce to see silhouettes.

For interior small apartment:

- Single main ceiling light per room (Area/Point).
- Optional warm accent lights (lamps, TV glow, window light).
- Keep contrast so corners and depths feel uneasy but still readable.

---

## Interaction with materials

For blockout:

- You can use simple diffuse materials with low/medium roughness.
- Do not over-focus on exact colors; concentrate on brightness and contrast.
- You may assign simple emissive materials to windows/screens to indicate light sources.

---

## Output / references for Unity

When the lighting blockout is ready:

- Provide:
  - Screenshots from key player viewpoints.
  - Description of light types and positions:
    - “Main street lamp ~4m above ground, warm light, intensity high.”
    - “Ambient night fill very low, blue tint.”

These references help recreate an approximate lighting setup inside Unity with URP + post-processing.

---

## Constraints

- Do not attempt to deliver final production lighting here.
- Keep setups simple and interpretable.
- Avoid heavy render settings that slow down previews unnecessarily.
