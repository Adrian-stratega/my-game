---
name: nano-banana-pro
description: >
  Use Nano Banana Pro 2 (Gemini 3.1 Flash Image Preview) for high-fidelity
  image generation: concept art, UI mockups, textures, reference images,
  diagrams. Leverage thinking, grounding, multi-turn editing, and 4K
  resolution for production-ready assets. Unlimited via Antigravity.
---

# Nano Banana Pro 2 Skill

## Goal

Generate **high-quality images** using Nano Banana Pro 2 integrated into Antigravity for:

- Concept art and mood boards.
- UI mockups (phone, QuickRun app).
- Base/reference textures.
- Reference images (props, environments).
- Technical diagrams (game flow, systems).

---

## When to use this skill

Use when:

- Need **quick visualization** of ideas.
- Concept art for environments, props, characters.
- UI mockups before Stitch.
- Precise photographic references.
- Base textures to be refined later in external tools.

**Do NOT use for**:

- 3D Assets → Meshy/Blender skills.
- Audio → ElevenLabs/Freesound skills.
- Code → programming tools.

---

## Available Models and When to Use Them

- **Nano Banana Pro 2** (`gemini-3.1-flash-image-preview`):
  - **Default for everything**. High quality, automatic thinking, grounding.
  - 4K, multi-turn editing, perfect for professional assets.

- **Nano Banana Pro** (`gemini-3-pro-image-preview`):
  - Slower but better for text rendering and complex instructions.

- **Nano Banana** (`gemini-2.5-flash-image`):
  - Fast for sketches, previews, multiple iterations.

Always specify the model in the prompt if it's not the default.

---

## Prompting Strategies

### Base Structure
[Subject] [Style] [Details] [Lighting/Composition] [Technical]

Examples for LAST DELIVERY:

- Concept art: “Psychological horror delivery app UI, dark mode, corporate cheerful tone, GPS screen with anomalous route, iPhone mockup, high detail, 4K.”
- Texture: “Wet asphalt road texture, night lighting, subtle reflections, PBR albedo map, 2K, seamless tileable.”
- Prop reference: “Realistic plastic intercom box 1970s style, wall-mounted, silver metal, buttons, studio photography, clean background.”

### Powerful Technical Keywords

- Resolution: `4K`, `2K`, `1K`, `512`.
- Aspect ratio: `16:9`, `9:16` (mobile), `1:1`, `4:3`.
- Style: `photorealistic`, `concept art`, `PBR texture`, `UI mockup`, `studio photography`.
- Lighting: `studio lighting`, `golden hour`, `night city`, `interior horror`.
- Composition: `isometric`, `top-down`, `45 degree`, `minimal background`.

### Thinking Control

- `thinkingLevel: High` for complex scenes.
- `includeThoughts: true` for reasoning and debugging.

---

## Workflow for Unity Assets

1. **Generation**:
   - Specific prompt with technical keywords.
   - `imageSize: 2K/4K`, `aspectRatio: 1:1` for textures.

2. **Post-processing**:
   - Import into Photoshop/GIMP for cleanup.
   - Convert to Unity formats (PNG, power-of-2 if needed).

3. **Integration**:
   - `Assets/_Game/Textures/Concept/<name>.png`
   - `Assets/_Game/Textures/PBR/<name>_D.png`
   - Register in governance log if it's a master asset.

---

## Specific Examples: LAST DELIVERY

### QuickRun UI
Modern delivery app GPS screen, dark theme (#0d0d1a background, #FF6B35 accents),
iPhone 14 frame, night mode, anomalous route shown, 5-star rating visible,
corporate cheerful typography, screenshot style, 16:9, 2K.

### Asphalt Texture
Wet asphalt road surface, nighttime street lighting, subtle oil puddles,
PBR albedo map, seamless tileable, high detail, 2K square, top-down view.

### Prop Reference
Realistic delivery bag, black nylon material, rectangular shape,
standing on concrete floor, studio photography, clean white background,
sharp focus, multiple angles if possible.

---

## Multi-turn Editing (Key)

Nano Banana Pro 2 shines in **conversation**:
Turn 1: "Psychological horror delivery app splash screen..."
[generates image 1]

Turn 2: "Same style, now GPS tab, add anomalous route marker..."
[edits image 1 → image 2]

Always iterate this way to refine.

---

## Grounding with Google Search

For precise references:
Enable Google Search grounding, generate "1970s plastic intercom box,
exact replica of common apartment model in Bolivia, photorealistic."

The model will search for real images and use them as a base.

---

## Constraints

- Always specify **aspect ratio** and **resolution**.
- For textures: `seamless tileable`, `PBR albedo/normal`.
- No prohibited content (Gemini policy).
- Iterate with multi-turn to perfect.
- Register important assets in governance log.
