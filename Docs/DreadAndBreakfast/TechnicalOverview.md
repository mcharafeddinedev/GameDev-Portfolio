# Technical Overview — Dread & Breakfast

> High-level architecture notes for my browser-friendly haunted-house game.

**Engine:** Unity 6 (C#) · **Targets:** WebGL (browser) + desktop builds · **Status:** Playable prototype with some iterative UI polish/patching

---

## What the Player Runs

Dread & Breakfast is a 2D, top-down haunted-house game: you play as the ghost, scare visitors, manage energy and abilities, and try to clear the house before the night ends. Progression spans individual nights and longer runs, with a persistent **Fright Points** currency for meta unlocks.

The **Inverted** jam theme is reflected in the premise: you are the ghost, haunting the visitors/investigator, instead of the other way around which is more common. (Phasmophobia etc.)

---

## Architecture Philosophy

### Configurable Content
Gameplay-facing definitions are authored as **ScriptableObjects**, **Resources** assets, and structured data:
- Room definitions
- Prop configurations
- Visitor archetypes (`HumanArchetypeSO`)
- Ability definitions (`AbilitySO`)
- SFX library keys

Tuning does not require rewriting core code for every balance pass.

### Loose Coupling
`GameEvents` and similar hooks let UI, audio, and progression react to simulation changes without tight coupling across every subsystem.

### One Screen, Many Panels
Screen-space UI (uGUI) keeps text sharp; the playfield is framed under `SimulatorViewportShell` so HUD and map read clearly. Some HUD layers use nested canvases for sort order; **GraphicRaycasters** on nested canvases are required where pointer hits must reach controls (e.g., ghost portrait).

### Saving Progress
Lightweight client-side storage holds preferences and meta unlock state appropriate for a jam-scale WebGL build.

---

## Stack

| Area | Implementation |
|------|----------------|
| Engine | Unity 6 — 2D gameplay, uGUI, WebGL export |
| Input | Pointer + `InputSystem` (`InputManager` for hotkeys; `PauseInputBinder` for Tab/Space/Escape) |
| Text | TextMeshPro for HUD and modals |
| Data | ScriptableObjects under `Assets/ScriptableObjects`, `Resources` for runtime loads |

---

## Major Systems

### Gameplay Simulation

| System | Responsibility |
|--------|----------------|
| House Generator | Grid-based templates with room assignment rules |
| Prop System | Interaction types (click, hold, drag, timing), energy cost, cooldowns |
| Visitor Controller | Patrol, investigate, flee behaviors; fear accumulation |
| Ability System | Six-slot loadout, energy management, cooldowns |
| Scare Calculator | Category matching with archetype multipliers |

### UI Entry Points

| Area | Components |
|------|------------|
| Action bar & portrait | `GameplayActionBar`, `GhostPresentationDriver`, `GhostPortraitClick` |
| Pause & codex | `PauseInputBinder`, `HardPauseOverlay`, `AbilityLibraryPanel` (Box of Tricks) |
| Hover & inspector | `HoverInfoManager`, `HoverInspectionFormatter`, `WorldInspectorHUD` |
| Audio | `GameAudioSettings`, `GameplayMusicDriver`, `GameplaySfxLibrary` / `SfxPlaybackSubscriber` pattern |
| Meta | `FrightPointsBank`, `AbilityProgression`, run upgrade / intermission flow |

---

## Content Authoring

Parallel authorable lists define:
- Props and room rules
- `HumanArchetypeSO` (names, flavor text, fear multipliers, voice lines)
- `AbilitySO` loadouts
- `GhostPresentationRosterSO` for portrait/cursor moods
- Catalog entries for Guest Archetypes / Props modals

Counts change with content drops; architecture stays data-driven.

---

## Prop Interaction Types

| Input | Feel | Examples |
|-------|------|----------|
| **Click** | Quick | Switches, cabinets, single-hit spooks |
| **Hold** | Building | Slow burns, drips, rising tension |
| **Drag** | Physical | Sliding furniture, dramatic moves |
| **Timing** | Skill expression | Bonus fear when executed in a window |

Each prop respects energy, cooldown, and category rules.

---

## Scare Category System

Five categories for strategic matching:

| Category | Description |
|----------|-------------|
| Auditory | Sound-based scares |
| Visual | Sight-based scares |
| Tactile | Touch-based scares |
| Environmental | World manipulation |
| Psychological | Mind games |

Visitors have multipliers per category based on archetype. Matching scare types to visitor weaknesses maximizes fear gain.

---

## Input Architecture

### Gameplay
- **Pointer** for world interaction and UI
- **Hotkeys** (1-6) for ability slots
- **Q** for ultimate when available
- **Tab** for Box of Tricks
- **Space/Esc** for pause

### Pause Input
Centralized in `PauseInputBinder`:
- Runs after scene loads so Main Menu → Game loads still get keyboard shortcuts
- Handles Tab/Space/Escape routing

---

## Accessibility Goals

- **Readability:** Scare types distinguishable by more than color where possible
- **Input:** Core loop is pointer-driven; keyboard augments HUD and loadout
- **Web:** Load size and frame cost matter for typical embed resolutions

---

## Performance Considerations

- WebGL Player Settings tuned per build
- Compression optimized for browser delivery
- Frame cost monitored for typical embed resolutions

---

*Dread & Breakfast — Gold Leaf Interactive — Mini Jam 208: Inverted*
