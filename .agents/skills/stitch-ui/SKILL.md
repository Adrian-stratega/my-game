---
name: stitch-ui
description: >
  Use the Stitch MCP to generate UI screens from design DNA and project
  context. Optimized for mobile apps (QuickRun delivery app), consistent
  dark theme, corporate cheerful tone, and Tailwind/CSS output for
  Unity Canvas or web prototypes.
---

# Stitch UI Skill

## Goal

Generate **UI screens** using Stitch MCP, maintaining consistency with:

- Official game palette (B8): `#0d0d1a` (bg), `#1a2a4a` (dark), `#FF6B35` (orange), `#e94560` (red), `#f0f0f0` (text).
- Tone: **corporate cheerful** scaling to menacing.
- Platform: **mobile-first** (iPhone/Android).
- Output: Tailwind/CSS ready for Unity Canvas or web mockups.

---

## When to use this skill

Use when:

- Need mockups or designs for the QuickRun app (tabs: Chat, GPS, Gallery, Rating).
- Iterating on specific screens.
- Generating layout or color variants.

Do **not** use for:

- State logic (that's code).
- Unity integration (that's Unity MCP).

---

## Stitch MCP capabilities

Stitch MCP allows:

- **Read design DNA** from the Stitch project.
- **Generate new screens** from text + code context.
- Output:
  - Component tree.
  - Tokens (colors, spacing, typography).
  - **Tailwind CSS**, HTML, or React code.

---

## Mandatory Palette and Design System

**Colors** (B8):

- Background: `#0d0d1a` (deep black)
- Dark UI: `#1a2a4a` (night blue)
- Accent primary: `#FF6B35` (hot orange)
- Danger: `#e94560` (menace red)
- Text: `#f0f0f0` (ghost white)

**Typography**:

- System fonts: `-apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto`.
- Sizes: 12px (small), 16px (body), 20px (titles), 24px (headers).
- Weights: 400 (normal), 500 (medium), 600 (semi-bold).

**Spacing**: Multiples of 4px (4, 8, 12, 16, 20, 24…).

**Radius**: 12px for buttons/cards, 8px for small elements.

---

## Prompting for Stitch

**Structure**:
Mobile app screen [name], [state], [devices], palette [#0d0d1a bg, #FF6B35 accent], Tailwind output.

Examples:

- “Delivery app GPS screen, night mode, iPhone 14, dark theme #0d0d1a background, orange accents #FF6B35, anomalous route shown, 5-star rating, Tailwind CSS + HTML.”
- “QuickRun chat tab, new message notification, corporate cheerful, unread badge pulsing, dark UI, Tailwind output.”

**Key keywords**:

- “Mobile app”, “iPhone 14 frame”, “dark theme”.
- “Tailwind CSS”, “responsive”, “design system”.
- States: “normal”, “anomalous”, “low rating”, “new notification”.

---

## Expected Output

Stitch should return:

1. **HTML + Tailwind CSS** ready for preview.
2. **Tokens JSON** (colors, spacing used).
3. **Component tree** (for reference).

---

## Unity Integration

Stitch output → manual steps:

1. Use HTML/CSS as **visual reference**.
2. Translate to **Unity Canvas** (RectTransform, Image, TextMeshPro).
3. Use **exact palette** in Unity materials.
4. Implement dynamic states in code.

---

## Constraints

- Always dark theme with B8 palette.
- Mobile-first, iPhone 14 proportions.
- Tailwind output for easy preview.
- Do not generate logic; visual only.
