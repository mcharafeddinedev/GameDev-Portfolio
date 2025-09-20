using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText; // Reference to the UI Text element for displaying the score
    public Text levelText; // Reference to the UI Text element for displaying the level

    private int score = 0;

    private static ScoreManager instance;

    public static ScoreManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        // Initialize the UI texts
        if (scoreText == null)
        {
            Debug.LogError("Score Text not assigned to ScoreManager!");
        }

        if (levelText == null)
        {
            Debug.LogError("Level Text not assigned to ScoreManager!");
        }

        UpdateScoreText();
        UpdateLevelText();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateLevelText();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "COINS: " + score.ToString();
        }
    }

    void UpdateLevelText()
    {
        if (levelText != null)
        {
            // Display the current scene index as the level number
            levelText.text = "LVL " + SceneManager.GetActiveScene().buildIndex.ToString();
        }
    }
}
