# Quantum Tether Documentation

This folder contains documentation and source code from **Quantum Tether**, my game jam project from TX Game Jam 2025 (EGaDS at UT Austin)

Check out the game's public repository! --> [https://github.com/mcharafeddinedev/QuantumTether]

## Project Overview

**Quantum Tether** is an infinite 2D side-scrolling grappling hook game I built in Unity 6 and scripted with C#. Players swing between anchor points using rope physics while the camera scrolling speed increases over time to introduce more difficulty. The game has procedural generation for the infinite scrolling environment and game objects, a rogue-like upgrade system, and audio integration.

**Game Jam**: TX Game Jam 2025 (EGaDS)
**Theme**: "Out of Time"  
**Development Time**: 52 hours + post-jam refinement for several days following the jam
**Platform**: Unity 6 (Windows)  

## What's Here

### Documentation Files
- **`Project_Analysis_and_Architecture.md`** - System overview and architecture analysis
- **`Quantum_Thread_Post_Mortem.md`** - Development analysis, what I learned, and technical insights
- **`README_System_Architecture.md`** - How all systems work together

### Source Code (`Scripts/`)
The C# source code organized by system:

#### Core Systems (`Core/`)
- **`EnhancedGameManager.cs`** - Central game state management with event system
- **`EnhancedBootstrap.cs`** - Game initialization and setup

#### Player Systems (`Player/`)
- **`EnhancedPlayerSwing.cs`** - Advanced grappling hook mechanics with rope physics
- **`EnhancedPlayerDash.cs`** - Dash movement with cooldown system

#### Camera System (`Camera/`)
- **`EnhancedCamera.cs`** - Auto-scrolling camera with speed ramping and death detection

#### UI Systems (`UI/`)
- **`EnhancedMainMenu.cs`** - Main menu controller
- **`EnhancedOptionsMenu.cs`** - Settings menu with volume controls
- **`EnhancedCreditsMenu.cs`** - Credits screen
- **`DeathQuoteManager.cs`** - Death screen quotes and feedback
- **`ResolutionManager.cs`** - Resolution handling

#### Scoring System (`Scoring/`)
- **`EnhancedScore.cs`** - Score management, death panel, and pause functionality

#### Spawning Systems (`Spawning/`)
- **`EnhancedSpawner.cs`** - Procedural generation of anchors and collectibles
- **`EnhancedCollectible.cs`** - Time crystal collection mechanics

#### Upgrade System (`Upgrades/`)
- **`UpgradeManager.cs`** - Central upgrade system manager
- **`UpgradeLibrary.cs`** - Available upgrades and pools
- **`RunUpgrade.cs`** - Upgrade data structure
- **`RunUpgradeState.cs`** - Upgrade state tracking
- **`UpgradeApplier.cs`** - Applies upgrade effects
- **`UpgradePanelUI.cs`** - Upgrade selection UI
- **`UpgradeCardUI.cs`** - Individual upgrade cards
- **`UpgradeFeedbackUI.cs`** - Upgrade feedback messages

#### Audio Systems (`Audio/`)
- **`EnhancedMusicManager.cs`** - Background music management
- **`EnhancedAudioBootstrap.cs`** - Audio system initialization

### Tutorial System (`Tutorials/`)
Complete setup guides for each system:
- Audio System Setup
- Camera System Setup
- Game Manager Setup
- Player Systems Setup
- Scoring System Setup
- Spawning System Setup
- UI System Setup
- Upgrade System Setup

## Key Technical Features

### Event System
Systems talk to each other through events instead of direct references, which makes the code more modular and easier to maintain than otherwise.

### Physics
- Simulated 'Rope' physics using DistanceJoint2D and LineRenderer
- Swinging mechanics with auto-contraction for a natural feeling
- Collision detection and bounce effects for feedback

### Procedural Generation
- 9+ different anchor patterns (clusters, lines, stairs, pyramids, etc.)
- Hazard anchors for strategic gameplay
- Difficulty gets harder over time

### Performance
- Object cleanup system prevents memory leaks
- Efficient object detection using name patterns
- Minimal Update() method usage

### Modular Design
- Each script does one thing
- Singleton pattern for core systems
- Component-based architecture

## What I Learned

This project taught me several game development concepts:

- **Event-driven programming** for loose coupling
- **Performance optimization** through object management
- **Procedural generation** techniques
- **Physics programming** with Unity's 2D physics
- **Modular architecture** for maintainable code
- **Documentation** and tutorial systems

This codebase works as both a game and a reference for my future projects, showing good organization and development practices.
