# Systems Overview — OVERCLOCKED: Data Dash MAX

Detailed breakdown of all core gameplay systems and their interactions.

---

## World Scroll System

**Component:** `UWorldScrollComponent`

The world scroll system creates the illusion of forward movement by moving track segments past a stationary player.

### Responsibilities
- Manage base scroll speed and ramping over time
- Apply OVERCLOCK speed multiplier (1.5x when active)
- Apply damage slowdown (0.7x for hit feedback)
- Broadcast speed changes for dependent systems

### Key Properties
```cpp
float BaseScrollSpeed = 1500.0f;      // Initial speed
float MaxScrollSpeed = 4000.0f;       // Speed cap
float SpeedIncreasePerSecond = 15.0f; // Ramp rate
float OverclockMultiplier = 1.5f;     // Speed boost when active
float DamageSlowdownMultiplier = 0.7f; // Slowdown on hit
float DamageSlowdownDuration = 0.5f;   // Duration of slowdown
```

### Speed Calculation
```
FinalSpeed = BaseSpeed × OverclockMultiplier × DamageSlowdownMultiplier
```

All multipliers stack, allowing the game to feel impactful when taking damage even during high-speed OVERCLOCK runs.

---

## Player Movement System

**Class:** `AStateRunner_ArcadeCharacter`

Handles all player input and movement across three lanes with jump and slide actions.

### Lane System
- **3 lanes:** Left (-375 Y), Center (0 Y), Right (+375 Y)
- **Smooth interpolation** between lanes at configurable speed
- **Input queuing** for held directional inputs
- **Tick optimization:** Only enabled during active movement

### Movement Actions
| Action | Axis | Description |
|--------|------|-------------|
| Lane Switch | Y | Smooth interpolation to adjacent lane |
| Jump | Z | Parabolic arc with configurable height/duration |
| Slide | Z | Lower hitbox, maintain forward momentum |

### Collision Handling
```cpp
void OnCollisionEnter(UPrimitiveComponent* HitComp, AActor* OtherActor, ...)
{
    if (AObstacleBase* Obstacle = Cast<AObstacleBase>(OtherActor))
    {
        if (!bIsInvincible)
        {
            // Notify lives system
            LivesSystemComponent->TakeDamage();
            
            // Apply damage slowdown for feel
            WorldScrollComponent->ApplyDamageSlowdown();
            
            // Start invincibility frames
            StartInvincibility(InvincibilityDuration);
        }
    }
}
```

---

## Obstacle Spawner System

**Component:** `UObstacleSpawnerComponent`

Manages obstacle spawning with hand-crafted patterns and procedural generation.

### Pattern System
- **45+ hand-crafted patterns** in Data Tables
- **Difficulty-gated patterns** unlock as game progresses
- **Fairness validation** ensures all patterns are survivable
- **Pattern cooldown** prevents immediate repetition

### Spawning Logic
```cpp
void SpawnNextPattern()
{
    // Select pattern based on current difficulty
    FObstaclePatternData Pattern = SelectValidPattern(CurrentDifficultyLevel);
    
    // Validate fairness (player can dodge all obstacles)
    if (!ValidatePatternFairness(Pattern))
    {
        Pattern = GetFallbackPattern();
    }
    
    // Spawn obstacles from pool
    for (const FObstacleSpawnInfo& Info : Pattern.Obstacles)
    {
        AObstacleBase* Obstacle = AcquireFromPool(Info.ObstacleClass);
        Obstacle->SetActorLocation(CalculateSpawnPosition(Info));
        Obstacle->Initialize(Info);
    }
}
```

### Object Pooling
- Pre-allocated obstacle pool reduces runtime allocations
- Obstacles returned to pool when passing behind camera
- Pool size scales with maximum simultaneous obstacles

---

## Pickup Spawner System

**Component:** `UPickupSpawnerComponent`

Manages all collectible pickups in the game.

### Pickup Types
| Type | Effect | Spawn Rate |
|------|--------|------------|
| Data Packet | +50 points, builds combo | Common |
| 1-UP | +1 life (or +500 points at max) | Rare |
| EMP | Destroys nearby obstacles, +OVERCLOCK | Very Rare |
| Magnet | Attracts nearby data packets | Rare |

### Spawn Coordination
- Pickups spawn in lanes **not blocked by obstacles**
- Spawn rate scales with difficulty
- Magnet pickup enables temporary attraction behavior

---

## Score System

**Component:** `UScoreSystemComponent`

Handles all scoring, combo tracking, and leaderboard management.

### Scoring Methods
| Source | Points | Notes |
|--------|--------|-------|
| Time (base) | 10/sec | Increases +5 every 5 seconds |
| Time (OVERCLOCK) | +200/sec | Bonus while OVERCLOCK active |
| Data Packet | +50 | Base pickup value |
| NICE Combo (6x) | +300 | 6 pickups in 4 seconds |
| INSANE Combo (10x) | +1000 | 10 pickups in 8 seconds |
| 1-UP at max | +500 | When collecting at max lives |
| EMP obstacles | +25/obstacle | Per destroyed obstacle |

