# Code Samples — OVERCLOCKED: Data Dash MAX

Representative C++ implementations demonstrating the core systems and architectural patterns.

---

## Lane Switching (Smooth Y-Axis Interpolation)

The player moves between three lanes using smooth interpolation along the Y-axis.

```cpp
void AStateRunner_ArcadeCharacter::ProcessLaneSwitching(float DeltaTime)
{
    const FVector CurrentLocation = GetActorLocation();
    const float CurrentY = CurrentLocation.Y;
    const float TargetY = GetLaneYPosition(TargetLane);
    const float Distance = FMath::Abs(TargetY - CurrentY);

    // Handle Z position based on current state
    float TargetZ;
    if (bIsJumping)
    {
        TargetZ = BaseZPosition + CurrentJumpOffset;
    }
    else if (bIsSliding)
    {
        TargetZ = CurrentLocation.Z;  // Maintain slide height
    }
    else
    {
        TargetZ = BaseZPosition;
    }

    // Check if we've arrived at target lane
    if (Distance <= LaneSnapThreshold)
    {
        // Snap to exact position
        SetActorLocation(FVector(LockedXPosition, TargetY, TargetZ));
        CurrentLane = TargetLane;
        bIsLaneSwitching = false;
        
        // Support held-input queuing for rapid lane changes
        if ((bIsLaneLeftHeld && CurrentLane != ELanePosition::Left) ||
            (bIsLaneRightHeld && CurrentLane != ELanePosition::Right))
        {
            if (bIsLaneLeftHeld)
                SwitchLaneLeft();
            else
                SwitchLaneRight();
            return;
        }
        
        // Disable tick if no movement is active (performance optimization)
        if (!IsAnyMovementActive())
        {
            SetActorTickEnabled(false);
        }
        return;
    }

    // Smooth interpolation toward target
    const float Direction = FMath::Sign(TargetY - CurrentY);
    const float Movement = LaneSwitchSpeed * DeltaTime * Direction;
    
    // Clamp to prevent overshooting
    float NewY = (Direction > 0) 
        ? FMath::Min(CurrentY + Movement, TargetY) 
        : FMath::Max(CurrentY + Movement, TargetY);
    
    SetActorLocation(FVector(LockedXPosition, NewY, TargetZ));
}
```

**Key Points:**
- Y-axis interpolation for lane switching (non-standard coordinate system)
- Snap threshold prevents jitter at destination
- Held-input queuing for responsive double-lane switches
- Conditional tick disabling for performance

---

## OVERCLOCK System (Risk/Reward Speed Boost)

The OVERCLOCK system manages a meter that fills passively and activates on player input for speed/score bonuses.

```cpp
void UOverclockSystemComponent::ActivateOverclock()
{
    if (bIsOverclockActive) return;
    
    bIsOverclockActive = true;

    // Apply speed multiplier to world scroll (1.5x default)
    if (WorldScrollComponent)
    {
        WorldScrollComponent->SetOverclockMultiplier(SpeedMultiplier, true);
    }

    // Activate bonus scoring (+200 pts/sec default)
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

    // Broadcast state change for UI
    OnOverclockStateChanged.Broadcast(true);

    UE_LOG(LogStateRunner_Arcade, Log, 
        TEXT("OVERCLOCK ACTIVATED! Speed: %.1fx, Bonus: +%d pts/sec"), 
        SpeedMultiplier, OverclockBonusRate);
}

void UOverclockSystemComponent::DeactivateOverclock()
{
    if (!bIsOverclockActive) return;
    
    bIsOverclockActive = false;

    // Remove speed multiplier
    if (WorldScrollComponent)
    {
        WorldScrollComponent->SetOverclockMultiplier(1.0f, false);
    }

    // Deactivate bonus scoring
    if (ScoreSystemComponent)
    {
        ScoreSystemComponent->SetOverclockActive(false);
    }

    // Reset camera zoom
    if (AStateRunner_ArcadeCharacter* Character = GetPlayerCharacter())
    {
        Character->SetOverclockZoom(false);
    }

    // Stop audio
    StopOverclockLoop();
    PlayOverclockSound(OverclockDeactivateSound);

    // Start cooldown
    bIsOnCooldown = true;
    GetWorld()->GetTimerManager().SetTimer(
        CooldownTimer, this,
        &UOverclockSystemComponent::OnCooldownComplete,
        CooldownDuration, false
    );

    OnOverclockStateChanged.Broadcast(false);
}
```

**Key Points:**
- Coordinates with WorldScroll, ScoreSystem, and Character
- Event-driven UI updates via delegate broadcast
- Audio/visual feedback for activation state
- Cooldown system to prevent spam

---

## World Scroll with Damage Slowdown

The scroll system handles base speed, OVERCLOCK multipliers, and damage feedback.

