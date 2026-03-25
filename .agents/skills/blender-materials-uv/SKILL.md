---
name: blender-materials-uv
description: >
  Use the Blender MCP server to create clean UV maps and assign
  engine-friendly materials for meshes that will be exported to Unity
  or other real-time engines. Focus on simple atlases, lightmap-ready
  UVs, and material naming that maps to the project's palette.
---

# Blender Materials & UV Skill

## Goal

Prepare **UVs and materials** in Blender so meshes are ready for:

- Texturing (PBR, baking) and
- Use in Unity (URP) or similar engines.

This includes:
- Unwrapping meshes into clean UV islands
- Minimizing stretching and overlaps
- Setting up simple atlases when useful
- Assigning materials with names that map to the project’s palette

---

## When to use this skill

Use this skill when:

- A scene or prop is already modeled and needs UVs/materials before export.
- You need consistent material naming that maps to a defined palette (e.g. B8).
- You want basic lightmap-friendly UVs.

Do **not** use this skill for:

- High-end texture painting or detailed baking passes (that’s a texturing/pipeline step).
- Sculpting or geometry creation.

---

## UV unwrapping guidelines

General rules:

- No overlapping UV islands for **lightmap UVs**.
- Minimal overlap for **texture UVs**, unless tiling is intentional.
- Avoid extreme stretching.

For simple architectural meshes:

- Use **Smart UV Project** or **Mark Seams + Unwrap**:
  - Mark seams along natural edges (e.g. corners where walls meet).
  - Keep islands mostly rectangular for walls/floors.

For props:

- Mark seams along invisible or less visible edges (back/bottom).
- Group logically connected surfaces into islands.

UV space usage:

- Fill the 0–1 UV square as much as possible without overlap.
- Leave small padding between islands (sufficient for mipmaps/bakes).

---

## Lightmap UVs

If a second UV channel for lightmaps is required:

- Create a separate UV map (e.g., `UVMap_Lightmap`).
- Ensure:
  - No overlaps.
  - Enough padding between islands.
  - Uniform texel density where feasible.

Do not overcomplicate lightmap UVs; focus on correctness and non-overlap.

---

## Material naming and palette mapping

Use **project-specific names** that map to your global palette.

Example palette:

- `MAT_Asphalt_Wet`
- `MAT_Concrete_Exterior`
- `MAT_Wall_Interior`
- `MAT_Wood_Floor`
- `MAT_Tile_Kitchen`
- `MAT_Metal_Door`
- `MAT_Glass_Window`

Rules:

- Only create materials that correspond to **real, planned materials**.
- Do not create random, one-off materials with arbitrary names.
- Reuse materials across meshes whenever possible.

These names must correspond 1:1 with materials in Unity URP, so that:

- Upon importing the FBX, Unity can recognize and link materials easily.
- There are no hundreds of duplicate materials.

---

## Atlas usage (optional)

For groups of small props:

- You may group them onto a shared UV atlas to reduce draw calls.
- In that case:
  - Pack multiple props into a single 0–1 UV space.
  - Use one material (e.g. `MAT_Props_Small_01`).
  - Maintain a simple layout so later texturing tools can paint on the atlas.

Only atlas when it truly helps and the user requests it or context suggests it.

---

## Interaction with texturing/baking pipeline

This skill doesn’t perform the actual baking, but must prepare the mesh for:

- Albedo, Normal, Roughness, Metallic, AO, etc.
- Consistent UVs for each type of bake.

Ensure:

- UVs are clean.
- No overlapping for channels that will receive baked maps.
- Island orientation is sensible (not randomly rotated) when important.

---

## Example tasks

Given:

> “Prepare UVs and materials for a small apartment kitchen: walls, floor, cabinets, table.”

This skill should:

1. Identify all kitchen meshes.
2. Create/assign materials:
   - `MAT_Wall_Interior`
   - `MAT_Tile_Kitchen`
   - `MAT_Wood_Floor`
   - `MAT_Furniture_Wood`
3. Mark seams and unwrap:
   - Walls with vertical seams at corners.
   - Floor as a single big island (if possible).
   - Cabinets and table with seams on back/bottom edges.
4. Pack UV islands into 0–1 space.
5. Create a second UV map for lightmaps if needed (`UVMap_Lightmap`) with no overlaps.

---

## Constraints

- Do not create excessive material variants; reuse palette materials.
- No overlapping UVs in lightmap UV sets.
- Avoid hidden or tiny islands that contribute little to visual quality.
- Keep UV layouts understandable and maintainable.
