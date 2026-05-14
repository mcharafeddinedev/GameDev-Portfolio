# Dread & Breakfast

**Unity 6 · C# · 2D Orthographic · WebGL + Windows**

A cozy-spooky top-down haunting strategy game where the roles are reversed—you are the ghost, and the humans are the problem. Scare visitors until they flee before dawn.

**Links:** [Play in Browser](https://goldleafinteractive.itch.io/dread-and-breakfast) | [Itch.io Page](https://goldleafinteractive.itch.io/dread-and-breakfast)

**Jam:** Mini Jam 208: Inverted (72 hours) — Theme built into premise: you are the haunting, not the victim.

---

## Elevator Pitch

**Dread & Breakfast** is a roguelike haunted-house game where the roles are reversed — you are the ghost, and the humans are the problem.

From a top-down view of a procedurally assembled house, you haunt props, deploy ghostly abilities from an action bar, and read each visitor's fear profile to find what breaks them — all before the night ends.

Nights escalate. Between nights, you pick run-long advantages. Across runs, **Fright Points** and unlocks widen your toolkit.

---

## Core Loop

### Inner Loop — One Night
Each night runs on an accelerated clock from evening toward morning. Visitors spawn and roam. You haunt props, spend energy, and deploy abilities to empty the house.

- **Win:** All visitors have fled before the cutoff
- **Lose:** Time runs out with someone still inside

### Outer Loop — One Run
A run is a sequence of nights with rising pressure. Between nights, the player chooses from limited rewards that apply for the rest of that run (roguelike-style drafts).

### Meta Loop — Across Runs
**Fright Points** persist between runs and unlock new haunts, slots, or other upgrades in the **Box of Tricks** (ability codex / meta shop flow).

---

## Technical Highlights

### Data-Driven Content
Gameplay-facing definitions are authored as **ScriptableObjects**, **Resources** assets, and structured data:
- Room definitions and prop pools
- Visitor archetypes with fear profiles
- Ability definitions with scare categories
- Ghost presentation roster (portrait/cursor moods)
- SFX library keys

Tuning does not require rewriting core code for every balance pass.

### Five Scare Categories
Scares map to distinct categories for strategic matching:
- **Auditory** — Sound-based scares
- **Visual** — Sight-based scares  
- **Tactile** — Touch-based scares
- **Environmental** — World manipulation
- **Psychological** — Mind games

Visitors have different multipliers for each category based on their archetype.

### Ability System
- **17 haunts** with varying effects
- **3 ultimates** (e.g., Second Wind)
- **Six-slot action bar** with drag-from-library assignment
- Numeric hotkeys (1-6) and ultimate key (Q)
- Energy cost and cooldown per ability

### Procedural House Generation
Houses are built from:
- **Templates** — Grid-based footprints
- **Room assignment rules** — Cells categorized (corner, edge, interior)
- **Prop pools** — Room types draw from defined pools

Layouts feel different without hand-placing every object.

### Visitor AI
Multiple archetypes with shared behavior controller:
- **Patrol** — Wander between rooms
- **Investigate** — Check disturbances
- **Flee** — Run when fear overflows
- **Panic chains** — Scared guests scare others

Each archetype has unique fear multipliers, flavor text, and voice lines.

### Loose Coupling Architecture
`GameEvents` and similar hooks let UI, audio, and progression react to simulation changes without tight coupling across every subsystem.

---

## Content Roster

| Category | Count |
|----------|-------|
| Haunts (abilities) | 17 |
| Ultimates | 3 |
| Guest archetypes | 6 |
| Prop types | 13 |
| Room types | 6 |
| Scare categories | 5 |

---

## UI Architecture

**One screen, many panels.** Screen-space UI (uGUI) keeps text sharp; the playfield is framed under `SimulatorViewportShell` so HUD and map read clearly.

| Area | Components |
|------|------------|
| Action bar & portrait | `GameplayActionBar`, `GhostPresentationDriver`, `GhostPortraitClick` |
| Pause & codex | `PauseInputBinder`, `HardPauseOverlay`, `AbilityLibraryPanel` (Box of Tricks) |
| Hover & inspector | `HoverInfoManager`, `HoverInspectionFormatter`, `WorldInspectorHUD` |
| Audio | `GameAudioSettings`, `GameplayMusicDriver`, `GameplaySfxLibrary` |
| Meta | `FrightPointsBank`, `AbilityProgression`, run upgrade flow |

---

## Input Design

- **Pointer** for world and UI interaction
- **Unity Input System** for ability hotkeys, inspector lock, pause shortcuts
- **Pause input** centralized in `PauseInputBinder` (Tab/Space/Escape)

---

## Persistence

Lightweight client-side storage holds:
- Preferences (volume, settings)
- Meta unlock state (Fright Points, unlocked abilities)
- Appropriate for jam-scale WebGL build

---

## Documentation

- **[TechnicalOverview.md](TechnicalOverview.md)** — Architecture and implementation details
- **[GameDesign.md](GameDesign.md)** — Full game design document

---

*Dread & Breakfast — Gold Leaf Interactive — Mini Jam 208: Inverted*
