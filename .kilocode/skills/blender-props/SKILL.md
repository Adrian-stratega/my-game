---
name: blender-props
description: >
  Use the Blender MCP server to create and edit individual 3D props
  (chairs, bags, electronics, furniture, etc.) that are game-ready for
  Unity or other engines. Always respect Unity scale, clean topology,
  correct pivots, and reasonable polycount.
---

# Blender Props Skill

## Goal

Model and edit **single 3D props** in Blender via the Blender MCP server
so they can be safely used in real-time engines (Unity, Unreal, Godot).

This includes:
- Furniture (chairs, tables, sofas)
- Everyday objects (delivery bags, interphones, food containers, phones)
- Small environment props (trash cans, lamps, mailboxes)
- Simple hero props (if polycount and detail are under control)

---

## When to use this skill

Use this skill when:

- The user asks for a **specific object** (e.g. “delivery bag”, “intercom device”, “kitchen chair”).
- The prop will be reused across scenes.
- You need to adjust scale, pivot, or basic topology of an existing prop.

Do **not** use this skill for:

- Whole scenes or rooms → use `blender-scene`.
- Heavy retopology on high-poly scans/IA meshes → use a retopo-specific skill.
- Complex character rigs or animations.

---

## Unity scale and proportions

Always model props with Unity scale in mind:

- 1 Blender unit = 1 meter (Unity meter).
- Examples:
  - Standard chair seat height: ~0.45m, total height: ~0.9m.
  - Intercom box: ~0.25m tall, ~0.1m wide, ~0.05m deep.
  - Delivery bag: ~0.4m × 0.4m × 0.3m.

Before export (handled by export skill):

- All transforms **applied** (scale = 1, rotation = 0,0,0).
- Model centered relative to its pivot rules.

---

## Pivot rules

Set the object origin/pivot de forma coherente:

- Objects that sit on the floor:
  - Pivot at the **center of the bottom** on the ground plane (Z=0).
  - Example: chairs, tables, trash cans, delivery bag on the floor.
- Wall-mounted objects:
  - Pivot on the **back center face** that touches the wall.
  - Example: interphones, wall lamps, switches.
- Handheld props:
  - Pivot at a logical grip point or object center, depending on usage.

These pivots facilitate placing and aligning props within Unity without weird adjustments.

---

## Topology and polycount

Aim for game-ready topology:

- Use quads where possible, but triangles are acceptable in moderation.
- Avoid long, skinny triangles and chaotic n-gons.
- Keep polycount **reasonable**:
  - Small simple props: < 1k triangles.
  - Medium props (chairs, tables, cabinets): 1k–5k triangles.
  - Only exceed this if explicitly requested for high-detail close-ups.

Use **modifiers** (Bevel, Subdivision) non-destructively while modeling, but:

- Apply them **before export**.
- Keep bevel widths small and segment counts low.

---

## LOD guidelines (basics)

For important props, prepare simple LODs:

- LOD0: full detail (original model).
- LOD1: ~50% triangles vs LOD0.
- LOD2: ~20–25% triangles vs LOD0.

Naming:

- `PRP_<Name>_LOD0`
- `PRP_<Name>_LOD1`
- `PRP_<Name>_LOD2`

Place them in a collection:

- `COL_<Name>_LODs`

Unity can handle LODs via a LODGroup configured later.

---

## Naming conventions

Object names:

- Base object: `PRP_<Name>_LOD0`
  - Ej: `PRP_DeliveryBag_LOD0`, `PRP_Interphone_LOD0`
- Optional sub-mesh pieces:
  - `PRP_DeliveryBag_Zipper`, `PRP_Chair_Backrest`

Collection for the prop:

- `PRP_<Name>_COL` (ej: `PRP_DeliveryBag_COL`)

Keep names clear and without spaces.

---

## Interaction with Meshy / IA 3D

When the project uses external IA 3D tools (e.g. Meshy):

- This skill is responsible for:
  - Fixing pivots.
  - Adjusting scale to Unity units.
  - Doing minimal cleanup (remove tiny stray pieces, unify orientation).
  - Optional simple LODs.

Do **not** try to re-sculpt or massively retopo heavy IA meshes here;
that belongs to a retopo-specific skill.

---

## Example tasks

Given a request:

> “Create a delivery bag prop, sized realistically, with a flat bottom and rectangular shape.”

This skill should:

1. In Blender, create a cube and scale it to ~0.4m × 0.3m × 0.4m.
2. Add edge loops to shape the top lid and bottom.
3. Optionally add simple strap geometry.
4. Set pivot at bottom center.
5. Name the object `PRP_DeliveryBag_LOD0`.
6. Place it in `PRP_DeliveryBag_COL`.

---

## Constraints

- Always respect Unity scale.
- Do not create extremely detailed geometry where a simple shape suffices.
- Avoid overlapping or non-manifold geometry.
- Do not handle UV or materials in depth here (just basic placeholders); delegate to materials/UV skills.
