# Marwan Charafeddine — Game Development Portfolio

[![Unreal](https://img.shields.io/badge/Engine-Unreal-informational?logo=unreal-engine)](https://www.unrealengine.com/)
[![Unity](https://img.shields.io/badge/Engine-Unity-informational?logo=unity)](https://unity.com/)
[![C%2B%2B](https://img.shields.io/badge/Language-C%2B%2B-informational?logo=c%2B%2B)](https://isocpp.org/)
[![C%23](https://img.shields.io/badge/Language-C%23-informational?logo=c-sharp)](https://learn.microsoft.com/en-us/dotnet/csharp/)

I’m a gameplay-focused developer working primarily with **Unreal Engine 5** and **Unity**, with an interest in mechanics, systems design, and technical implementation.  
After completing my studies at Texas A&M, I returned to school to pursue game development—bringing a structured, analytical mindset into learning programming and interactive systems.

This repository serves as my **technical portfolio**: design documentation, system breakdowns, and code samples that complement my main website and playable projects.

🌐 **Portfolio Website:** https://mcharafeddinedev.github.io

---

## Technical Skills

### Languages
- C#, C++, Blueprint Visual Scripting  

### Engines
- Unreal Engine 5, Unity

### Focus Areas
- Gameplay programming & systems design  
- Mechanics and state-driven architecture  
- Prototyping and iteration  
- Procedural and physics-based systems  

### Topics I’m Actively Learning
- Memory and object management  
- Performance optimization  
- AI logic and behavior trees  
- Modular and engine-level architecture  

### Tools
- Visual Studio, Git, Trello, Jira  
- Unity 6, Unreal Engine 5, UE Blueprints Editor  

### Platforms
- Windows PC, Steam, Itch.io, Game Jams  

---

## Current Development Focus

### 🎮 Gameplay Prototyping
I’m currently developing an **endless-runner style arcade game in Unreal Engine (C++)**, focused on responsive movement, scoring systems, and replayable game loops.  
This project is where I spend most of my time refining moment-to-moment feel, structuring clean gameplay logic, and experimenting with state-driven mechanics.

### ⚙️ Technical Curiosity & Learning Projects
Alongside gameplay work, I occasionally explore lower-level or experimental projects—such as small engine experiments or editor automation tools. These are **not production projects**, but learning sandboxes that help me better understand how engines, tools, and pipelines work under the hood.

I don’t treat these as finished products; they exist to strengthen my fundamentals, improve how I think about systems, and inform my future gameplay work.

### 🧩 Tooling & Workflow Exploration
I also experiment with lightweight tooling and automation (including Python-based Unreal Editor scripts) to better understand development pipelines and improve iteration speed. These efforts are **purely exploratory**, but reflect my interest in how modern workflows and emerging tools can support creative development.

---

## Documentation

This repository functions as a technical archive alongside my website:

- **[Documentation Overview](Docs/README.md)** – Index of all materials and code samples  
- **[Quantum Tether](Docs/QuantumTether/)** – System design, source, and analysis  
- **[Project Experiences](Docs/MC_ProjectExperiences.pdf)** – Tech stacks and production notes  
- **[Ginger Shroom Journey – Code Analysis](Docs/GSJ_CSharp_Analysis.pdf)** – Architecture deep dive  
- **[Development Videos](Docs/VideoLinks.pdf)** – Process demos  
- **[Complete Source Code](Docs/GSJ_Scripts/)** – Full C# codebase  
- **[Tooling & Automation Notes](Docs/Python/ToolingAndAutomation.md)** – Experimental editor scripting and workflow exploration
- **[Custom 2D C++ Engine Project](Docs/EngineExperiments/README.md)** – Experimental game engine & editor debug tool

---

## Featured Project

### Quantum Tether — Unity 6 (TX Game Jam)
A 2D grappling-based endless runner built around physics-driven movement, procedural anchor generation, and modular gameplay systems.

**Key Systems**
- Event-based architecture for decoupled gameplay systems  
- Grappling mechanics using `DistanceJoint2D` and `LineRenderer`  
- Procedural anchor patterns (Fibonacci, wave functions, parametric math)  
- Upgrade and progression framework  
- Object cleanup and memory-conscious spawning  

---

## Code Example — Grappling Physics (Unity / C#)

```csharp
void TryGrapple(Vector2 targetPos)
{
    Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
    float distance = Vector2.Distance(transform.position, targetPos);

    if (distance > maxRopeLength) return;

    RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, grappleLayer);
    if (hit.collider != null)
    {
        CreateGrappleJoint(hit.point);
    }
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


