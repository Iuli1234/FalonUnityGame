using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float delay = 1f;

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(delay);

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

       
        ScenePersist scenePersist = FindFirstObjectByType<ScenePersist>();
        if (scenePersist != null)
        {
            scenePersist.ResetScenePersist();
        }
        if (nextSceneIndex == 5)
        {
            GameSession gameSession = FindFirstObjectByType<GameSession>();
            if (gameSession != null)
            {
                gameSession.HideUI();
            }
        }
        else if (nextSceneIndex == 7)
        {
           ResetFullGame(); 
           nextSceneIndex = 0;
        }
        if (SceneFader.Instance != null)
        {
            SceneFader.Instance.FadeToScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
    private void ResetFullGame()
    {
        GameSession gs = FindFirstObjectByType<GameSession>();
        if (gs != null)
        {
            AudioSource music = gs.GetComponent<AudioSource>();
            if (music != null) music.Stop();
            
            Destroy(gs.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LoadNextLevel());
        }
    }
}