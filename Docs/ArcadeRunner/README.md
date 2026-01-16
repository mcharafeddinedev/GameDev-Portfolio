# StateRunner: Arcade (Updated Janurary 16, 2025)

**Engine:** Unreal Engine 5.7+  
**Language:** C++ (with Blueprint integration)  
**Platform:** Windows PC (Arcade Cabinet Target)

StateRunner: Arcade is a fast-paced endless runner set inside a stylized computer system. You play as an electric impulse racing through neon-lit circuitry, dodging obstacles, collecting data packets, and pushing for high scores.

What makes the game unique is that **you control how the world behaves**. Rather than simply reacting to obstacles, you actively switch the environment between gameplay “states” that alter physics, collision rules, and risk/reward in real time.

---

## 🎮 Core Concept

StateRunner is built around a **state-driven world system**, not a traditional game-state machine. The player can dynamically change how the environment behaves while running:

### World Behavior States

- **FLOW (Default State)**  
  Standard gameplay. Obstacles behave normally and block the player.

- **PHASE** *(Planned)*  
  Allows the player to pass through specific obstacles, providing a temporary safety valve when the course becomes overwhelming.

- **OVERCLOCK** *(Planned)*  
  A high-risk, high-reward mode accessible only from FLOW. The world speeds up dramatically and score multipliers increase, but obstacle density and reaction demands spike.

Unlike menu or lifecycle states, these modes are **active gameplay mechanics** that modify collision rules, speed, scoring, and player agency while the run is in progress. This gives players strategic control over the environment rather than simply responding to it.

*(Note: The current prototype focuses on core movement, scrolling, and scoring systems. PHASE and OVERCLOCK are part of the full design and will be layered in once the foundation is solid.)* :contentReference[oaicite:0]{index=0}

---

## 🏃 Gameplay Loop

- Dodge obstacles using **lane switching, jumping, and sliding**  
- Collect **Data Packets** for score bonuses  
- Survive as the world **scrolls faster and grows denser**  
- Strategically manipulate world behavior through state switching  
- Compete for high scores in short, arcade-style sessions  

Designed for **“one more run” replayability** and future arcade cabinet deployment.

---

## ⚙️ Technical Focus

StateRunner is being developed as a **systems-driven C++ project** with performance, modularity, and scalability in mind.

### Core Systems

- **World Scrolling System**  
  Procedural track segments scroll past the player to simulate continuous forward motion.

- **Player Movement System**  
  - Smooth lane switching  
  - Jump and slide mechanics  
  - Forward-motion illusion maintained during lateral movement  

- **Obstacle & Pickup Systems**  
  - Obstacles and Data Packets spawn per track segment  
  - Collision-driven life loss and score rewards  

- **Score & Lives System**  
  - Time-based scoring with stepped difficulty scaling  
  - Bonus scoring from pickups  
  - Local high-score tracking  

- **UI Framework**  
  - Score display  
  - Lives indicator  
  - Game-over screen with restart flow  

---

## 🧠 Architecture

StateRunner uses a **component-based, data-driven design**:

### Systems as Components
Core systems are implemented as modular components (attached to the GameMode), including:
- World scrolling
- Obstacle spawning
- Pickup management
- Score tracking
- (Planned) State control logic for FLOW / PHASE / OVERCLOCK

### Event-Driven Communication
Systems communicate via **delegates/events**, minimizing tight coupling and reducing per-frame polling.

### Data-Driven Content
- Obstacle types, spawn patterns, and difficulty curves are configured through **Data Assets and Data Tables**.
- This allows rapid iteration, tuning, and expansion without recompiling code.

### Performance-Oriented Design
- Object pooling for obstacles and pickups  
- Despawning and culling behind the player  
- Minimal Tick usage; timers and events preferred  
- Simple geometry and material instancing to reduce draw calls  

---

## 🛠️ Development Status

**Current Phase:** Playable Prototype (Core Gameplay Systems)

### Implemented
- Character movement (lanes, jump, slide)  
- Procedural world scrolling  
- Obstacle spawning and avoidance  
- Data Packet pickups  
- Score and lives system  
- Core UI and game-over flow  

### Planned (Post-Prototype)
- PHASE state (selective obstacle pass-through)  
- OVERCLOCK state (speed boost with scoring multipliers)  
- Advanced visuals, audio, and local leaderboard  

Development follows a strict **“gameplay first, polish later”** philosophy to ensure the core loop is engaging, performant, and extensible before layering on advanced mechanics. :contentReference[oaicite:1]{index=1}

---

## 🧩 Design Goals

- **Skill-Based Play:** Reflex-driven movement enhanced by strategic world manipulation  
- **Readable Systems:** Clear separation of responsibilities between systems  
- **Iterative Architecture:** Data-driven tuning and modular expansion  
- **Arcade-Ready Performance:** 60 FPS target on cabinet hardware  
- **High Replayability:** Short sessions with escalating intensity  

---

## 👤 Developer

**Marwan Charafeddine**  
Solo Developer — Game Design, C++ Programming, Systems Architecture, and Technical Direction

- Unreal Engine 5.7+ (C++)  
- Blueprint integration for iteration and testing  
- Data-driven and performance-focused design  
- Built for future arcade cabinet deployment  

---

## 📌 Notes

This project emphasizes **technical architecture, gameplay systems, and professional development practices** over rapid prototyping alone. It will serve as both a playable arcade game and a demonstration of scalable game system design.

