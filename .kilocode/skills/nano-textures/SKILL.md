---
name: nano-textures
description: >
  Use Nano Banana Pro 2 to generate PBR textures, albedo maps, normal
  maps, and UI elements. Focus on seamless tiling, correct aspect ratios,
  and Unity-friendly formats. Leverage grounding and multi-turn editing
  for perfection.
---

# Nano Banana Pro 2 Textures Skill

## Goal

Generate perfect **PBR textures and UI assets** for Unity:

- Albedo seamless.
- Normal maps.
- Roughness/Metallic/AO.
- UI elements (buttons, icons).

---

## Prompting for textures

**Albedo seamless**:
PBR albedo texture [material], seamless tileable, [resolution] square,
studio lighting, no shadows, high detail, game-ready.

Example: “Wet asphalt road PBR albedo texture, seamless tileable, nighttime reflections, 2K square, studio lighting, game-ready.”

**Normal map**:
PBR normal map [material], seamless tileable, [resolution] square,
blue/purple colorspace, high detail, game-ready normal map.

---

## Output Formats

- Preferred formats for Unity: PNG or TGA.
- Keep power-of-two resolutions (512, 1024, 2048).
- Organize in `Assets/_Game/Textures/<Category>/`.

---

## Constraints

- Always ensure textures are seamless if intended for tiling.
- Use standard PBR naming: `T_<Name>_D`, `T_<Name>_N`, etc.
- Multi-turn editing can be used to refine textures (e.g., "Add more cracks", "Make it wetter").
