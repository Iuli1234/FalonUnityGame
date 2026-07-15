using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class EndMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] TextMeshProUGUI finalEnemyCountText;
    [SerializeField] Button secretLevelButton;
    [SerializeField] TextMeshProUGUI secretLevelButtonContitionText;
    [SerializeField] AudioMixerSnapshot startMenuSnapshot;

    int finalScore = 0;
    int finalEnemyCount = 0;
    int playerLives = 0;

    void Awake()
    {
        UpdateUI();
        LevelUnlocked();
    }
    void Start()
    {
        startMenuSnapshot.TransitionTo(0f); 
        GameSession gs = FindFirstObjectByType<GameSession>();
        if (gs != null) 
        {
            gs.HideUI(); 
        }
        AudioSource music = gs.GetComponent<AudioSource>();
            if (music != null)
            {
                music.Stop();
            }
    }
    public void RestartGame()
    {
        GameSession gs = FindFirstObjectByType<GameSession>();
        if (gs != null) 
        {
            Destroy(gs.gameObject);
        }
        SceneFader.Instance.FadeToScene(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void SecretLevel()
    {
        GameSession gs = FindFirstObjectByType<GameSession>();
        if (gs != null) Destroy(gs.gameObject); 
        
        SceneFader.Instance.FadeToScene(6);
    }
    public void UpdateUI()
    {
        GameSession gs = FindFirstObjectByType<GameSession>();
        if (gs != null)
        {
            finalScore = gs.GetPlayerScore();
            finalEnemyCount = gs.GetEnemyCount();
            playerLives = gs.GetCurrentPlayerLives();
        }
        finalScoreText.text = finalScore.ToString();
        finalEnemyCountText.text = finalEnemyCount.ToString();
    }
    public void LevelUnlocked()
    {
        if(finalScore >= 1200 && finalEnemyCount >= 10 && playerLives > 0)
        {
            secretLevelButton.interactable = true;
            secretLevelButtonContitionText.text = "Secret Level Unlocked!";
        }
        else
        {
            secretLevelButton.interactable = false;
            secretLevelButtonContitionText.text = "Complete all levels, Score 1200 coins and Defeat 10 Enemies to Unlock Secret Level";
        }
    }
}
