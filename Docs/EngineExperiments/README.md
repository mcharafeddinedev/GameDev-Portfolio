# RB Engine — Custom 2D C++ Game Engine & Editor

**Language:** C++  
**Platform:** Windows  
**Architecture:** ECS (EnTT)  
**Rendering & Input:** SDL2  
**Math:** GLM  
**Serialization:** nlohmann/json  
**Editor UI:** Dear ImGui  
**Status:** Alpha / Experimental

---

## Overview

**RB Engine** is a custom-built 2D game engine and editor written from scratch in C++. What began as a single game project idea to build a C++ platformer from the ground up evolved into a general-purpose engine as systems were abstracted, stabilized, and organized into reusable architecture.

This project exists as a **technical exploration of engine design, tooling, and system architecture**, with an emphasis on:
- Clean separation between engine and game projects  
- Editor-first workflows  
- Modular, extensible systems  
- Practical understanding of how commercial engines are structured internally  

RB Engine is not intended to compete with established engines. It is a learning-driven project and a simple demonstration of low-level game engine technology and tooling.

---

## Design Philosophy

- **Engine / Project Separation**  
  The engine is designed to remain generic. Individual games live as independent projects layered on top of it.

- **Tool-First Workflow**  
  Emphasis on editor usability, inspection, debugging, and rapid iteration—not just runtime features.

- **Perspective-Agnostic 2D**  
  Meant to support side-scroller, top-down, and isometric-style games through camera-relative systems.

- **Learning Through Construction**  
  Every major system was designed and implemented manually to understand how professional engines structure their pipelines.

---

## Current Capabilities

### Editor & Workflow
- Project creation and management  
- Scene hierarchy, inspector, and content browser  
- Dockable editor layout using Dear ImGui  
- In-editor documentation and system views  
- JSON-based settings, scenes, and project serialization  

### Engine Systems
- Core runtime loop (windowing, input, time, logging)  
- ECS-driven entity architecture (EnTT)  
- Rendering system with camera-relative drawing  
- Physics and collision (AABB-based)  
- Scene loading, saving, and project management  
- Asset import and management pipeline  

### Scripting Model
- C++ “MonoBehaviour-style” scripting with lifecycle hooks  
- Script registry for attachable behaviors  
- Modular, component-driven entity logic  

---

## Architecture Overview

RB_Engine/
├── src/ # Core engine systems
├── Projects/ # Game projects built on the engine
├── docs/ # Technical documentation
├── assets/ # Engine-level assets
└── CMakeLists.txt # Build configuration

- **Engine Layer:** Rendering, ECS, physics, asset management, editor  
- **Project Layer:** Game-specific logic, content, scripts, scenes  
- **Tooling Layer:** Editor UI, inspectors, debugging panels, project controls  

---

## Purpose of the Project

RB Engine exists to answer a fundamental engineering question:

> **“Can I design and implement a real, reusable and user friendly game engine and editor using modern C++ and established libraries?”**

It serves as:
- A **technical learning platform** for engine architecture  
- A **portfolio artifact** demonstrating systems-level thinking, tooling, and an interest in low-level implementation  

---

## Future Direction

- Expanded asset pipeline and editor tooling  
- More advanced physics and rendering features  
- Improved scripting and modular system design  
- Continued architectural refinement and performance tuning  

---

## Author

**Marwan Charafeddine**  
Gameplay Programming • Systems Design  
Gold Leaf Interactive


