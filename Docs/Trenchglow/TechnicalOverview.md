# Technical Overview — Trenchglow

> Architecture and implementation details for the deep-sea sonar adventure.

**Engine:** Unity (2D URP) · **Input:** Input System package with legacy-axis fallbacks · **Targets:** WebGL + Windows

---

## High-Level Architecture

- **Engine:** Unity 2D, **Input System** package (with legacy-axis fallbacks in some player code), **URP** for rendering
- **Scenes (Build Settings):**
  - `_Persistent` — Long-lived singleton services (DontDestroyOnLoad)
  - `MainMenu` — Title/start flow
  - `Level_01` — Tutorial/vertical-slice gameplay level
- **Single entry for scene changes:** `GameFlow` is the only component that calls `SceneManager.LoadScene`; menus and `LevelExit` go through it with `ScreenFader` (fade out → load → fade in)
- **App state machine:** `GameSession` in `_Persistent` owns `GameState` (Boot, Menu, Loading, Playing, Paused, Dying, …), `Time.timeScale` rules (0 only for Paused/Dying), and input routing (`AcceptsGameplayInput` / `AcceptsUIInput`)

---

## Typical Flow

1. Boot loads `_Persistent` (and `GameFlow` may load `MainMenu` if nothing else is present)
2. Player starts game → `GameFlow` loads `Level_01` (or next level by name) with fader
3. `LevelBootstrap` on the level's gameplay root applies `LevelDefinition` (camera size, flow hooks), may load `_Persistent` additively if you pressed Play on the level only, and starts **Playing** in `GameSession`
4. `LevelExit` at the end calls into `LevelBootstrap` → `GameFlow` loads `LevelDefinition.NextSceneName` (or back to menu if empty)
5. On death, `ProgressionService` runs a death coroutine and `GameFlow` reloads the chapter's first scene (permadeath-style within the chapter)

---

## Flow & Level Data

| Component | Role |
|-----------|------|
| `FlowConfig` | First scene name, fade timings, persistent scene name |
| `GameFlow` | Singleton; `LoadScene`/`LoadLevel`; `DontDestroyOnLoad` on root; first-boot `MainMenu` load; boot fade to avoid white frame |
| `LevelBootstrap` | Per-level — assigns camera ortho from `LevelDefinition`, optional spawn warp, `GemScoreService.OnLevelStart`, ambience start, tilemap collider refresh for physics |
| `LevelDefinition` | ScriptableObject — `levelId`, `displayName`, `nextSceneName`, `cameraOrthoSize`, `ChapterDefinition` link |
| `ScreenFader` | UI Image alpha; used by `GameFlow` and `ProgressionService` |
| `SessionConfig` | Pause-related tuning on `GameSession` (e.g., post-process weight) |

**Designer workflow:** One `LevelDefinition` asset per level scene, referenced by that scene's `LevelBootstrap`.

---

## Main Menu & WebGL/Resolution

| Component | Role |
|-----------|------|
| `MainMenuController` | Wires Start/Settings/How to play/Credits/Quit, runtime modal panels (dimmer, scroll text, close), copy from Inspector; can load `_Persistent` if missing |
| `MainMenuResponsiveLayout` | On `MainMenuCanvas` — re-anchors TitlePlate, TitleSubtitle, ButtonStack, and FocusScrim images from fixed 1920×1080-style coordinates to top-centered title and right-pinned button column for browser iframes, ultrawide, and narrow widths |
| `CanvasScaler` | `Scale With Screen Size`, reference 1920×1080, match 0.5 (width/height blend) |
| Modal font scaling | Clamps panel size to root canvas, scales down body/title font when root canvas is short (e.g., short WebGL windows) |

---

## Player & Input

| Component | Role |
|-----------|------|
| `PlayerMotor` | Rigidbody2D swim: horizontal move, stamina-gated upward swim, boost (one-shot kick), lateral swim buoyancy when only moving sideways in air, no dedicated jump |
| `PlayerMotorProfile` | All tuning (accel, drag, gravity scales, stamina, boost) |
| `PlayerDeathSpriteFade` | Death presentation — fades sprite; used with `ProgressionService` |
| `InputRouter` | Exposes `Gameplay` action map; `SonarPulseController` and `PlayerMotor` read `Pulse`/`Move`/`Boost` |

`GameSession.AcceptsGameplayInput` gates whether pulse and move apply.

---

## Sonar (Pulse) System

