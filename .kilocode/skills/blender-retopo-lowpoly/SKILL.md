---
name: blender-retopo-lowpoly
description: >
  Use Blender to perform basic retopology and low-poly cleanup on high
  poly or AI-generated meshes, making them suitable for real-time use in
  game engines while preserving overall shape and silhouette.
---

# Blender Retopo Lowpoly Skill

## Goal

Take **high-poly** or AI-generated 3D meshes and:

- Reduce polycount.
- Clean topology.
- Preserve silhouette and key details.
- Prepare them for UVs, materials, and export to Unity.

This is meant for **practical, game-ready retopo**, not film-level perfection.

---

## When to use this skill

Use this skill when:

- A mesh comes from an AI 3D generator or scan and is too heavy.
- Topology is chaotic (many long skinny triangles, noisy surfaces).
- The user wants a “lowpoly / midpoly” version for real-time use.

Do **not** use this skill for:

- Sculpting new high-poly from scratch.
- Micro-optimization down to every edge; focus on a good balance.

---

## Target density and detail

Suggested triangle targets:

- Small props:
  - From >50k tris down to ~2k–5k (depending on visual importance).
- Medium props:
  - From hundreds of thousands down to ~5k–15k.
- Hero pieces:
  - Higher counts acceptable if justified and requested.

Always discuss or infer importance:
- If the prop is background-only, be more aggressive in reduction.
- If the prop is inspected up close, keep more detail around silhouette and key shapes.

---

## Retopo approach

Use a mixture of:

- Automatic decimation:
  - Decimate modifier (Collapse / Planar).
  - Use conservative ratios first (e.g. 0.3–0.5).
- Manual cleanup where necessary:
  - Remove stray geometry.
  - Simplify overly dense areas with little silhouette impact.
  - Merge hidden interior faces that never contribute visually.

Ensure:

- No non-manifold geometry.
- No huge spikes or collapsed faces.
- A relatively uniform triangle size where possible.

---

## UV and materials handoff

This skill can optionally:

- Simplify material slots (merge similar ones).
- Prepare the mesh for a fresh UV unwrap (handled via `blender-materials-uv`).

Do not perform full UV + material setup unless explicitly requested; focus is on geometry.

---

## Interaction with AI-generated assets

When a mesh comes from an AI 3D generator:

- Assume:
  - Topology may be dense and uneven.
  - There may be interior/hidden geometry that can be removed.
- Goals:
  - Keep exterior shape and silhouette.
  - Remove hidden interior volume where it does not impact shading.

---

## Constraints

- Do not over-simplify to the point where the asset looks broken or unrecognizable.
- Keep polycount within a practical range for your target platform (PC horror game by default).
- Avoid adding new high-frequency detail; this skill is about reduction and cleanup.
- Coordinate with `blender-materials-uv` and `blender-export-unity` for downstream steps.
