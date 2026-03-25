---
name: freesound-download
description: >
  Download selected Freesound CC0 sounds via the Freesound MCP server
  and organize them into a reproducible, engine-friendly folder structure
  (e.g. Unity Assets/_Game/Audio/SFX). Ensure licenses are respected and
  provenance is logged.
---

# Freesound Download & Library Skill

## Goal

Given one or more Freesound sound IDs (already vetted via search),
download the corresponding audio files (or highest-quality previews if
that is what the MCP provides), save them into the project, and log:

- File path
- Freesound URL
- License
- Original name and tags

This creates a small, reproducible *sound library* for the project.

---

## When to use this skill

Use this skill when:

- The user has chosen specific Freesound IDs or links.
- A shortlist from the `freesound-search` skill needs to be imported.
- You want to script a batch download for several sounds.

---

## License handling

- Before downloading, verify that the sound is **CC0**.
- If license is not CC0, **do not download** unless the user explicitly understands and accepts different licensing.
- For CC0:
  - Attribution is not required, but keeping provenance is recommended.

---

## File organization (Unity-focused)

Default target paths:

- SFX:
  - `Assets/_Game/Audio/SFX/<category>/<descriptive_name>_fs<id>.wav`
- Ambience:
  - `Assets/_Game/Audio/Ambience/<location>_<mood>_fs<id>.wav`
- UI:
  - `Assets/_Game/Audio/SFX/UI/<ui_event>_fs<id>.wav`

Where:
- `<category>` might be `Doors`, `Footsteps`, `Impacts`, `UI`, `Weather`, etc.
- `<descriptive_name>` is a cleaned version of the Freesound name (lowercase, spaces to `_`, remove special chars).
- `fs<id>` is the Freesound ID to preserve traceability.

Additionally, create/update a CSV or Markdown log (optional but recommended):

- `Assets/_Game/Audio/freesound_library_log.csv`

Columns:
- `engine_path`
- `freesound_id`
- `freesound_url`
- `license`
- `original_name`
- `duration`
- `tags`

---

## Implementation patterns

The details depend on how the Freesound MCP exposes download URLs and audio previews. Typical pattern:

1. Use MCP to retrieve metadata for the sound ID (including preview URL and license).
2. Confirm license is CC0.
3. Download the best-quality preview or original file via HTTP.
4. Write it to the structured path.
5. Append a row to the log file.

### Example: Python (conceptual download from preview URL)

```python
import csv
import os
import requests

sound_id = 123456  # example ID
preview_url = "https://freesound.org/data/previews/123/123456_..._hq.mp3"
license_str = "Creative Commons 0"
engine_rel_path = "Assets/_Game/Audio/SFX/Doors/door_heavy_close_fs123456.mp3"
freesound_url = f"https://freesound.org/s/{sound_id}/"
original_name = "heavy_metal_door_close"
duration = 2.7
tags = ["door", "metal", "close"]

# Download file
os.makedirs(os.path.dirname(engine_rel_path), exist_ok=True)
res = requests.get(preview_url)
res.raise_for_status()
with open(engine_rel_path, "wb") as f:
    f.write(res.content)

# Update log
log_path = "Assets/_Game/Audio/freesound_library_log.csv"
log_exists = os.path.exists(log_path)

with open(log_path, "a", newline="", encoding="utf-8") as csvfile:
    writer = csv.writer(csvfile)
    if not log_exists:
        writer.writerow([
            "engine_path", "freesound_id", "freesound_url",
            "license", "original_name", "duration", "tags"
        ])
    writer.writerow([
        engine_rel_path, sound_id, freesound_url,
        license_str, original_name, duration, " ".join(tags)
    ])

print(f"Downloaded and logged {engine_rel_path}")
```

---

## Constraints

- Only download sounds whose license is CC0 by default.
- Preserve the Freesound ID in filenames or logs.
- Do not modify the raw audio at download time (no normalization/EQ here); post-processing happens in the DAW or engine.
- Keep the structure and log consistent to avoid duplicates and confusion.