```cpp
void UWorldScrollComponent::ApplyDamageSlowdown()
{
    bIsDamageSlowdownActive = true;
    DamageSlowdownTimeRemaining = DamageSlowdownDuration;
    
    UE_LOG(LogStateRunner_Arcade, Log, 
        TEXT("WorldScrollComponent: Damage slowdown APPLIED (%.2f%% speed for %.2f sec)"), 
        DamageSlowdownMultiplier * 100.0f, DamageSlowdownDuration);
}

float UWorldScrollComponent::GetCurrentScrollSpeed() const
{
    if (!bIsScrolling) return 0.0f;
    
    // Start with base speed (which ramps over time)
    float FinalSpeed = CurrentScrollSpeed;
    
    // Apply OVERCLOCK multiplier (1.5x when active)
    if (bIsOverclockActive)
    {
        FinalSpeed *= OverclockMultiplier;
    }
    
    // Apply damage slowdown (0.7x for impact feel)
    if (bIsDamageSlowdownActive)
    {
        FinalSpeed *= DamageSlowdownMultiplier;
    }
    
    return FinalSpeed;
}

void UWorldScrollComponent::UpdateScrollSpeed()
{
    if (!bIsScrolling) return;
    
    // Handle damage slowdown timer
    if (bIsDamageSlowdownActive)
    {
        DamageSlowdownTimeRemaining -= GetWorld()->GetDeltaSeconds();
        if (DamageSlowdownTimeRemaining <= 0.0f)
        {
            bIsDamageSlowdownActive = false;
            UE_LOG(LogStateRunner_Arcade, Log, TEXT("Damage slowdown ENDED"));
        }
    }
    
    // Ramp base speed over time
    float TargetSpeed = BaseScrollSpeed + (SpeedIncreasePerSecond * TimeElapsed);
    TargetSpeed = FMath::Min(TargetSpeed, MaxScrollSpeed);
    
    // Smooth interpolation to target
    CurrentScrollSpeed = FMath::FInterpTo(
        CurrentScrollSpeed, TargetSpeed,
        GetWorld()->GetDeltaSeconds(), SpeedInterpRate
    );
}
```

**Key Points:**
- Speed multipliers stack (OVERCLOCK × damage slowdown)
- Damage slowdown provides visceral hit feedback
- Speed ramps smoothly over time with a cap
- Clean separation of speed calculation from application

---

## Theme System (Data-Driven Color Schemes)

The ThemeSubsystem manages color themes via Data Assets and dynamic material instances.

```cpp
void UThemeSubsystem::SetCurrentTheme(EThemeType NewTheme, bool bApplyImmediately)
{
    if (CurrentThemeType == NewTheme) return;  // No change needed

    EThemeType OldTheme = CurrentThemeType;
    CurrentThemeType = NewTheme;

    // Persist preference
    SaveThemePreference();

    // Broadcast for UI/systems that care about theme changes
    OnThemeChanged.Broadcast(NewTheme);

    UE_LOG(LogStateRunner_Arcade, Log, 
        TEXT("ThemeSubsystem: Theme changed from %d to %d"), 
        static_cast<int32>(OldTheme), static_cast<int32>(NewTheme));

    // Apply to all registered meshes if requested
    if (bApplyImmediately)
    {
        RefreshAllThemedMeshes();
    }
}

void UThemeSubsystem::ApplyCircuitPatternTheme(
    UStaticMeshComponent* MeshComponent, 
    const UThemeDataAsset* ThemeData)
{
    if (!MeshComponent || !ThemeData) return;

    // Calculate final emissive color with intensity
    FLinearColor FinalEmissiveColor = 
        ThemeData->CircuitEmissiveColor * ThemeData->CircuitEmissiveIntensity;

    // Apply to circuit pattern material elements (indices 0-255)
    for (int32 ElementIndex = CIRCUIT_PATTERN_START; 
         ElementIndex <= CIRCUIT_PATTERN_END; 
         ++ElementIndex)
    {
        UMaterialInterface* BaseMaterial = MeshComponent->GetMaterial(ElementIndex);
        if (!BaseMaterial) continue;

        // Create or reuse dynamic material instance
        UMaterialInstanceDynamic* DynamicMat = 
            Cast<UMaterialInstanceDynamic>(BaseMaterial);
        if (!DynamicMat)
        {
            DynamicMat = MeshComponent->CreateDynamicMaterialInstance(
                ElementIndex, BaseMaterial);
        }

        if (DynamicMat)
        {
            // Set emissive color parameter (try common naming conventions)
            DynamicMat->SetVectorParameterValue(
                FName("EmissiveColor"), FinalEmissiveColor);
            DynamicMat->SetVectorParameterValue(
                FName("Emissive Color"), FinalEmissiveColor);
            DynamicMat->SetVectorParameterValue(
                FName("EmissiveTint"), FinalEmissiveColor);
        }
    }
}
```

**Key Points:**
- Data Assets define theme colors and materials
- Dynamic material instances allow runtime color changes
- Mesh registration pattern for batch updates
- Preference persistence via config files

---

## Combo Scoring System

Tracks rapid pickup collection for bonus scoring with NICE (6x) and INSANE (10x) combos.

