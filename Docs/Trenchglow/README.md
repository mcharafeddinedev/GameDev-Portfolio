# Trenchglow

**Unity · C# · 2D URP · WebGL + Windows**

A 2D deep-sea adventure where you play as a treasure-hunter trapped in abyssal trenches. Most of the world is hidden in darkness—sonar pulses reveal the environment momentarily.

**Links:** [Play in Browser](https://goldleafinteractive.itch.io/trenchglow) | [Itch.io Page](https://goldleafinteractive.itch.io/trenchglow)

**Jam:** Mini Jam 209: Deep (April 23–26, 2026)
- **Theme:** Deep
- **Limitation:** Only one life

---

## Concept

Trenchglow is a 2D side-scrolling adventure game where you play as a treasure-hunter trapped in an abyssal maze of trenches at the ocean floor. Most of the world is hidden in the dark: sonar pulses reveal most of the environment around you, but only momentarily.

This isn't supposed to be a platformer, but rather a deep sea escape adventure with puzzles and mild platforming.

---

## Gameplay

- **Navigate** handcrafted levels (swim & scan, avoid hazards, solve puzzles)
- **Use sonar** to reveal the world momentarily
- **Manipulate pressure points** to open paths and escape
- **Collect gems** as optional treasure
- **Escape each room** by solving puzzles and progressing through the abyss

---

## Technical Highlights

### Sonar Reveal System

**Dual Tilemap Visibility:**
- `Terrain_Resolved` — Full detail terrain
- `Terrain_Scan` — Silhouette/scan version

**Visibility Modes:**
- **Shader globals** — GPU-driven reveal radius
- **Fallback mode** — Full resolved terrain on; darkness handled separately (for WebGL/budget GPUs)

**Darkness Overlay:**
- `DarknessRadialOverlay` — Screen-space darkening
- Lit radius tracks pulse timing (expand → hold → contract)
- `PulseRevealTiming` — Shared clock for lit radius animation

### Player Controller

**Rigidbody2D Swimming:**
- Horizontal movement with acceleration curves
- **Stamina-gated upward swim** — Going up costs stamina
- **Boost/Lunge** — One-shot kick with Shift
- **Lateral swim buoyancy** — When only moving sideways in air
- No dedicated jump — `Jump` input not consumed by design

**Tuning via ScriptableObject:**
- `PlayerMotorProfile` — Acceleration, drag, gravity scales, stamina, boost parameters

### Sonar (Pulse) System

| Component | Role |
|-----------|------|
| `SonarPulseController` | Player-side pulse logic; spawns VFX, notifies visibility system |
| `PulseProfile` | References `PulsePolicy` + timing/visual radii |
| `PulsePolicy` | Cooldown, deny rules; `CreateRuntimeInstance` for per-scene state |
| `VisibilityController` | Manages dual tilemap reveal |
| `DarknessRadialOverlay` | Screen-space lit radius |
| `PulseRevealTiming` | Expand/hold/contract timing clock |

### Puzzle Infrastructure

| Component | Role |
|-----------|------|
| `PressurePoint` | Floor/ceiling plates — `StandOn` mode, UnityEvent on activate, audio, `IResettable` |
| `TrenchChunk` | Kinematic moving geometry — slide/shift/rotate, easing, impact shake, `IResettable` |
| `Door` | Open/close collider + sprite swap, audio, `IResettable` |
| `StateChannel` | Boolean bus for combinational logic |
| `StateWriter` | `Apply()` from UnityEvents; `IResettable` |
| `StateListener` | Subscribes to `StateChannel` and drives UnityEvents |

### Hazards & Death

| Component | Role |
|-----------|------|
| `KillVolume` | Trigger death via `ProgressionService` |
| `Mine` | Detonation + audio |
| `ProgressionService` | Death path — fade, reload chapter first scene |
| `CheckpointTrigger` | Legacy for scene wiring (no-op in permadeath model) |

### App State Machine

`GameSession` in `_Persistent` scene owns:
- `GameState` (Boot, Menu, Loading, Playing, Paused, Dying, …)
- `Time.timeScale` rules (0 only for Paused/Dying)
- Input routing (`AcceptsGameplayInput` / `AcceptsUIInput`)

### Scene Architecture

**Build Settings Scenes:**
- `_Persistent` — Long-lived singleton services (DontDestroyOnLoad)
- `MainMenu` — Title/start flow
- `Level_01` — Tutorial/vertical-slice gameplay level

**Single Entry for Scene Changes:**
`GameFlow` is the only component that calls `SceneManager.LoadScene`; menus and `LevelExit` go through it with `ScreenFader` (fade out → load → fade in).

### Responsive UI

`MainMenuResponsiveLayout` on `MainMenuCanvas`:
- Re-anchors TitlePlate, TitleSubtitle, ButtonStack, FocusScrim
- Handles WebGL iframes, ultrawide, and narrow widths
- Modal font scaling for short windows

---

## Controls

| Input | Action |
|-------|--------|
| WASD / Arrows | Swim (up uses stamina) |
| E / LMB | Sonar Pulse |
| Shift | Boost/Lunge |
| Esc / Tab | Pause |
| Enter | Confirm (menus) |

---

## Level Data

| Component | Role |
|-----------|------|
| `FlowConfig` | First scene name, fade timings, persistent scene name |
| `GameFlow` | Singleton; scene loading; boot fade |
| `LevelBootstrap` | Per-level setup — camera ortho, spawn warp, ambience start |
| `LevelDefinition` | ScriptableObject — levelId, displayName, nextSceneName, cameraOrthoSize, ChapterDefinition link |
| `ChapterDefinition` | Names first scene of chapter for death restart |

---

## Progression

- **Permadeath-style** within chapters — death reloads chapter's first scene
- **Checkpoints** exist in code but are no-op for the jam permadeath model
- **Gem scoring** via `GemScoreService` with `OnGemsChanged` events for HUD

---

## Documentation

- **[TechnicalOverview.md](TechnicalOverview.md)** — Detailed architecture breakdown

---

## Attribution

- **Art Pack:** Underwater Diving — Luis Zuno (ansimuz)
- **Art Pack:** Gems spritesheets — Gerson Marcelo
- **Art Pack:** UI Panels — styloo
- **Art Pack:** Pressure Point Buttons — c0ffeenn_

---

*Trenchglow — Gold Leaf Interactive — Mini Jam 209: Deep*
