# System Architecture — OVERCLOCKED: Data Dash MAX

## Design Philosophy

OVERCLOCKED uses a **component-based, event-driven architecture** where:

1. **GameMode owns systems** — Core gameplay components attach to `AStateRunner_ArcadeGameMode`
2. **Systems communicate via delegates** — Minimizes coupling, enables reactive UI
3. **Data drives content** — Obstacle patterns, themes, and difficulty configured via assets
4. **Tick is minimized** — Timers and events preferred for performance

---

## Component Architecture

### GameMode Components

The `AStateRunner_ArcadeGameMode` acts as the central orchestrator, owning all gameplay-critical components:

```cpp
UPROPERTY(VisibleAnywhere, BlueprintReadOnly)
TObjectPtr<UWorldScrollComponent> WorldScrollComponent;

UPROPERTY(VisibleAnywhere, BlueprintReadOnly)
TObjectPtr<UObstacleSpawnerComponent> ObstacleSpawnerComponent;

UPROPERTY(VisibleAnywhere, BlueprintReadOnly)
TObjectPtr<UPickupSpawnerComponent> PickupSpawnerComponent;

UPROPERTY(VisibleAnywhere, BlueprintReadOnly)
TObjectPtr<UScoreSystemComponent> ScoreSystemComponent;

UPROPERTY(VisibleAnywhere, BlueprintReadOnly)
TObjectPtr<UOverclockSystemComponent> OverclockSystemComponent;

UPROPERTY(VisibleAnywhere, BlueprintReadOnly)
TObjectPtr<ULivesSystemComponent> LivesSystemComponent;
```

### System Responsibilities

| Component | Primary Responsibility | Key Interactions |
|-----------|----------------------|------------------|
| `WorldScrollComponent` | Track speed, scrolling, damage feedback | ← OverclockSystem, LivesSystem |
| `ObstacleSpawnerComponent` | Obstacle patterns, pooling, difficulty | ← WorldScroll (speed), ScoreSystem (difficulty) |
| `PickupSpawnerComponent` | Pickup spawning, magnet behavior | ← ObstacleSpawner (positions) |
| `ScoreSystemComponent` | Points, combos, leaderboard | ← OverclockSystem, PickupSpawner |
| `OverclockSystemComponent` | Meter, activation, speed bonus | → WorldScroll, ScoreSystem, Character |
| `LivesSystemComponent` | Health, invincibility, game over | → WorldScroll, GameMode |

---

## Coordinate System

### World Orientation (Non-Standard)

```
           +Z (up)
            │
            │
            │
    ────────┼────────→ +Y (track width / lanes)
           ╱
          ╱
         ╱
        +X (track length, world scrolls in -X)
```

### Player Constants

```cpp
// Player is locked at this X position — world moves past them
static constexpr float LockedXPosition = -5000.0f;

// Base Z height (ground level for player)
static constexpr float BaseZPosition = 150.0f;

// Lane Y positions
float CenterLaneY = 0.0f;
float LeftLaneY = -LaneWidth;   // e.g., -375.0f
float RightLaneY = +LaneWidth;  // e.g., +375.0f
```

### Why This Matters

When working with this codebase, remember:
- **Lane switching** = Y-axis interpolation
- **Jumping/sliding** = Z-axis modification
- **World scroll** = Negative X direction
- All spawn positions are in **world space** relative to the camera

---

## Event-Driven Communication

### Delegate Pattern

Systems expose delegates that UI widgets and other systems subscribe to:

```cpp
// ScoreSystemComponent.h
DECLARE_DYNAMIC_MULTICAST_DELEGATE_OneParam(FOnScoreChanged, int32, NewScore);

UPROPERTY(BlueprintAssignable, Category = "Score")
FOnScoreChanged OnScoreChanged;
```

### Example: Score Update Flow

```
[ScoreSystemComponent::AddScore()]
    │
    ├──→ OnScoreChanged.Broadcast(NewScore)
    │       │
    │       └──→ [WBP_HUD::OnScoreChanged()] → Update score text
    │
    └──→ Check high score → OnHighScoreBeaten.Broadcast()
```

### Common Delegates

| Component | Delegate | Purpose |
|-----------|----------|---------|
| ScoreSystem | `OnScoreChanged` | Update HUD score display |
| ScoreSystem | `OnHighScoreBeaten` | Trigger celebration VFX |
| ScoreSystem | `OnNiceCombo` | Display combo popup |
| OverclockSystem | `OnOverclockStateChanged` | Update HUD meter/effects |
| OverclockSystem | `OnOverclockMeterChanged` | Animate meter fill |
| LivesSystem | `OnLivesChanged` | Update life display |
| LivesSystem | `OnPlayerDied` | Trigger game over flow |
| ThemeSubsystem | `OnThemeChanged` | Refresh themed materials |

---

## Data-Driven Systems

### Obstacle Patterns

Obstacles spawn from predefined patterns stored in Data Tables:

