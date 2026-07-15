using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] AudioMixerSnapshot startMenuSnapshot;
    [SerializeField] Button startButton;
    [SerializeField] Button quitButton;

    void Start()
    {
        startMenuSnapshot.TransitionTo(0f);
    }
    public void StartGame()
    {
        SceneFader.Instance.FadeToScene(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
