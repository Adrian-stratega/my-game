---
name: elevenlabs-voice
description: >
  Use the ElevenLabs Text-to-Speech (TTS) API to generate dialogue,
  narration, and character voices. Follow best practices for stability,
  style prompting, and file organization suitable for games and media.
---

# ElevenLabs Voice Skill

## Goal

Generate high‑quality voice lines using the ElevenLabs **Text-to-Speech** API (v3 or latest stable), suitable for:

- In‑game dialogue
- Narration and internal monologue
- System / app voices
- Characters with different styles and emotions

---

## When to use this skill

Use this skill when the user asks for:
- Spoken lines, dialogue scripts, narration, character voices
- Multiple takes or variations of the same line
- Different voice styles (calm, angry, whispering, robotic, corporate)

Do **not** use this skill when the user needs:
- Pure sound effects → use ElevenLabs SFX skill
- Music or ambience → use ElevenLabs Music skill

---

## API usage

Use the official ElevenLabs Text‑to‑Speech API.

Typical HTTP endpoint (check docs for exact path/version):

- `POST https://api.elevenlabs.io/v1/text-to-speech/{voice_id}`

Headers:
- `xi-api-key: <ELEVENLABS_API_KEY>`
- `Content-Type: application/json`

Important fields (JSON body):

- `text` (string, required)  
  The text to be spoken. Can include style cues if supported.

- `model_id` (string, optional)  
  Use the recommended TTS model ID from the official docs (e.g. `eleven_multilingual_v2` or newer).

- `voice_settings` (object, optional)  
  Tuning parameters such as `stability`, `similarity_boost`, and others as documented.  
  Examples:
  - `stability`: controls expressiveness vs predictability
  - Use moderate values for game dialogue unless the user requests otherwise.

Output format is typically audio binary (mp3, wav, etc.) depending on the docs and settings.

When available, prefer the official ElevenLabs SDK (e.g. `client.text_to_speech.convert`) instead of manual HTTP.

---

## Best practices (from official guidance)

Follow official best‑practice recommendations:

- Keep text natural and clear; avoid overloading with symbols or markup that is not documented.
- Use **stable settings** for production dialogue to avoid unexpected variations.
- If style/emotion tags are supported, use only those documented in ElevenLabs guides; do not invent arbitrary tags.
- Break long monologues into smaller lines if needed for timing and in‑engine control.

---

## Style & emotion prompting

When specifying how a line should be delivered:

- Describe:
  - Character (age, gender, accent, personality)
  - Emotion (calm, scared, angry, exhausted, cheerful)
  - Context (whispering in a hallway, talking over the phone, corporate voice notification)
- Use a consistent style per character to keep identity recognizable.

Examples of intent definitions (to be captured in comments or surrounding context, not necessarily inside the text):

- “Young adult male, Latin American accent, tired but professional, low energy internal monologue.”
- “Corporate app voice, neutral, cheerful, slightly patronizing, clear diction, no strong emotion.”

If the API supports inline style tags, only use tags from the official docs (e.g. `[whisper]`, `[shouting]`, etc., if documented). Do not invent custom tags.

---

## Engine-friendly file paths and naming

For game projects (e.g. Unity), use consistent naming:

- `Assets/_Game/Audio/VO/Marco/marco_line_001_intro_hello.wav`
- `Assets/_Game/Audio/VO/App/app_warning_low_rating_01.wav`
- `Assets/_Game/Audio/VO/Narrator/narrator_opening_01.wav`

Patterns:

- Include character or role (`Marco`, `App`, `Narrator`, `Customer`)
- Use incremental line IDs (`001`, `002`, …)
- Optionally include scene / context (e.g. `_delivery1_`, `_house_`, `_ending_`)

---

## Implementation patterns

Prefer the official SDK when available; otherwise use HTTP.

### Example: Python (simple TTS)

```python
import os
import requests

api_key = os.environ.get("ELEVENLABS_API_KEY")
assert api_key, "ELEVENLABS_API_KEY not set"

voice_id = os.environ.get("ELEVENLABS_MARCO_VOICE_ID")
assert voice_id, "ELEVENLABS_MARCO_VOICE_ID not set"

url = f"https://api.elevenlabs.io/v1/text-to-speech/{voice_id}"
headers = {
    "xi-api-key": api_key,
    "Content-Type": "application/json",
}

text = (
    "I have been doing these deliveries every night, "
    "but I can't remember a single one."
)

payload = {
    "text": text,
    "model_id": "eleven_multilingual_v2",
    "voice_settings": {
        "stability": 0.6,
        "similarity_boost": 0.8
    }
}

response = requests.post(url, headers=headers, json=payload)
response.raise_for_status()

output_path = "Assets/_Game/Audio/VO/Marco/marco_line_001_intro.wav"
os.makedirs(os.path.dirname(output_path), exist_ok=True)

with open(output_path, "wb") as f:
    f.write(response.content)

print(f"Saved VO line to {output_path}")
```

---

## Constraints

- Do not generate illegal or policy‑breaking content; follow ElevenLabs content guidelines.
- Do not invent unsupported parameters or voice settings.
- Keep per‑line length reasonable for in‑engine control; split monologues when useful.
- Use consistent voices per character (do not randomly switch voice IDs for the same persona).
