# Agentic Python Bridge Tool - UE Editor Automation Framework

*Procedural Layout • Command Queues • Developer Tooling • AI-Assisted Workflows*

---

## Overview

This project is a **Python-based automation framework for Unreal Engine editor workflows**. It enables rapid, repeatable creation and manipulation of level geometry, actor placement, and scene structure through **structured command files** rather than manual editor operations.

The framework is designed as a **developer tool**, not a gameplay system. It supports:

- Greyboxing and layout prototyping  
- Procedural level assembly  
- Batch actor operations  
- Editor automation and internal utilities  

The goal is to improve iteration speed, maintain structural consistency, and explore how modern tooling and automation can augment traditional game development workflows.

---

## What This Tool Is

- A **command-driven editor automation layer** built on Unreal’s Python API  
- A framework for **procedural layout and structural composition**  
- A foundation for **internal tooling and pipeline automation**

## What This Tool Is Not

- ❌ Not a gameplay scripting system  
- ❌ Not runtime AI or behavior logic  
- ❌ Not a replacement for Blueprints or C++  

This system operates entirely at the **editor and development pipeline level**.

---

## Core Concepts

### 1. Command-Based Automation

All editor actions are expressed as **structured JSON command files**.  
Each command represents a single, well-defined operation such as:

- Spawning actors  
- Modifying transforms and materials  
- Creating structural assemblies  
- Organizing folders and tags  
- Exporting or validating scene data  

Commands are **modular, composable, and auditable**, allowing complex workflows and structures to be built from simple building blocks.

---

### 2. Queue-Based Execution

Commands are placed into a queue directory and executed in batch via a Python processor inside Unreal Editor.

This enables:

- Repeatable and deterministic editor actions

- Dry-run previews before applying changes

- Logging and validation of results

- Clean separation between *intent* and *execution*

---

### 3. Project-Aware Structure

The framework enforces:

- Consistent World Outliner organization

- Standardized tagging conventions

- Reusable layout patterns

- Project-specific constraints and safety policies  

This allows the same tooling to be reused across multiple prototypes and production experiments.

---

## Role of the AI Agentic Assistant

### Why an AI Assistant Is Currently Essential

In its current form, this framework is **not directly embedded into Unreal Engine’s UI** and does not provide an in-editor graphical interface or interactive prompt system. Instead, the workflow depends on:

1. Authoring structured command files (JSON)  
2. Executing them via Python inside the editor  
3. Iterating by refining commands and re-running queues  

To make this process **fast, ergonomic, and practical**, an **AI agentic assistant** like 'Cursor Agent' can be used to:

- Translate high-level intent (e.g., “build a corridor with obstacles and pickups”) into structured command files

- Compose and modify JSON safely and consistently

- Generate or adapt Python handlers for new command types

- Diagnose errors from execution logs and suggest corrections  

Without an assistant, authoring and iterating on large batches of commands would be technically possible but **slow and error-prone**. The AI effectively acts as a **command authoring layer**, accelerating development while the underlying framework remains deterministic and fully auditable.

---

### How This Differs from Other AI-Integrated Tools

Some commercial or experimental tools embed AI agents **directly into the Unreal Editor**, providing chat panels, inline generation, or direct scene manipulation.

This framework intentionally does **not** do that.

Instead:
- The AI agent operates **externally**, generating adaptive commands and scripts outside the project files, allowing the user full control.

- For maximum efficiency/reliablity, the tool/framework assumes the user is using a project aware Agentic AI assistant.
  
- Unreal remains the authoritative execution environment, UE's Python (REPL) terminal allows for direct Python script execution.

This preserves:
- Tooling transparency

- Deterministic results

- Version-controlled workflows  

The AI assists with *authoring and reasoning*, while the engine executes only explicit, validated commands.

---

## Example: JSON Command

A simple command to spawn a static obstacle at a specific location:

```json
{
  "action": "spawn_static_obstacle",
  "params": {
    "name": "TestObstacle_01",
    "mesh": "/Engine/BasicShapes/Cube",
    "location": [0, 0, 100],
    "scale": [1.5, 1.5, 1.5],
    "tags": ["Obstacle", "Test"],
    "folder": "Gameplay/Obstacles"
  }
}
```
This file is placed into:
AI/Commands/

In practice, this file is often generated or modified with the assistance of the AI agent, based on higher-level design intent.

## Example: Python Action Handler

Each command maps to a Python function inside the framework:

```
def spawn_static_obstacle(params):
    name = params.get("name", "StaticObstacle")
    mesh_path = params["mesh"]
    location = params["location"]
    scale = params.get("scale", [1, 1, 1])
    tags = params.get("tags", [])
    folder = params.get("folder", "Generated")

    actor = unreal.EditorLevelLibrary.spawn_actor_from_object(
        unreal.load_object(None, mesh_path),
        unreal.Vector(*location)
    )

    actor.set_actor_scale3d(unreal.Vector(*scale))

    for tag in tags:
        actor.tags.append(tag)

    unreal.EditorActorSubsystem().set_folder_path(actor, folder)

    return f"Spawned {name} at {location}"
```

Each handler:

-Performs one specific task

-Enforces project constraints

-Logs results for validation

The AI assistant can help generate new handlers or extend existing ones, but all execution remains explicit and controlled.

## Example: Batch Execution (Queue Processing)

The processor script scans all command files and executes them:

```
exec(open(r"C:\UnrealProjects\StateRunner\AI\Scripts\run_ai_queue.py", encoding="utf-8").read())
```

What Happens:

1. All .json commands in AI/Commands/ are loaded

2. Commands are validated against allowed actions

3. Each action handler is executed in sequence

4. Results are logged to:
'AI/Commands/_results/results_log.json'

5. Processed commands are archived or removed

This enables dozens or hundreds of editor operations to be executed in a single step.

## Example: Actor Manipulation
A command to modify materials on a group of actors:

```
{
  "action": "modify_actor_material",
  "params": {
    "filter": {
      "tags": ["Obstacle"]
    },
    "material": "/Game/Materials/M_EmissiveRed",
    "folder": "Gameplay/Obstacles"
  }
}
```
This supports:

-Batch material updates

-Theming passes

-Visual iteration without manual selection

## Example: Exporting Level State
Commands can export scene data for validation or tooling:

```
{
  "action": "export_level_snapshot",
  "params": {
    "output_file": "AI/Exports/level_snapshot.json",
    "include_tags": true,
    "include_transforms": true
  }
}
```

Resulting JSON can be used for:

-Debugging

-Automated testing

-Procedural analysis

-External tools and validation scripts

# Design Philosophy
## Modular & Safe

Each command performs one job well and can be composed with others. All actions are validated against allowed operations and project rules to ensure full user/developer control.

## Transparent

Every command is explicit, logged, and reviewable.

## AI-Assisted, Not AI-Driven

The AI agent helps author and reason about commands, but:

-Does not execute changes directly

-Does not bypass validation

-Does not modify the editor without explicit command files

This keeps the pipeline deterministic and auditable.

## Why This Matters

Modern game development increasingly depends on tooling, automation, and scalable workflows. This framework demonstrates:

-Editor scripting and internal pipeline design

-Structured, reproducible environment generation

-Developer tool construction, not just gameplay features

-A forward-looking workflow that integrates AI assistance without sacrificing control or clarity

This project reflects a focus on engine-level thinking, automation, and professional production tooling—skills directly applicable to technical game development and tools programming roles.

## Status

Active R&D project.

Currently used as a sandbox for:

-Editor automation

-Procedural layout generation

-Batch scene construction

-Internal pipeline experimentation

-AI-assisted command authoring




