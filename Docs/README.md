# Documentation

This folder contains documentation and code samples from my game development projects, including my game jam winning project Quantum Tether and my published game Ginger Shroom Journey.

## What's Here

### Quantum Tether (`QuantumTether/`)
My game jam project from TX Game Jam 2024 - an infinite 2D grappling hook game with physics and procedural generation.

- **`README.md`** - Project overview and documentation guide
- **`Project_Analysis_and_Architecture.md`** - System architecture analysis
- **`Quantum_Thread_Post_Mortem.md`** - Development insights and what I learned
- **`README_System_Architecture.md`** - How all systems work together
- **`Scripts/`** - Source code organized by system (7,600+ lines)

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