| Component | Role |
|-----------|------|
| `SonarPulseController` | Player-side — owns a runtime clone of `PulsePolicy` (so the asset on disk is not mutated), reads cooldown/deny, spawns pulse VFX, notifies visibility, optional camera shake on fire |
| `PulseProfile` | References `PulsePolicy` + timing/visual radii (used by VFX, visibility, darkness) |
| `PulsePolicy` | Cooldown, deny rules; `CreateRuntimeInstance` for per-scene state |
| `VisibilityController` | Dual tilemap reveal: `Terrain_Resolved` vs `Terrain_Scan` (silhouette). Modes: shader globals or fallback (full resolved on; darkness handled separately) |
| `DarknessRadialOverlay` | Screen-space darkening: lit radius tracks pulse timing (expand/hold/contract) |
| `PulseRevealTiming` | Shared expand/hold/contract clock for lit radius |
| `SonarPulsedHighlight` | Optional pulsed highlight on world props when sonar hits |

---

## World Interaction & Puzzles

| Component | Role |
|-----------|------|
| `PressurePoint` | Floor/ceiling plates — `StandOn` mode, `UnityEvent` on activate, audio, `IResettable` |
| `TrenchChunk` | Kinematic moving geometry — slide/shift/rotate, easing, optional SFX-length duration, impact shake, `IResettable` |
| `Door` | Open/close collider + sprite swap, audio, `IResettable` |
| `StateChannel` | Boolean bus for combinational logic |
| `StateWriter` | `Apply()` from UnityEvents; `IResettable` |
| `StateListener` | Subscribes to `StateChannel` and drives UnityEvents |
| `SegmentResetRegistry` | Batches `IResettable` for segment reset |
| `PulseResonator` / `IResonanceActor` | Optional sonar-keyed props |

---

## Hazards & Death

| Component | Role |
|-----------|------|
| `KillVolume` | Trigger death via `ProgressionService` |
| `KillVolumePitVisuals` | Optional pit visuals, mine cluster, net |
| `Mine` | Detonation + `AudioId.MineDetonation` |
| `ProgressionService` | Death path — `PlayerDeathSpriteFade`, `ScreenFader`, then `GameFlow.LoadScene(chapter first scene)`. Checkpoints register but are no-op for latch in the permadeath model |
| `ProgressionConfig` | Death timing and audio |
| `CheckpointTrigger` | Legacy for scene wiring |

---

## Progression, Scoring, Level End

| Component | Role |
|-----------|------|
| `LevelExit` | Trigger2D — `LevelBootstrap.OnLevelComplete` → `GameFlow.LoadLevel` |
| `GemScoreService` | Run gem total; `OnGemsChanged` for HUD |
| `TreasureGemPickup` + `TreasureGemProfile` | Collectible gems |
| `GemsHud` | Displays total from `GemScoreService` |

**Chapter/level metadata:** `ChapterDefinition` (referenced by `LevelDefinition`) names the first scene of chapter for death restart.

---

## Audio

| Component | Role |
|-----------|------|
| `AudioManager` | Singleton — plays by `AudioId`, loops, 2D SFX |
| `AudioId` | Enum of cues |
| `AudioLibrary` | ScriptableObject — maps `AudioId` → clip + volume |

---

## Camera & Juice

| Component | Role |
|-----------|------|
| `LevelCameraFollow` | Follows player |
| `CameraShaker` | Shake for pressure, chunks, sonar, doors |
| `TutorialPopup` | In-level tips, layout |

---

## Visual Helpers (Non-Gameplay)

Examples: `SpriteTintPulse`, `UnderwaterBob`, `MineClusterGroupBob`, shaders under `_Project/Shaders/`.

---

## Feature Request Mapping

| Request | Where to Look |
|---------|---------------|
| "Menu clips in the browser" | `MainMenuResponsiveLayout` + `CanvasScaler` on `MainMenuCanvas`; modal font logic in `MainMenuController.CreateMenuModal` |
| "Pulse feels wrong" | `PulseProfile`/`PulsePolicy`, `DarknessRadialOverlay` + `VisibilityController` |
| "Plate doesn't open door" | Scene wiring: `PressurePoint` → `Door`/`StateWriter`/`TrenchChunk` |
| "Death is broken" | `KillVolume`/`Mine` → `ProgressionService.Die` → `GameFlow` + `ChapterDefinition` |
| "After level, wrong scene" | `LevelDefinition.NextSceneName` |

---

*Trenchglow — Gold Leaf Interactive — Mini Jam 209: Deep*