### Combo System
Tracks pickup timestamps in sliding windows:
- **NICE window:** 4 seconds, requires 6 pickups
- **INSANE window:** 8 seconds, requires 10 pickups
- INSANE cancels pending NICE popup (priority system)

### Leaderboard
- Top 10 local scores with initials
- Stores: score, run time, date, player initials
- Persistent via `USaveGame`

---

## OVERCLOCK System

**Component:** `UOverclockSystemComponent`

The core risk/reward mechanic that defines high-level play.

### Meter Behavior
- **Passive fill:** Meter fills slowly over time
- **Pickup boost:** Data packets add meter
- **EMP instant charge:** EMP pickup fully charges meter
- **Drain on use:** Meter depletes while OVERCLOCK active

### Activation Effects
| System | Effect |
|--------|--------|
| World Scroll | 1.5x speed multiplier |
| Score | +200 points/second bonus |
| Camera | Zoom in for intensity |
| Audio | Activating sound + loop |
| UI | Meter glow, speed lines |

### Risk/Reward Balance
- **Risk:** Faster speed = less reaction time
- **Reward:** Higher score rate + multiplier
- **Cooldown:** Cannot reactivate immediately after deactivation

---

## Lives System

**Component:** `ULivesSystemComponent`

Manages player health and game over detection.

### Properties
```cpp
int32 MaxLives = 3;
int32 CurrentLives = 3;
float InvincibilityDuration = 2.0f;  // After taking damage
```

### Damage Flow
```
[Obstacle Collision]
    │
    ├─ Check invincibility → If invincible, ignore
    │
    └─ Apply damage
        ├─ Decrement lives
        ├─ Broadcast OnLivesChanged
        ├─ Start invincibility timer
        ├─ Trigger damage slowdown (WorldScroll)
        │
        └─ If lives == 0
            └─ Broadcast OnPlayerDied → Game Over
```

### 1-UP Behavior
When player collects 1-UP:
- If lives < max: Add 1 life
- If lives == max: Award bonus points instead

---

## Theme Subsystem

**Class:** `UThemeSubsystem` (World Subsystem)

Data-driven color theme system for visual customization.

### Available Themes
| Theme | Primary Color | Description |
|-------|---------------|-------------|
| Cryogenic (Cyan) | #00FFFF | Default icy blue |
| Voltage (Orange) | #FF8800 | Electric orange |
| Synthwave (Purple/Pink) | #FF00FF | Retro purple |
| Matrix (Green) | #00FF00 | Classic green |
| Ruby | #FF0044 | Deep red |
| Gold | #FFD700 | Premium gold |

### Theme Data Asset Structure
```cpp
UCLASS(BlueprintType)
class UThemeDataAsset : public UDataAsset
{
    FText DisplayName;                    // "Cryogenic"
    FLinearColor CircuitEmissiveColor;    // Base glow color
    float CircuitEmissiveIntensity;       // Brightness multiplier
    UMaterialInterface* LaserPointerMaterial; // Obstacle laser material
};
```

### Theme Application
1. Meshes register with subsystem on spawn
2. Subsystem applies current theme's materials
3. On theme change, all registered meshes refresh
4. Theme preference persists to config file

---

## UI System

### HUD Elements
- **Score display** — Current and high score
- **Lives indicator** — Visual hearts/icons
- **OVERCLOCK meter** — Fill bar with glow
- **Combo popups** — NICE! and INSANE! notifications

### Menu Navigation
- **Zone-based system** — Logical groupings of buttons
- **Keyboard/gamepad support** — Full navigation without mouse
- **Leaderboard display** — Scrollable top 10
- **Initials entry** — Per-character selection (A-Z, 0-9)

### Event-Driven Updates
All HUD elements bind to component delegates:
```
ScoreSystemComponent::OnScoreChanged → Update score text
LivesSystemComponent::OnLivesChanged → Update life icons
OverclockSystemComponent::OnOverclockMeterChanged → Animate meter
```

---

## Audio System

### Sound Categories
| Category | Examples |
|----------|----------|
| Player | Jump, slide, lane switch |
| Pickups | Data packet, 1-UP, EMP |
| OVERCLOCK | Activate, deactivate, loop |
| Damage | Hit, death |
| UI | Menu navigate, confirm |
| Music | Main theme, OVERCLOCK variant |

### Pitch Scaling
Data packet collection uses pitch ramping for combo feedback:
```cpp
float GetCurrentPickupPitchMultiplier() const
{
    // Pitch ramps from 1.0 to 1.5 based on combo streak
    float Pitch = PickupPitchMin + (StreakCount - 1) * PickupPitchStep;
    return FMath::Clamp(Pitch, PickupPitchMin, PickupPitchMax);
}
```

---

## Debug System

**Class:** `UGameDebugSubsystem`

Runtime debugging tools for development and QA.

### Features
- On-screen stat display (score, speed, lives, OVERCLOCK)
- Event logging with categories
- Debug score/time overrides for testing
- Leaderboard manipulation commands

### Debug Commands (Development Only)
```cpp
DebugInitialScore = 50000;      // Start with specific score
DebugInitialTimeElapsed = 120.0f; // Skip early game
```
