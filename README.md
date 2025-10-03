# Marwan Charafeddine - Game Developer Portfolio

[![Unity](https://img.shields.io/badge/Engine-Unity-informational?logo=unity)](https://unity.com/)
[![Unreal](https://img.shields.io/badge/Engine-Unreal-informational?logo=unreal-engine)](https://www.unrealengine.com/)
[![C%23](https://img.shields.io/badge/Language-C%23-informational?logo=c-sharp)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![C%2B%2B](https://img.shields.io/badge/Language-C%2B%2B-informational?logo=c%2B%2B)](https://isocpp.org/)

This is my game development portfolio. I work with Unity and Unreal Engine, focusing mainly on gameplay programming and building game systems. The code samples and documentation here are from projects I've worked on, including my published game.

I got into game development a few years after finishing my veterinary studies. The problem-solving approach from my science background has been really helpful for debugging and building complex game systems, understanding new ideas and techniques, and approaching challenges systematically.

## Technical Skills

**Programming Languages:** C#, C++, Blueprint Visual Scripting  
**Game Engines:** Unity 6, Unity 2022+, Unreal Engine 5  
**What I Work On:** Gameplay Programming, Systems Design, Physics Programming, Procedural Generation, Event Systems  
**Advanced Stuff:** Rope Physics, Object Management, Memory Management, Performance Optimization, Modular Code  
**Tools:** Visual Studio, Rider, Git, Perforce, Unity Input System, Unreal Blueprint Editor  
**Platforms:** PC (Windows), Steam, Itch.io, Game Jams  

## Documentation

- **[Documentation Overview](Docs/README.md)**  
  Guide to all the materials and code samples in this portfolio.

- **[Quantum Tether: Complete Documentation](Docs/QuantumTether/)**  
  Documentation, source code, and analysis of my game jam project.

- **[Project Experiences & Technical Overview](Docs/MC_ProjectExperiences.pdf)**  
  Notes on the tech stack and tools I've used across different projects.

- **[Ginger Shroom Journey: Code Architecture Analysis](Docs/GSJ_CSharp_Analysis.pdf)**  
  Deep dive into the code architecture and systems I built for my published game.

- **[Development Process Videos](Docs/VideoLinks.pdf)**  
  Some development process recordings and demos.

- **[Complete Source Code](Docs/GSJ_Scripts/)**  
  All the C# source code from Ginger Shroom Journey, organized by system.

## Featured Projects

### Quantum Tether (TX Game Jam 2025)
An infinite 2D side-scrolling grappling hook game I built in Unity 6 for TX Game Jam. Players swing between grapple points using rope physics while the camera scrolls faster over time to make it harder. Has procedural generation, upgrade system, and audio integration.

**What I Built:**
- Event system so different parts of the game can talk to each other without being directly connected
- Grappling hook physics using DistanceJoint2D and LineRenderer
- Procedural generation with 10+ different anchor patterns
- Upgrade system where you pick improvements after dying
- Object cleanup system to prevent memory issues
- Modular design where each script does one thing

**Development:**
- Built in ~52 hours for TX Game Jam 2024 (theme: "Out of Time")
- Spent several days after the jam adding more features, cleaning up and documenting everything
- Made tutorial guides for all the game & mechanics systems

### Ginger Shroom Journey (Published on Steam)
A 2D platformer with movement mechanics, enemy AI, and level elements. The game shows my work on game systems, code architecture, and more.

**What I Built:**
- 2D physics-based movement system
- Enemy AI that patrols and detects obstacles
- Game state management system
- Started learning about Steam SDK integration for achievements and cloud saves
- Performance optimization to maintain 60fps

### Additional Projects
Smaller projects showing different game development techniques, including 3D level design analysis and some experimental work.

## Code Samples
Here are some code snippets from my projects. These show how I approached different game systems. The code includes error handling, documentation, and performance considerations.

*Note: These are simplified versions for readability. The actual implementation includes 
additional error handling and edge cases.*

### Quantum Tether - Grappling Hook Physics
```csharp
public class EnhancedPlayerSwing : MonoBehaviour
{
    [SerializeField] private float maxRopeLength = 8f;
    [SerializeField] private LayerMask grappleLayer = 1;
    [SerializeField] private bool autoContractOnConnect = true;
    [SerializeField] private float autoContractAmount = 0.2f;
    
    private DistanceJoint2D primaryJoint;
    private LineRenderer rope;
    private bool isGrappling = false;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            TryGrapple(mousePos);
        }
        
        if (Input.GetKey(KeyCode.Space) && isGrappling)
        {
            ContractRope();
        }
    }
    
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
    
    void CreateGrappleJoint(Vector2 anchorPoint)
    {
        primaryJoint = gameObject.AddComponent<DistanceJoint2D>();
        primaryJoint.connectedAnchor = anchorPoint;
        primaryJoint.distance = Vector2.Distance(transform.position, anchorPoint);
        
        isGrappling = true;
        
        if (autoContractOnConnect)
        {
            primaryJoint.distance *= (1f - autoContractAmount);
        }
    }
}
```

The core grappling hook mechanics - raycast to mouse position, create physics joint, auto-contract for that satisfying "yoink" effect.

### Quantum Tether - Dash Movement
```csharp
public class EnhancedPlayerDash : MonoBehaviour
{
    [SerializeField] private float dashForce = 15f;
    [SerializeField] private float dashCooldown = 2f;
    [SerializeField] private float dashDuration = 0.2f;
    
    private Rigidbody2D rb;
    private bool canDash = true;
    private bool isDashing = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartDash();
        }
    }
    
    void StartDash()
    {
        isDashing = true;
        canDash = false;
        
        Vector2 dashDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        rb.velocity = dashDirection * dashForce;
        
        StartCoroutine(DashCooldown());
    }
    
    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
```

Quick burst movement toward the mouse cursor with cooldown system. Adds a lot of mobility to the grappling gameplay.

### Quantum Tether - Procedural Generation Patterns
```csharp
public class EnhancedSpawner : MonoBehaviour
{
    void SpawnFibonacciSpiralAt(Vector3 basePosition)
    {
        // Create a Fibonacci spiral approximation using golden ratio
        float goldenRatio = 1.618f;
        int pointCount = Random.Range(10, 16);
        float scale = Random.Range(0.5f, 1.0f);
        
        for (int i = 0; i < pointCount; i++)
        {
            // Approximate Fibonacci spiral using golden ratio
            float angle = i * goldenRatio * 0.5f;
            float radius = Mathf.Pow(goldenRatio, i * 0.1f) * scale;
            
            // Convert polar to cartesian
            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);
            
            Vector3 pos = basePosition + Vector3.right * x + Vector3.up * y;
            SpawnSingleAnchorAt(pos);
        }
    }
    
    void SpawnWavePatternAt(Vector3 basePosition)
    {
        // Create a complex wave pattern combining multiple sine waves
        float amplitude1 = Random.Range(1.5f, 3f);
        float frequency1 = Random.Range(0.3f, 0.8f);
        float amplitude2 = Random.Range(0.5f, 1.5f);
        float frequency2 = Random.Range(1.0f, 2.0f);
        float spacing = Random.Range(0.6f, 1.0f);
        int pointCount = Random.Range(10, 18);
        
        for (int i = 0; i < pointCount; i++)
        {
            float x = i * spacing;
            // Combine two sine waves for a more complex pattern
            float y = (Mathf.Sin(x * frequency1) * amplitude1) + (Mathf.Sin(x * frequency2) * amplitude2);
            Vector3 pos = basePosition + Vector3.right * x + Vector3.up * y;
            SpawnSingleAnchorAt(pos);
        }
    }
}
```

Procedural generation using mathematical patterns - Fibonacci spirals with golden ratio and complex wave patterns combining multiple sine waves. Creates varied, but structured and interesting grapple point arrangements.

### Ginger Shroom Journey - Arrow Shooting
```csharp
public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        Destroy(gameObject, lifetime);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
```

Simple projectile system - arrows fly forward, destroy enemies on hit, disappear after hitting ground or timing out.

### Ginger Shroom Journey - Coin Collection
```csharp
public class CoinScript : MonoBehaviour
{
    public int coinValue = 1;
    private ScoreManager scoreManager;
    private bool isCollected = false;
    public AudioSource collectSound;
    
    void Start()
    {
        scoreManager = ScoreManager.Instance;
        collectSound = GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCollected && collision.gameObject.CompareTag("Player"))
        {
            CollectCoin();
        }
    }
    
    void CollectCoin()
    {
        isCollected = true;
        
        if (scoreManager != null)
        {
            scoreManager.AddScore(coinValue);
        }
        
        if (collectSound != null)
        {
            collectSound.PlayOneShot(collectSound.clip);
        }
        
        Destroy(gameObject, 0.25f);
    }
}
```

Collectible coins that add to score, play sound effects, and have a small delay before disappearing for visual feedback.

---

[![Alt text](Docs/assets/revisedLogoForGitHubPages.png)](https://goldleafinteractive.itch.io)

---
  
---

## About Me
- B.S. in Veterinary Studies from Texas A&M University at College Station
- A.A.S. in Digital Gaming & Simulation for Programmers from Houston Community College
- Currently working on gameplay programming and systems design

---

## Contact  

[![LinkedIn](https://img.shields.io/badge/LinkedIn-Profile-blue?logo=linkedin)](https://www.linkedin.com/in/marwan-charafeddine-213065155) 
[![Email](https://img.shields.io/badge/Email-Contact%20Me-red?logo=gmail)](mailto:mcharafeddinedev@gmail.com) 
[![Itch.io](https://img.shields.io/badge/Itch.io-Portfolio-critical?logo=itch.io)](https://goldleafinteractive.itch.io/) 
[![Steam](https://img.shields.io/badge/Steam-Projects-lightgrey?logo=steam)](https://store.steampowered.com/app/3023100/Ginger_Shroom_Journey/) 
[![GitHub](https://img.shields.io/badge/GitHub-Portfolio-black?logo=github)](https://github.com/mcharafeddinedev)