```cpp
void UScoreSystemComponent::AddPickupBonus()
{
    AddScore(PickupScoreValue);
    DataPacketsCollected++;
    
    // Track pickup timestamps for combo detection
    float CurrentTime = GetWorld()->GetTimeSeconds();
    RecentDataPacketTimestamps.Add(CurrentTime);
    
    // Clean up timestamps outside INSANE window (longest)
    float InsaneWindowStart = CurrentTime - InsaneComboTimeWindow;
    RecentDataPacketTimestamps.RemoveAll([InsaneWindowStart](float Timestamp)
    {
        return Timestamp < InsaneWindowStart;
    });
    
    // Count pickups in each time window
    int32 InsaneWindowCount = RecentDataPacketTimestamps.Num();
    
    // NICE window is shorter — count separately
    float NiceWindowStart = CurrentTime - NiceComboTimeWindow;
    int32 NiceWindowCount = 0;
    for (float Timestamp : RecentDataPacketTimestamps)
    {
        if (Timestamp >= NiceWindowStart)
        {
            NiceWindowCount++;
        }
    }
    
    // Check for INSANE! combo (10x) — highest priority
    if (InsaneWindowCount >= InsaneComboRequiredPickups)
    {
        // Cancel pending NICE popup — INSANE takes priority
        if (bNiceComboDelayPending)
        {
            GetWorld()->GetTimerManager().ClearTimer(NiceComboDelayTimer);
            bNiceComboDelayPending = false;
        }
        
        AddScore(InsaneComboBonusValue);
        OnInsaneCombo.Broadcast(InsaneComboBonusValue);
        
        // Clear timestamps to require fresh combo streak
        RecentDataPacketTimestamps.Empty();
        bNiceComboAwarded = false;
    }
    // Check for NICE! combo (6x)
    else if (NiceWindowCount >= NiceComboRequiredPickups && !bNiceComboAwarded)
    {
        AddScore(NiceComboBonusValue);
        bNiceComboAwarded = true;
        
        // Delay popup in case INSANE is coming
        bNiceComboDelayPending = true;
        PendingNiceComboBonusValue = NiceComboBonusValue;
        
        GetWorld()->GetTimerManager().SetTimer(
            NiceComboDelayTimer, this,
            &UScoreSystemComponent::OnNiceComboDelayExpired,
            ComboPopupDelaySeconds, false
        );
    }
}
```

**Key Points:**
- Sliding time window for combo detection
- NICE and INSANE have separate time windows
- INSANE cancels pending NICE popup (priority system)
- Score awarded immediately, popup may be delayed

---

## Leaderboard with Initials Entry

Local leaderboard stores top 10 scores with arcade-style 3-character initials.

```cpp
USTRUCT(BlueprintType)
struct FLeaderboardEntry
{
    GENERATED_BODY()

    UPROPERTY(BlueprintReadOnly, Category="Leaderboard")
    int32 Score = 0;

    UPROPERTY(BlueprintReadOnly, Category="Leaderboard")
    float RunTimeSeconds = 0.0f;

    UPROPERTY(BlueprintReadOnly, Category="Leaderboard")
    FDateTime DateAchieved;

    UPROPERTY(BlueprintReadOnly, Category="Leaderboard")
    FString PlayerInitials = TEXT("---");

    /** Constructor with initials */
    FLeaderboardEntry(int32 InScore, float InRunTime, FDateTime InDate, 
                      const FString& InInitials)
        : Score(InScore)
        , RunTimeSeconds(InRunTime)
        , DateAchieved(InDate)
        , PlayerInitials(InInitials.Left(3).ToUpper())  // Enforce 3 chars
    {
    }

    /** Get formatted run time (MM:SS or HH:MM:SS) */
    FString GetFormattedRunTime() const
    {
        int32 TotalSeconds = FMath::FloorToInt(RunTimeSeconds);
        int32 Hours = TotalSeconds / 3600;
        int32 Minutes = (TotalSeconds % 3600) / 60;
        int32 Seconds = TotalSeconds % 60;

        if (Hours > 0)
        {
            return FString::Printf(TEXT("%d:%02d:%02d"), Hours, Minutes, Seconds);
        }
        return FString::Printf(TEXT("%d:%02d"), Minutes, Seconds);
    }
};

int32 UScoreSystemComponent::SubmitToLeaderboardWithInitials(const FString& Initials)
{
    FLeaderboardEntry NewEntry(CurrentScore, TimeElapsed, FDateTime::Now(), Initials);
    
    if (!CachedLeaderboard)
    {
        LoadLeaderboard();
    }
    
    if (CachedLeaderboard)
    {
        // AddEntry returns rank (1-10) if qualified, 0 if not
        LeaderboardRankThisRun = CachedLeaderboard->AddEntry(NewEntry);
        
        if (LeaderboardRankThisRun > 0)
        {
            SaveLeaderboard();
            
            UE_LOG(LogStateRunner_Arcade, Log, 
                TEXT("LEADERBOARD #%d! %s - Score: %d, Time: %s"), 
                LeaderboardRankThisRun, *NewEntry.PlayerInitials, 
                CurrentScore, *NewEntry.GetFormattedRunTime());
        }
    }
    
    return LeaderboardRankThisRun;
}
```

**Key Points:**
- Struct contains score, run time, date, and initials
- Initials automatically truncated and uppercased
- Leaderboard sorted by score, trimmed to 10 entries
- USaveGame-based persistence
