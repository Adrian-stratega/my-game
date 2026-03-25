---
name: elevenlabs-music
description: >
  Use the ElevenLabs Music API to generate background music, ambience,
  and cinematic cues. Follow best practices for composition plans, loop
  length, mood, tempo, and export formats suitable for games and media.
---

# ElevenLabs Music Skill

## Goal

Generate high‑quality music and ambience using the ElevenLabs **Music** API.

This includes:
- Loopable background tracks for games (tension, calm, exploration)
- Cinematic cues and stingers (short transitions, jumpscares, scene changes)
- Longer pieces for trailers or menus

---

## When to use this skill

Use this skill when the user wants:
- Music, ambience, or musical soundscapes
- Tracks of 30s–10min suitable for looping or scoring
- Specific moods (tension, relief, melancholy, etc.), genres, BPM, and keys

Do **not** use this skill when the user wants:
- Short one‑shot SFX or Foley → use ElevenLabs SFX skill
- Spoken dialogue or voiceover → use ElevenLabs Voice skill

---

## API flow

Follow the recommended **two‑step flow** from the ElevenLabs docs:

1. **Create a composition plan**  
   Endpoint (HTTP):  
   - `POST https://api.elevenlabs.io/v1/music/plan`  
   or via SDK: `music.composition_plan.create(...)`  

   Important fields:
   - `prompt` (string, required)  
     Detailed description of the desired music.
   - `music_length_ms` (number, required)  
     Total length in milliseconds (e.g. 60000 for 60s).
   - Additional fields if documented (e.g. style, genre, tempo hints).

   The result is a composition plan object you can pass to the next step.

2. **Compose the music from the plan**  
   Endpoint (HTTP) or SDK:  
   - `music.compose(...)` or `music.compose_detailed(...)`  

   Provide:
   - The `composition_plan` from step 1, or
   - A structured prompt, when supported by the SDK.

The response returns audio data (commonly MP3/OGG) plus metadata (BPM, key, sections) when using detailed composition.

---

## Prompting best practices

Follow ElevenLabs best practices for music prompts:

Prompt structure:

- Genre & style:
  - “dark minimal ambient”, “cinematic hybrid”, “lo‑fi”, “post‑rock”
- Mood:
  - “tense”, “melancholic”, “hopeful”, “cold and clinical”
- Tempo & rhythm:
  - “60 BPM”, “slow pulse”, “no percussion”, “subtle rhythmic pulse”
- Harmony:
  - “in D minor”, “modal, eerie chords”, “simple repetitive progression”
- Instrumentation:
  - “low drones”, “distant piano”, “high string harmonics”, “sub bass”
- Usage context:
  - “loopable background for game level”, “menu theme”, “trailer climax”

Examples of strong prompts:

- “dark minimal ambient in D minor, 60 BPM, subtle low drones and distant piano hits, no drums, designed to loop for a psychological horror game”
- “slow evolving ambient texture, 70 BPM, in F minor, low strings and noise beds, no melodic hooks, for tension under dialogue”

---

## Loop length guidelines

For game loops:

- 30–90 seconds is usually enough to loop seamlessly.
- Use `music_length_ms` between `30000` and `90000` unless a different length is explicitly requested.
- Avoid long intros and outros; mention in the prompt:
  - “short intro, quickly reach main loop section, minimal ending”

---

## Engine-friendly file paths

Examples for Unity:

- `Assets/_Game/Audio/Music/menu_theme_70bpm_Am.mp3`
- `Assets/_Game/Audio/Music/loop_tension_dminor_60bpm_01.mp3`
- `Assets/_Game/Audio/Music/stinger_jump_scare_01.mp3`

Prefer:
- Lowercase names
- `_` as separator
- `_01`, `_02`, etc. for variations

---

## Implementation patterns

Prefer using the official ElevenLabs SDK when available, otherwise basic HTTP.

### Example: Python (plan + compose)

```python
import os
import requests

api_key = os.environ.get("ELEVENLABS_API_KEY")
assert api_key, "ELEVENLABS_API_KEY not set"

base_url = "https://api.elevenlabs.io/v1"
headers = {
    "xi-api-key": api_key,
    "Content-Type": "application/json",
}

# 1) Create composition plan
plan_payload = {
    "prompt": (
        "dark minimal ambient in D minor, 60 BPM, subtle low drones, "
        "distant piano hits, no drums, loopable background for horror game"
    ),
    "music_length_ms": 60000  # 60 seconds
}

plan_res = requests.post(f"{base_url}/music/plan", headers=headers, json=plan_payload)
plan_res.raise_for_status()
composition_plan = plan_res.json()

# 2) Compose from plan
compose_payload = {
    "composition_plan": composition_plan
}

compose_res = requests.post(f"{base_url}/music/compose", headers=headers, json=compose_payload)
compose_res.raise_for_status()

output_path = "Assets/_Game/Audio/Music/loop_tension_dminor_60bpm_01.mp3"
os.makedirs(os.path.dirname(output_path), exist_ok=True)

with open(output_path, "wb") as f:
    f.write(compose_res.content)

print(f"Saved music track to {output_path}")
```

---

## Constraints

- Use only parameters and flows documented in the official Music API.
- Prefer lengths between 30s and 90s for loopable in‑game tracks.
- Respect content policies; do not generate prompts that violate ElevenLabs terms.
- Always specify mood, tempo, and instrumentation; avoid vague prompts.
- For trailers or long pieces, explicitly increase `music_length_ms` and mention sections in the prompt (intro, build, climax, outro).