```cpp
USTRUCT(BlueprintType)
struct FObstaclePatternData : public FTableRowBase
{
    UPROPERTY(EditAnywhere)
    TArray<FObstacleSpawnInfo> Obstacles;
    
    UPROPERTY(EditAnywhere)
    float PatternLength = 1000.0f;
    
    UPROPERTY(EditAnywhere)
    int32 MinDifficultyLevel = 1;
};
```

### Theme Data Assets

Color themes are defined as `UDataAsset` subclasses:

```cpp
UCLASS(BlueprintType)
class UThemeDataAsset : public UDataAsset
{
    UPROPERTY(EditAnywhere, Category = "Display")
    FText DisplayName;
    
    UPROPERTY(EditAnywhere, Category = "Colors")
    FLinearColor CircuitEmissiveColor;
    
    UPROPERTY(EditAnywhere, Category = "Colors")
    float CircuitEmissiveIntensity = 10.0f;
    
    UPROPERTY(EditAnywhere, Category = "Materials")
    TObjectPtr<UMaterialInterface> LaserPointerMaterial;
};
```

---

## Performance Considerations

### Tick Optimization

```cpp
// Most components disable tick entirely
PrimaryComponentTick.bCanEverTick = false;

// Use timers for periodic updates
GetWorld()->GetTimerManager().SetTimer(
    ScoreAccumulationTimer,
    this,
    &UScoreSystemComponent::AccumulateScore,
    ScoreAccumulationInterval,  // e.g., 0.1f
    true  // looping
);
```

### Object Pooling

Obstacles and pickups use pooling to avoid runtime allocations:

```cpp
// Acquire from pool
AObstacleBase* Obstacle = ObstaclePool.Acquire();
Obstacle->SetActorHiddenInGame(false);
Obstacle->SetActorEnableCollision(true);

// Return to pool (instead of destroying)
void UObstacleSpawnerComponent::ReturnToPool(AObstacleBase* Obstacle)
{
    Obstacle->SetActorHiddenInGame(true);
    Obstacle->SetActorEnableCollision(false);
    ObstaclePool.Return(Obstacle);
}
```

### Conditional Actor Tick

The player character only enables tick when actively moving:

```cpp
void AStateRunner_ArcadeCharacter::SwitchLaneLeft()
{
    if (CurrentLane == ELanePosition::Left) return;
    
    TargetLane = GetLaneToLeft(CurrentLane);
    bIsLaneSwitching = true;
    SetActorTickEnabled(true);  // Enable tick for smooth interpolation
}

void AStateRunner_ArcadeCharacter::ProcessLaneSwitching(float DeltaTime)
{
    // ... interpolation logic ...
    
    if (Distance <= LaneSnapThreshold)
    {
        // Snap to target, disable tick if no other movement active
        if (!IsAnyMovementActive())
        {
            SetActorTickEnabled(false);
        }
    }
}
```

---

## Blueprint Integration

### C++ to Blueprint Bridge

Core systems expose Blueprint-callable functions and events:

```cpp
// OverclockSystemComponent.h
UFUNCTION(BlueprintCallable, Category = "Overclock")
void ActivateOverclock();

UFUNCTION(BlueprintCallable, Category = "Overclock")
void DeactivateOverclock();

UFUNCTION(BlueprintPure, Category = "Overclock")
float GetMeterPercent() const;

UPROPERTY(BlueprintAssignable, Category = "Overclock")
FOnOverclockStateChanged OnOverclockStateChanged;
```

### UI Widget Communication

HUD widgets bind to component delegates in Blueprint:

```
WBP_HUD (Widget Blueprint)
├── Event Construct
│   ├── Get GameMode → Get ScoreSystemComponent
│   │   └── Bind to OnScoreChanged
│   ├── Get GameMode → Get OverclockSystemComponent
│   │   └── Bind to OnOverclockMeterChanged
│   └── Get GameMode → Get LivesSystemComponent
│       └── Bind to OnLivesChanged
```

---

## Save System

### High Score Persistence

```cpp
UCLASS()
class UHighScoreSaveGame : public USaveGame
{
    UPROPERTY()
    int32 HighScore = 0;
};
```

### Leaderboard Persistence

```cpp
UCLASS()
class ULeaderboardSaveGame : public USaveGame
{
    UPROPERTY()
    TArray<FLeaderboardEntry> Entries;
    
    static constexpr int32 MaxEntries = 10;
};

USTRUCT(BlueprintType)
struct FLeaderboardEntry
{
    UPROPERTY()
    int32 Score = 0;
    
    UPROPERTY()
    float RunTimeSeconds = 0.0f;
    
    UPROPERTY()
    FDateTime DateAchieved;
    
    UPROPERTY()
    FString PlayerInitials = TEXT("---");
};
```

### Theme Persistence

Theme preference stored via `GConfig` to `GameUserSettings.ini`:

```cpp
void UThemeSubsystem::SaveThemePreference()
{
    GConfig->SetInt(
        *ThemeConfigSection,  // "StateRunnerArcade.Theme"
        *ThemeConfigKey,      // "CurrentTheme"
        static_cast<int32>(CurrentThemeType),
        GGameUserSettingsIni
    );
    GConfig->Flush(false, GGameUserSettingsIni);
}
```
