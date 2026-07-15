using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameSession : MonoBehaviour
{
    [SerializeField] int maxPlayerLives = 5;
    [SerializeField] int currentPlayerLives = 5;
    [SerializeField] int playerScore = 0;
    [SerializeField] int enemyCount = 0;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI enemyCountText;
    [SerializeField] Sprite emptyHeart;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Image[] hearts;

    [Header("Audio Settings")]
    [SerializeField] AudioMixerSnapshot normalSnapshot; 
    [SerializeField] float transitionOut = 1.5f;  
    public int GetCurrentPlayerLives()
    {
        return currentPlayerLives;
    }   
    void Awake()
    {
        int numberGameSessions = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;
        if(numberGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        UpdateUI();
    }
    void UpdateUI()
    {
        ResetHeartsVisuals();
        scoreText.text = playerScore.ToString();
        enemyCountText.text = enemyCount.ToString();
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
        var container = GameObject.Find("HeartBar");
        if (container != null)
        {
            hearts = container.GetComponentsInChildren<Image>();
        }
        AudioSource myAudio = GetComponent<AudioSource>();
        if (myAudio != null)
        {
            if (scene.buildIndex == 0 || scene.buildIndex == 5)
            {
                myAudio.Stop();
            }
            else 
            {
                if (!myAudio.isPlaying) myAudio.Play();
            }
        }
        if (normalSnapshot != null)
        {
            normalSnapshot.TransitionTo(2.0f); 
        }
        UpdateUI();
    }
    public void ResetHeartsVisuals()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            if(i < currentPlayerLives)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
            if(i < maxPlayerLives)
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;
        }
    }
    public void ProcessPlayerDeath()
    {
        if(currentPlayerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }
    void TakeLife()
    {
        currentPlayerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        Invoke("ResetAudioToNormal", 1.5f); 
    }
    void ResetAudioToNormal()
    {
        if (normalSnapshot != null)
        {
            normalSnapshot.TransitionTo(transitionOut);
        }
    }
    public void AddToScore(int points)
    {
        playerScore += points;
        scoreText.text = playerScore.ToString();
    }
    public void AddToEnemyCount(int points)
    {
        enemyCount += points;
        enemyCountText.text = enemyCount.ToString();
    }
    public void AddLife(int points)
    {
        if(currentPlayerLives < maxPlayerLives)
        {
            currentPlayerLives += points;
            UpdateUI();
        }
    }
    void ResetGameSession()
    {
        FindFirstObjectByType<ScenePersist>().ResetScenePersist();
        HideUI();
        SceneManager.LoadScene(5);
    }
    public int GetPlayerScore()
    {
        return playerScore;
    }
    public int GetEnemyCount()
    {
        return enemyCount;
    }
    public void HideUI()
    {
        GetComponentInChildren<Canvas>().enabled = false;
    }
}
