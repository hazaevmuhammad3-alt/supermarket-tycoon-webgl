using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public bool isGamePaused = false;
    public float gameTime = 0f;

    private FinanceManager financeManager;
    private GridManager gridManager;
    private CustomerSpawner customerSpawner;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        financeManager = FindObjectOfType<FinanceManager>();
        gridManager = FindObjectOfType<GridManager>();
        customerSpawner = FindObjectOfType<CustomerSpawner>();

        InitializeGame();
    }

    void Update()
    {
        if (!isGamePaused)
        {
            gameTime += Time.deltaTime;
        }
    }

    void InitializeGame()
    {
        Debug.Log("Supermarket Tycoon MVP - Game Initialized!");
    }

    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }
}
