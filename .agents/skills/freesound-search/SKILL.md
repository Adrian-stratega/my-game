---
name: freesound-search
description: >
  Search Freesound.org via the Freesound MCP server for high-quality,
  CC0-licensed sound effects and ambience suitable for games and media.
  Always filter for Creative Commons 0 and return engine-friendly metadata.
---

# Freesound Search Skill

## Goal

Use the Freesound MCP server (Freesound API v2) to find **CC0-licensed** sounds
that are directly usable in game engines (Unity, Unreal, Godot, etc.).

This includes:
- Foley and SFX (footsteps, doors, UI, impacts, machinery)
- Ambience loops (streets, rain, rooms, wind)
- Short musical loops only if explicitly requested (but primary focus is SFX/ambience)

---

## When to use this skill

Use this skill when the user wants:
- To *discover* or shortlist sounds from Freesound
- Metadata and preview URLs for multiple candidates
- License-safe results (CC0 only by default)

Do **not** use this skill when:
- The user wants to generate new audio → use ElevenLabs skills
- The user wants to download a specific known sound ID → use a dedicated download flow or another tool if available

---

## License and legal constraints

For commercial games and client work, always prefer **CC0**:

- CC0 = safe for commercial use, no attribution required (but you can still credit)
- CC BY = allows commercial use but requires attribution
- CC BY-NC = **not allowed** in paid games or monetized projects

For this project, default behavior:
- Filter **only** `license:"Creative Commons 0"` in all searches.
- Never propose NC-licensed or unclear-licensed sounds by default.

Only deviate from CC0 if the user explicitly, knowingly requests another license.

---

## Query construction

Use the Freesound **text search** endpoint (via MCP).

Base parameters to set conceptually (the MCP server abstracts the HTTP):

- `query` or `q`: text query
- `filter`: always includes `license:"Creative Commons 0"`
- Optional filter additions:
  - Duration:
    - `duration:[0 TO 5]` for short one-shots
    - `duration:[3 TO 20]` for ambiences
  - File type or tags if available in the MCP
- `page_size`: 20–50 results per query
- `sort`: `score_desc` or `rating_desc` by default

When forming **text queries**:
- Use concise, descriptive English terms:
  - “rain on window interior”
  - “city street at night distant traffic”
  - “elevator door close”
  - “footsteps wood corridor reverb”
- Avoid adding “CC0” or “free” in the text query; rely on license filters instead.

---

## Result format

From the MCP, request/expect fields like:

- `id`
- `name`
- `duration`
- `license`
- `url`
- `previews` (especially `preview-hq-mp3` or `preview-hq-ogg`)
- `tags`

When presenting results to the user, prefer a compact table:

- `id`: Freesound sound ID
- `name`: short name
- `duration`: seconds
- `tags`: key tags (subset)
- `preview_url`: one preview URL
- `license`: should be CC0 for all

Use this to let the user shortlist sounds before any download.

---

## Game-audio-specific guidelines

When suggesting or ranking sounds:

- Prefer:
  - Clean recordings with minimal background noise
  - Reasonable dynamic range (not completely crushed)
  - Duration appropriate for use (not 5 min for a door sound)
- For loops:
  - Look for tags like `loop`, `seamless`, `ambience`
  - Duration typically 10–60s
- For one-shots:
  - Duration 0.1–3s, depending on type (footstep vs long impact tail)

If the MCP exposes preview URLs, always include them to allow quick listening.

---

## Usage patterns

1. Take the user's description of the needed sound.
2. Convert it into a focused English query.
3. Add license and basic duration filtering.
4. Request 20–50 results.
5. Present the best 5–10 candidates with preview URLs and metadata.

---

## Constraints

- Always filter by `license:"Creative Commons 0"` by default.
- Do not suggest NC (non-commercial) content for paid or monetized games.
- Do not assume audio quality; rely on tags, duration, and, if exposed, ratings.
- When in doubt about license, drop that result.
