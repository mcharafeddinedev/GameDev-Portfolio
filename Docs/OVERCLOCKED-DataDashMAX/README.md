# OVERCLOCKED: Data Dash MAX

**Engine:** Unreal Engine 5.5  
**Language:** C++ (with Blueprint wrappers)  
**Platform:** Windows PC / Arcade Cabinet Hardware  
**Status:** Released (February 2026)

**Links:** [Itch.io](https://goldleafinteractive.itch.io/overclocked-ddm) | [YouTube Trailer](https://www.youtube.com/watch?v=dI9Ctq9LkLs)

---

## Overview

OVERCLOCKED: Data Dash MAX is a high-speed arcade endless runner where you play as an electric impulse racing through neon-lit circuitry. The game features responsive three-lane movement, dynamic obstacles, risk/reward speed boosting, and a full leaderboard system with arcade-style initials entry.

Built with a component-based C++ architecture and shipped to arcade cabinet hardware at stable 60fps.

---

## Documentation Index

- **[System Architecture](SystemArchitecture.md)** — Component structure, coordinate system, and data flow
- **[Code Samples](CodeSamples.md)** — Representative C++ implementations
- **[Systems Overview](SystemsOverview.md)** — Detailed breakdown of all core systems

---

## Key Features

### Technical Highlights
- **UE5 C++ with Blueprint wrappers** — Clean C++ core systems exposed to Blueprints for rapid iteration
- **Component-based architecture** — Modular systems as `UActorComponent` subclasses
- **Data-driven design** — Obstacle patterns, themes, and difficulty curves via Data Assets
- **Event-driven communication** — Delegates for decoupled UI updates
- **Performance-optimized** — Object pooling, conditional ticking, timer-based updates

### Gameplay Features
- **45+ hand-crafted obstacle patterns** with procedural generation and fairness validation
- **Risk/reward OVERCLOCK system** — Speed boost with score multipliers
- **6 data-driven color themes** — Full visual customization via ThemeSubsystem
- **Local leaderboard** with arcade-style 3-character initials entry
- **Full keyboard/gamepad navigation** — Zone-based menu system

---

## Core Systems

| System | Description |
|--------|-------------|
| `WorldScrollComponent` | Manages track scrolling, speed ramping, OVERCLOCK multiplier, damage slowdown |
| `ObstacleSpawnerComponent` | Spawns obstacles from patterns, handles pooling, difficulty scaling |
| `PickupSpawnerComponent` | Manages data packets, 1-UPs, EMPs, and magnet pickups |
| `ScoreSystemComponent` | Time-based scoring, combo tracking, leaderboard management |
| `OverclockSystemComponent` | Meter management, activation/deactivation, speed and score bonuses |
| `LivesSystemComponent` | Life tracking, invincibility frames, game over detection |
| `ThemeSubsystem` | Data-driven color theme management with persistence |

---

## Architecture Overview

```
AStateRunner_ArcadeGameMode (Orchestrator)
├── WorldScrollComponent
├── ObstacleSpawnerComponent
├── PickupSpawnerComponent
├── ScoreSystemComponent
├── OverclockSystemComponent
└── LivesSystemComponent

AStateRunner_ArcadeCharacter (Player)
├── Lane switching (Y-axis)
├── Jump/Slide (Z-axis)
├── Collision handling
└── Input processing

UThemeSubsystem (World Subsystem)
├── Theme asset loading
├── Dynamic material application
└── Preference persistence
```

---

## Coordinate System

**CRITICAL:** This project uses a non-standard coordinate system:

| Axis | Purpose | Notes |
|------|---------|-------|
| **X** | Track length (scroll direction) | World scrolls in **negative X** |
| **Y** | Track width (lanes) | Lane switching interpolates along Y |
| **Z** | Height | Jump/slide modifies Z position |

**Player Position (Locked):**
- X: -5000 (fixed)
- Y: 0 (center lane), ±LaneWidth for side lanes
- Z: 150 (base height)

---

## Developer

**Marwan Charafeddine**  
Solo Developer — Game Design, C++ Programming, Systems Architecture

- [Portfolio Website](https://mcharafeddinedev.github.io)
- [GitHub](https://github.com/mcharafeddinedev)
- [LinkedIn](https://www.linkedin.com/in/marwan-charafeddine-213065155)
