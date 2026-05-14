# Documentation

This folder contains documentation and code samples from my game development projects. It includes system architecture breakdowns, technical overviews, and code samples showcasing my approach to gameplay programming and systems design.

**Featured Projects:** OVERCLOCKED: Data Dash MAX, ACCESS GRANTED, Dread & Breakfast, Trenchglow, BREATHE Arcade, Quantum Tether, Ginger Shroom Journey

---

## What's Here

### OVERCLOCKED: Data Dash MAX (`OVERCLOCKED-DataDashMAX/`)
My most technically polished project — a complete arcade endless runner built in **Unreal Engine 5 (C++)** and shipped to arcade cabinet hardware.

**Links:** [Itch.io](https://goldleafinteractive.itch.io/overclocked-ddm) | [YouTube Trailer](https://www.youtube.com/watch?v=dI9Ctq9LkLs)

- **`README.md`** - Project overview, features, and documentation index
- **`SystemArchitecture.md`** - Component architecture, coordinate system, event-driven patterns
- **`CodeSamples.md`** - Representative C++ implementations (lane switching, OVERCLOCK, themes, combos)
- **`SystemsOverview.md`** - Detailed breakdown of all core gameplay systems

**Technical Highlights:**
- Component-based architecture with 7 core systems
- 45+ hand-crafted obstacle patterns with procedural generation
- Data-driven theme system (6 color schemes)
- Local leaderboard with arcade-style initials entry
- Stable 60fps on arcade cabinet hardware

---

### ACCESS GRANTED (`AccessGranted/`)
*Hackers* (1995)-inspired challenging breakout-like with a typing twist — Unity 6 (C#), 2D physics. Dual-mode gameplay switches between paddle reflexes and typing precision.

**Links:** [Play in Browser](https://goldleafinteractive.itch.io/access-granted)

- **`README.md`** — Project overview, concept, and architecture diagram
- **`TechnicalOverview.md`** — Detailed component breakdown, block types, typing subsystem, WebGL considerations

**Technical Highlights:**
- Dual-mode gameplay (paddle ↔ typing)
- Modular block types with HP tiers (Standard, Heavy, Command, Explosive, Indestructible)
- World-space typing prompts with slow-mo and overlap prevention
- Rule-of-three failure system (separate ball-miss and typing-fail counters)
- WebGL-first with auto-pause on tab blur

---

### Dread & Breakfast (`DreadAndBreakfast/`)
Top-down haunting strategy roguelike — Unity 6 (C#), 2D orthographic. You are the ghost; scare house guests, make them flee before dawn. Built for Mini Jam 208: Inverted.

**Links:** [Play in Browser](https://goldleafinteractive.itch.io/dread-and-breakfast)

- **`README.md`** — Project overview, game loops, content roster
- **`TechnicalOverview.md`** — Data-driven architecture, scare categories, ability system, visitor AI

**Technical Highlights:**
- Data-driven content (ScriptableObjects for props, visitors, abilities, archetypes)
- Five scare categories with visitor-specific fear profiles
- 17 haunts + 3 ultimates with eight-slot ability action bar
- Procedural house generation (grid templates + room assignment rules)
- Roguelike meta-progression with Fright Points

---

### Trenchglow (`Trenchglow/`)
Deep-sea sonar adventure — Unity (C#), 2D URP. Navigate dark abyssal trenches; sonar pulses reveal the world momentarily. Built for Mini Jam 209: Deep.

**Links:** [Play in Browser](https://goldleafinteractive.itch.io/trenchglow)

- **`README.md`** — Project overview, controls, puzzle systems, level architecture
- **`TechnicalOverview.md`** — Sonar reveal system, player controller, state machine, scene architecture

**Technical Highlights:**
- Dual tilemap visibility system (resolved vs scan silhouette)
- Shader-driven sonar with timed expand/hold/contract
- Stamina-gated swimming with boost mechanics
- Puzzle infrastructure (PressurePoints, TrenchChunks, Doors, StateChannels)
- App state machine with clean scene flow

---

### Quantum Tether (`QuantumTether/`)
Endless roguelike grappling sidescroller — Unity (C#). Swing on stars and asteroids with physics-driven movement and procedural generation. Built for Texas Game Jam 2025.

**Links:** [Itch.io](https://goldleafinteractive.itch.io/quantum-tether)

- **`README.md`** — Project overview and documentation guide
- **`Project_Analysis_and_Architecture.md`** — System architecture analysis
- **`Quantum_Thread_Post_Mortem.md`** — Development insights and what I learned
- **`README_System_Architecture.md`** — How all systems work together
- **`Scripts/`** — Source code organized by system (7,600+ lines)

**Technical Highlights:**
- Grapple feel tuned for arcade swing (damping, rope length, dash assists)
- Procedural level generation with hand-authored pattern pieces
- Difficulty tiers with weighted selection as run speeds up
- 10+ upgrades affecting movement and interaction
- Event-based architecture for decoupled systems

---

### BREATHE Arcade (`Breathe/`)
Breath-controlled minigame collection — Unity 6 (C#), 2D URP. Five minigames where breath is the only input; custom hardware (fan sensor with Arduino), microphone fallback, and keyboard simulation. No-fail design ensures every session ends positively.

**Links:** [Play in Browser](https://goldleafinteractive.itch.io/breathe-arcade) | **Project repo:** [Breathe-Game](https://github.com/mcharafeddinedev/Breathe-Game)

- **`README.md`** — Portfolio summary: pitch, minigame list, tech stack, hardware overview
- **`ARCHITECTURE_OVERVIEW.md`** — Breath pipeline layers, input abstraction, no-fail rationale

**Minigames:**
- **Sailboat** — Fill sails with steady breath to race AI companions
- **Balloon** — Controlled exhales inflate balloons faster
- **Bubbles** — Chain bubbles with consistent breath stream
- **Stargaze** — Push clouds to reveal constellations
- **Skydive** — Guide parachuting skydiver with jetpack thrusters

**Technical Highlights:**
- IMinigame interface for shared lifecycle, analytics, and result display
- Source-agnostic breath pipeline (hardware / mic / simulated)
- Custom hardware integration (DC motor voltage → Arduino → USB serial → Unity)
- Spin-down compensation for fan coast detection
- Procedural generation (scenes are minimal; everything generated at runtime)
- Breath analytics with personal bests per minigame

---

### Project Documentation
- **`MC_ProjectExperiences.pdf`** - Notes on the tech stack and tools I've used across different projects
- **`GSJ_CSharp_Analysis.pdf`** - Deep dive into the code architecture and systems I built for Ginger Shroom Journey
- **`VideoLinks.pdf`** - Some development process recordings and demos

### Game Scripts (`GSJ_Scripts/`)
The actual C# source code from Ginger Shroom Journey, organized by what each part does:

#### Core Systems
- **`GameManager.cs`** - Handles the main game state and scene transitions
- **`ScoreManager.cs`** - Keeps track of the player's score and updates the UI
- **`PauseManager.cs`** - Manages the pause menu using Unity's new input system
- **`CursorManager.cs`** - Controls when the cursor is visible or locked

#### Player Systems
- **`PlayerController.cs`** - The main player movement script with jumping and shooting
- **`PlayerClimb.cs`** - Handles ladder climbing with sound effects
- **`Arrow.cs`** - Manages the arrows the player can shoot

#### Enemy AI
- **`SlimeController.cs`** - Basic enemy AI that patrols back and forth
- **`FireflyController.cs`** - Particle effects for environmental ambiance

#### Level Systems
- **`CoinScript.cs`** - Collectible coins that add to the score
- **`TrapScript.cs`** - Spikes and other hazards that kill the player
- **`Warp.cs`** - Teleports the player between levels
- **`SideToSideMovement.cs`** - Moving platforms

#### UI Systems (`Buttons/`)
- **`PlayButton.cs`** - Starts the game
- **`ExitGameButton.cs`** - Quits the application
- **`IronManModeButton.cs`** - Toggles a harder game mode
- **`MainMenuButton.cs`** - Returns to the main menu
- **`SettingsButton.cs`** - Opens the settings menu

#### Platform Integration
- **`SteamManager.cs`** - Integrates with Steam for achievements and cloud saves

### Visual Assets (`assets/`)
- **`logo/`** - Game logo and branding images
- **`MoT Blueprints/`** - Screenshots of Unreal Engine Blueprint work

### Academic Work (`LogbookExamples/`, `PPTs/`)
- **Logbook Examples** - Development logs and process documentation
- **Presentation Materials** - Analysis of 3D level design from other games

## About the Code

The scripts here are from the actual game I published on Steam. I tried to keep things organized and documented, so each script has a clear purpose. The code includes:

- Error handling and null checks where needed
- Comments explaining the trickier parts
- Modern Unity practices like the new Input System
- Integration with Steam for achievements and saves

Most of the scripts are pretty straightforward - they handle one specific thing like player movement or enemy behavior. I used singleton patterns for managers that need to persist between scenes.

The code samples are here for portfolio purposes. If you want to use any of it, check the main LICENSE.md file for details.
