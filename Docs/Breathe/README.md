# Breathe — Breath-Controlled Sailboat Race

A short portfolio summary. For full project docs and source, see the main repository: **[Breathe-Game](https://github.com/mcharafeddinedev/Breathe-Game)**.

---

## One-Line Pitch

Breath is the only input: blow toward a custom device to power the wind that drives a sailboat in a race alongside AI companions. No one ever loses.

**Core metaphor:** breath → wind → sail power. The harder you blow, the faster the boat goes.

---

## Why Breath-Only

- **Focus** — No secondary inputs; breathing *is* the game.
- **Accessibility** — Playable by anyone who can blow; no controller skill required.
- **Physical metaphor** — Blow harder, sail faster; the link is immediate.
- **Spectator clarity** — Onlookers see the fan spin and the boat respond; the interaction reads from across the room.

---

## Tech Stack

- **Engine:** Unity (C#), 2D URP
- **Platform:** Windows PC
- **Input:** Custom breath-sensing hardware (primary), microphone fallback, simulated input for development. All feed through a **single abstraction layer** so gameplay code is unchanged when the source is swapped.

---

## No-Fail Design

Every session ends positively: no "Game Over," no loss states. Races adapt to the player’s breathing ability, and the finish celebrates personal stats and progress so players leave feeling good and want to play again.

---

## In This Folder

- **[ARCHITECTURE_OVERVIEW.md](ARCHITECTURE_OVERVIEW.md)** — Breath pipeline layers, input abstraction, and no-fail design rationale (one-page summary).

---

## Current Status

**Prototype / vertical slice in progress.** A playable loop is being built: breath input → sailboat race → AI companions → positive finish. Design and architecture are set; implementation is ongoing.

---

## Project Repository

**[github.com/mcharafeddinedev/Breathe-Game](https://github.com/mcharafeddinedev/Breathe-Game)** — Full documentation, design docs, and source code.

---

*This portfolio page is a summary only. The Breathe project and its repository are proprietary (All Rights Reserved).*
