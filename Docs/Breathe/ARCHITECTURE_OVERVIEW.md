# Breathe — Architecture Overview

A one-page summary of how the breath-to-gameplay pipeline is structured. For full project details and source, see **[Breathe-Game](https://github.com/mcharafeddinedev/Breathe-Game)**.

---

## Breath Pipeline Layers

The system turns raw breath input into a stable signal the game can use. It’s split into clear layers so each part can be tuned or replaced without breaking the rest:

1. **Signal capture** — Reads the active input source (custom hardware, microphone, or simulated).
2. **Normalization** — Converts the raw signal into a bounded value (e.g. 0–1 intensity).
3. **Stability** — Smoothing and filtering to cut noise, jitter, and spikes.
4. **Gameplay integration** — Feeds the normalized intensity into wind/propulsion (and any other systems that need it).

Game logic only sees the final, stable value. It doesn’t care where the signal came from.

---

## Input Abstraction

All input paths use the **same interface**. Whether the signal comes from the custom fan device, the microphone fallback, or a simulated curve in the editor, gameplay code talks to one abstraction. The actual source is chosen at configuration time. That keeps the codebase stable, makes testing and demos easier (e.g. no hardware required), and lets you add or swap input methods without touching gameplay.

---

## No-Fail Design Rationale

The game is built so that no player ever “loses.” There are no failure states or Game Over screens. Races adapt to the player’s breathing ability; the finish screen highlights personal stats and progress instead of win/loss. That choice supports accessibility (especially for younger or respiratory-rehabilitation players), reduces pressure, and encourages repeat play—which also makes the system suitable for capturing consistent breath-effort data over time if used in healthcare-adjacent contexts.

---

*This document is a high-level summary for portfolio readers. Implementation details and full design docs live in the [Breathe-Game](https://github.com/mcharafeddinedev/Breathe-Game) repository.*
