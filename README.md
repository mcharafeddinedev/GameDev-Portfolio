# Marwan Charafeddine — Game Development Portfolio

[![Unreal](https://img.shields.io/badge/Engine-Unreal-informational?logo=unreal-engine)](https://www.unrealengine.com/)
[![Unity](https://img.shields.io/badge/Engine-Unity-informational?logo=unity)](https://unity.com/)
[![C%2B%2B](https://img.shields.io/badge/Language-C%2B%2B-informational?logo=c%2B%2B)](https://isocpp.org/)
[![C%23](https://img.shields.io/badge/Language-C%23-informational?logo=c-sharp)](https://learn.microsoft.com/en-us/dotnet/csharp/)

I'm a gameplay-focused developer working primarily with **Unreal Engine 5** and **Unity**, with an interest in mechanics, systems design, and technical implementation.  
After completing my studies at Texas A&M, I returned to school to pursue game development—bringing a structured, analytical mindset into learning programming and interactive systems.

---

> **📂 This repository is a technical supplement to my portfolio website.**  
> It contains design documentation, system architecture breakdowns, and code samples for my projects.  
> For screenshots, gameplay videos, playable builds, and a complete project overview, visit my main site:
>
> 🌐 **[mcharafeddinedev.github.io](https://mcharafeddinedev.github.io)**

---

---

## Technical Skills

### Languages
- **C++**, C#, Blueprint Visual Scripting  

### Engines
- Unreal Engine 5, Unity

### Focus Areas
- Gameplay programming & systems design  
- Component-based and state-driven architecture  
- Procedural generation and data-driven systems  
- Prototyping & Iteration

### Topics I'm Actively Learning
- Memory and object management  
- Performance optimization  
- AI logic and behavior trees  
- Modular and engine-level architecture  

### Tools
- Visual Studio, Rider, Git, Trello, Jira  
- Unity 6, Unreal Engine 5, UE Blueprints Editor  

### Platforms
- Windows PC, Steam, Itch.io, Arcade Cabinet Hardware, Game Jams  

---

## Current Development Focus

### 🎮 Recent Release: OVERCLOCKED: Data Dash MAX
I recently shipped **[OVERCLOCKED: Data Dash MAX](https://goldleafinteractive.itch.io/overclocked-ddm)** — a complete arcade endless runner built in **Unreal Engine 5 (C++)**. This is my most technically polished project to date: component-based architecture, 45+ hand-crafted obstacle patterns with procedural generation, and deployed to arcade cabinet hardware at stable 60fps.

### ⚙️ Technical Curiosity & Learning Projects
Alongside gameplay work, I occasionally explore lower-level or experimental projects—such as small engine experiments or editor automation tools. These are often **not production projects**, though they can turn into them sometimes, but rather they are learning sandboxes that help me better understand how engines, tools, and pipelines work under the hood.

I don't treat these as finished products; they exist to strengthen my fundamentals, improve how I think about systems, and inform my future gameplay work.

### 🧩 Tooling & Workflow Exploration
I also experiment with lightweight tooling and automation (including Python-based Unreal Editor scripts) to better understand development pipelines and improve iteration speed. These efforts are **purely exploratory**, but reflect my interest in how modern workflows and emerging tools can support creative development.

---

## Documentation

This repository functions as a technical archive alongside my website:

- **[Documentation Overview](Docs/README.md)** – Index of all materials and code samples  
- **[OVERCLOCKED: Data Dash MAX](Docs/OVERCLOCKED-DataDashMAX/)** – System design and C++ code samples  
- **[Quantum Tether](Docs/QuantumTether/)** – System design, source, and analysis  
- **[Project Experiences](Docs/MC_ProjectExperiences.pdf)** – Tech stacks and production notes  
- **[Ginger Shroom Journey – Code Analysis](Docs/GSJ_CSharp_Analysis.pdf)** – Architecture deep dive  
- **[Development Videos](Docs/VideoLinks.pdf)** – Process demos  
- **[Complete Source Code](Docs/GSJ_Scripts/)** – Full C# codebase  
- **[Tooling & Automation Notes](Docs/Python/ToolingAndAutomation.md)** – Experimental editor scripting and workflow exploration
- **[Custom 2D C++ Engine Project](Docs/EngineExperiments/README.md)** – Experimental game engine & editor debug tool

---

## Featured Projects

### OVERCLOCKED: Data Dash MAX — Unreal Engine 5 (C++)
A high-speed arcade endless runner where you play as an electric impulse racing through neon-lit circuitry. Built with a component-based C++ architecture and shipped to arcade cabinet hardware.

**Links:** [Itch.io](https://goldleafinteractive.itch.io/overclocked-ddm) | [YouTube Trailer](https://www.youtube.com/watch?v=dI9Ctq9LkLs) | [Documentation](Docs/OVERCLOCKED-DataDashMAX/)

**Technical Highlights**
- **UE5 C++ with Blueprint wrappers** — Clean C++ core systems exposed to Blueprints for rapid iteration  
- **Component-based architecture** — WorldScrollComponent, ObstacleSpawnerComponent, PickupSpawnerComponent, ScoreSystemComponent, OverclockSystemComponent, ThemeSubsystem  
- **45+ hand-crafted obstacle patterns** — Combined with procedural generation and fairness validation  
- **Data-driven subsystems** — 6 color themes, combo scoring, local leaderboard with initials entry, persistent settings  
- **Full keyboard/gamepad-navigable UI** — Zone-based menu navigation, music player system  
- **Risk/reward OVERCLOCK system** — Hold to activate speed boost with score multipliers, meter drain mechanics  
- **Shipped to arcade cabinet hardware** — Stable 60fps on target hardware  

---

### Quantum Tether — Unity 6 (TX Game Jam)
A 2D grappling-based endless runner built around physics-driven movement, procedural anchor generation, and modular gameplay systems.

**Key Systems**
- Event-based architecture for decoupled gameplay systems  
- Grappling mechanics using `DistanceJoint2D` and `LineRenderer`  
- Procedural anchor patterns (Fibonacci, wave functions, parametric math)  
- Upgrade and progression framework  
- Object cleanup and memory-conscious spawning  

---

## Code Example — OVERCLOCK System (Unreal / C++)

```cpp
void UOverclockSystemComponent::ActivateOverclock()
{
    if (bIsOverclockActive) return;
    
    bIsOverclockActive = true;

    // Apply speed multiplier to world scroll
    if (WorldScrollComponent)
    {
        WorldScrollComponent->SetOverclockMultiplier(SpeedMultiplier, true);
    }

    // Activate bonus scoring
    if (ScoreSystemComponent)
    {
        ScoreSystemComponent->SetOverclockActive(true);
    }

    // Trigger camera zoom for intensity
    if (AStateRunner_ArcadeCharacter* Character = GetPlayerCharacter())
    {
        Character->SetOverclockZoom(true);
    }

    // Audio feedback
    PlayOverclockSound(OverclockActivateSound);
    StartOverclockLoop();

    OnOverclockStateChanged.Broadcast(true);
}
```

## About Me
- B.S. in Veterinary Medicine from Texas A&M University at College Station
- A.A.S. in Digital Gaming & Simulation for Programmers from Houston Community College
- Currently working on gameplay programming and systems design

---

## Contact  

[![LinkedIn](https://img.shields.io/badge/LinkedIn-Profile-blue?logo=linkedin)](https://www.linkedin.com/in/marwan-charafeddine-213065155) 
[![Email](https://img.shields.io/badge/Email-Contact%20Me-red?logo=gmail)](mailto:mcharafeddinedev@gmail.com) 
[![Itch.io](https://img.shields.io/badge/Itch.io-Portfolio-critical?logo=itch.io)](https://goldleafinteractive.itch.io/) 
[![Steam](https://img.shields.io/badge/Steam-Projects-lightgrey?logo=steam)](https://store.steampowered.com/app/3023100/Ginger_Shroom_Journey/) 
[![GitHub](https://img.shields.io/badge/GitHub-Portfolio-black?logo=github)](https://github.com/mcharafeddinedev)
