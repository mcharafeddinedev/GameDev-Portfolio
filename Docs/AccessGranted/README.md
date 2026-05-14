# ACCESS GRANTED

**Unity 6 · C# · 2D Physics · WebGL + Windows**

A *Hackers* (1995)-inspired arcade game: classic brick breaker action plus typing challenges when you hit special command blocks. Neon terminal aesthetics, CRT-style presentation, short score-chasing sessions.

**Links:** [Play in Browser](https://goldleafinteractive.itch.io/access-granted) | [Itch.io Page](https://goldleafinteractive.itch.io/access-granted)

**Jam:** The Movie The Game The Jam 2026

---

## Concept

**Fantasy:** You are punching through a firewall of blocks; the keyboard is part of the tension, not a simulation of real hacking.

**Core Loop:**
- Paddle and ball — easy to understand in seconds
- Typing under pressure — strict word challenges that slow the action and force a mode switch from reflexes to precision
- Fair stakes — separate "ball miss" and "typing fail" pressure tracks; clear game over when either hits its limit
- Handcrafted levels — designed layouts, not procedural generation

---

## Technical Highlights

### Dual-Mode Gameplay
The game switches between two distinct input modes:
- **Paddle Mode:** Arrow keys control horizontal paddle movement for ball deflection
- **Typing Mode:** When command blocks are hit or balls are missed, gameplay slows and a typing prompt appears

### Block System
Modular block types with HP tiers:
- **Standard/Heavy blocks** — Different HP values
- **Command blocks** — Require ball hit + typed command to destroy
- **Indestructible blocks** — Level layout obstacles
- **Explosive blocks** — Chain reaction damage
- **Pickup carriers** — Drop power-ups on destruction

### Typing Subsystem
- **World-space falling prompts** — Commands fall from block position or random X on miss
- **Slow-motion** — `timeScale` drops to 0.5 during typing with wall-clock motion preserved
- **Overlap prevention** — Minimum ΔX/ΔY between prompts; max 2 active prompts
- **Paddle freeze** — Input locked to typing during challenge
- **Ball reattach** — After miss recovery, ball returns to paddle with cooldown launch

### Power-Up System
Three pickups keep scope tight:
- **Wide Paddle** — Increased catch area
- **Slow Ball** — Reduced ball velocity
- **Score Burst** — Bonus points

### Audio Design
- **Duck + low-pass** filter on music during typing mode for focus
- **Click-to-start** WebGL splash for audio context initialization

### WebGL Considerations
- **Auto-pause on tab blur** — Prevents unfair deaths when focus is lost
- **Keyboard-only input** — A/D and arrow keys for paddle; typing for challenges
- **Resolution handling** — 1280×720 minimum recommended; HUD scales with screen

---

## Architecture

```
┌─────────────────────────────────────────────────────────┐
│                      GameFlow                            │
│  (States: Playing, Typing, GameOver, LevelComplete)     │
└─────────────────────────────────────────────────────────┘
           │                    │                    │
           ▼                    ▼                    ▼
┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐
│  PaddleController│  │  BallController │  │  Block Prefabs  │
│  (Movement,      │  │  (Physics,      │  │  (HP, Type,     │
│   Bounds)        │  │   Anti-stuck)   │  │   Points)       │
└─────────────────┘  └─────────────────┘  └─────────────────┘
                              │
                              ▼
                    ┌─────────────────┐
                    │ CommandTyping   │
                    │ Controller      │
                    │ (Prompts, Input)│
                    └─────────────────┘
```

---

## Scoring

**Formula (v1):**
```
accuracyMult = (1 - ballMisses/3) * (1 - typingFails/3)
finalScore = (baseScore + timeBonus) * accuracyMult
```

---

## Documentation

- **[TechnicalOverview.md](TechnicalOverview.md)** — Detailed architecture and implementation notes

---

*ACCESS GRANTED — Gold Leaf Interactive — The Movie The Game The Jam 2026*
