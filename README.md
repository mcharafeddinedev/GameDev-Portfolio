# Game Dev Portfolio

Welcome! This repository is a **curated showcase of my game development work** — built to give 
recruiters, hiring managers, and collaborators a clear look at my coding style, problem-solving, 
and technical depth.

## 📄 Featured Documents
- **[Technologies & Techniques PDF](Docs/MC_ProjectExperiences.pdf)**  
  A detailed breakdown of the programming languages, engines, tools, and workflows I’ve applied 
  across past projects.  

- **[Ginger Shroom Journey: C# Deep Dive](Docs/GSJ_CSharp_Analysis.pdf)**  
  An in-depth analysis of my own published project, focusing on gameplay scripting and 
  architecture.  

## 💻 Code Excerpts
Below are a few representative code samples from **Ginger Shroom Journey**.  
Each example highlights a different gameplay system and includes an explanation.  
(For deeper dives, see the [Docs](Docs) folder.)

---

### 🕹️ Game Manager – Centralized State & Scene Handling
```csharp
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private bool ironManMode = false;
    private int currentLevel = 1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
    }
}
```

Highlights:

    Implements the Singleton Pattern for centralized control across scenes.

    Uses DontDestroyOnLoad to persist through level transitions.

    Acts as a hub for connecting pause, scoring, and UI systems.

⏸️ Pause Manager – Toggling Time & UI
```csharp
public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
    }
}
```
Highlights:

    Uses Time.timeScale to freeze/resume gameplay.

    Dynamically toggles UI elements with SetActive().

    Simple design that can be extended with state machines or events.

🧑‍🚀 Player Controller – Physics-Based Character Control
```csharp
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public float runSpeed = 6f;
    public float jumpSpeed = 5f;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }
}
```
Highlights:

    Demonstrates physics-driven movement using Rigidbody2D.velocity.

    Integrates Unity’s input system with animation control.

    Foundation for more advanced movement features (coyote time, double-jump).

🐌 Slime AI – Enemy Patrol Logic
```csharp
public class SlimeController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    void Update()
    {
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        bool isBlocked = Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);

        if (!isGrounded || isBlocked)
        {
            Flip();
        }
    }

    void Flip()
    {
        moveSpeed = -moveSpeed;
        transform.localScale = new Vector3(
            -transform.localScale.x,
            transform.localScale.y,
            transform.localScale.z
        );
    }
}
```
Highlights:

    Uses physics checks to detect walls/edges.

    Implements a clean Flip() mechanic for enemy patrol AI.

    Easily extensible with player detection or aggressive states.

## 🚀 Projects
Links to project highlights and demos:  
- **Ginger Shroom Journey** (Steam) — [https://store.steampowered.com/app/3023100/Ginger_Shroom_Journey/]  
- **Prototypes & More** — various experiments and demos (Itch.io) - [https://goldleafinteractive.itch.io/]

---

[![Alt text](Docs/assets/revisedLogoForGitHubPages.png)](https://goldleafinteractive.itch.io)

---
  
---

## 👋 About Me
- A.A.S. in Digital Gaming & Simulation for Programmers (Houston Community College)  
- Founder of **Gold Leaf Interactive**
- Focused on growing in gameplay programming, systems design, and UX

