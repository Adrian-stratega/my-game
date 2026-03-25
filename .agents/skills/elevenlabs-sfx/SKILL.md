---
name: elevenlabs-sfx
description: >
  Use the ElevenLabs Text-to-Sound-Effects API to generate high-quality,
  game-ready sound effects (SFX, Foley, UI, ambience) for games, films,
  and interactive experiences. Prefer precise prompts, short durations,
  and formats compatible with real-time engines like Unity and Unreal.
---

# ElevenLabs SFX Skill

## Goal

Create short, high‑quality sound effects using the ElevenLabs **Text to Sound Effects** API and save them in engine‑friendly formats and paths (e.g. Unity / Unreal assets).

This includes:
- One‑shot SFX (UI clicks, doors, impacts, horror stingers)
- Foley (footsteps, cloth, object handling)
- Short ambience loops (2–20s beds for rooms, locations, machines)
- Variations of the same sound (pitch, texture, intensity)

---

## When to use this skill

Use this skill when the user wants:
- Sound effects, Foley, UI sounds, ambience beds, whooshes, hits, risers
- Short audio (seconds, not full songs) for interactive media
- Engine‑ready assets (Unity, Unreal, Godot, etc.)

Do **not** use this skill when the user asks for:
- Full music tracks or themes → use the ElevenLabs Music skill
- Voice lines or dialogue → use the ElevenLabs Voice skill
- Pure editing of existing audio (EQ, compression, etc.) without generation

---

## API usage

Use the official ElevenLabs **Text-to-Sound-Effects** API.

Base endpoint (HTTP):
- `POST https://api.elevenlabs.io/v1/sound-generation`

Headers:
- `xi-api-key: <ELEVENLABS_API_KEY>` (read from environment or config)
- `Content-Type: application/json`

Important request fields (JSON body):

- `text` (string, required)  
  Natural‑language description of the sound.

- `duration_seconds` (number, optional)  
  Target length of the generated sound in seconds.  
  Use:
  - 1–4s for one‑shots (UI, hits, stingers)
  - 3–8s for loops and ambience beds
  - 8–20s only when explicitly requested

- `output_format` (string, optional)  
  Recommended defaults:
  - `wav_44100` for master and in‑engine SFX
  - `mp3_44100_128` for previews or web

- `prompt_influence` (number 0–1, optional)  
  How literally the model follows the prompt.  
  - 0.3–0.5 for natural variation  
  - 0.6–0.9 for very literal sounds

- `model_id` (string, optional)  
  Default recommended: `eleven_text_to_sound_v2`  
  Use other official IDs only if the docs explicitly recommend them.

The response returns binary audio data. Save it to disk with the requested extension.

---

## Prompting guidelines

Always write **precise, production‑oriented prompts**.  
Avoid vague prompts like “scary sound” or “cool effect”.

Recommended prompt structure:

- Source: what is making the sound  
  - “old wooden door”, “metal elevator”, “wet footsteps”, “neon sign”
- Action: what happens  
  - “slowly creaking open”, “closing hard”, “soft landing”
- Environment: where / room / exterior  
  - “small apartment hallway”, “underground parking lot”
- Distance & perspective:  
  - “close‑mic’d”, “medium distance”, “far in the background”, “stereo”
- Mood & intensity (optional):  
  - “subtle”, “cinematic”, “high impact”
- Exclusions if needed:  
  - “no music”, “no voices”

Examples of good prompts:

- “wet footsteps on concrete at night, medium distance, city ambience low in background, stereo, no music”
- “old wooden door slowly creaking open, close mic, small apartment interior, soft reverb, no music”
- “short UI notification ping, modern phone app style, clean, soft, not harsh, no reverb”

---

## Engine-friendly file paths

When saving audio files, prefer clear, engine‑friendly paths.  
Examples for Unity:

- `Assets/_Game/Audio/SFX/UI/ui_notification_ping.wav`
- `Assets/_Game/Audio/SFX/Doors/door_wood_creak_open_01.wav`
- `Assets/_Game/Audio/SFX/Footsteps/footsteps_wet_concrete_01.wav`
- `Assets/_Game/Audio/Ambience/room_small_night_bed_01.wav`

Name variations with numeric suffixes (`_01`, `_02`, `_03`) instead of changing prompt drastically.

---

## Implementation patterns

When generating SFX from Antigravity, prefer **scripts using the official ElevenLabs SDK** when available, or plain HTTP if not.

### Example: Python (sound effect)

```python
import os
import requests

api_key = os.environ.get("ELEVENLABS_API_KEY")
assert api_key, "ELEVENLABS_API_KEY not set"

url = "https://api.elevenlabs.io/v1/sound-generation"
headers = {
    "xi-api-key": api_key,
    "Content-Type": "application/json",
}

payload = {
    "text": "old wooden apartment door slowly creaking open, close mic, small hallway, subtle reverb, no music",
    "duration_seconds": 4.0,
    "output_format": "wav_44100",
    "prompt_influence": 0.6,
    "model_id": "eleven_text_to_sound_v2",
}

response = requests.post(url, headers=headers, json=payload)
response.raise_for_status()

output_path = "Assets/_Game/Audio/SFX/Doors/door_wood_creak_open_01.wav"
os.makedirs(os.path.dirname(output_path), exist_ok=True)

with open(output_path, "wb") as f:
    f.write(response.content)

print(f"Saved SFX to {output_path}")
```

---

## Constraints

- Do **not** fabricate unsupported parameters; only use fields documented in the official API.
- Do **not** generate excessively long SFX unless explicitly requested.
- Prefer `wav_44100` for final in‑engine assets.
- Keep prompts safe and in line with ElevenLabs content policies.
- For multiple variations, slightly tweak the prompt or `prompt_influence`, but keep the same core description.
