using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio; 
using System.Collections;

public class SceneFader : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] AudioMixer audioMixer; 
    [SerializeField] string musicVolumeParam = "MusicVol"; 
    [SerializeField] float fadeDuration = 1f;

    public static SceneFader Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
            return; 
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void FadeToScene(int sceneIndex)
    {
        StartCoroutine(FadeSequence(sceneIndex));
    }

    private IEnumerator FadeSequence(int sceneIndex)
    {
        yield return StartCoroutine(Fade(1f, -80f)); 

        yield return SceneManager.LoadSceneAsync(sceneIndex);

        yield return StartCoroutine(Fade(0f, 0f)); 
    }

    private IEnumerator Fade(float targetAlpha, float targetVol)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0;

        float startVol;
        audioMixer.GetFloat(musicVolumeParam, out startVol);

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float lerpTime = time / fadeDuration;

            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, lerpTime);
            
            float currentVol = Mathf.Lerp(startVol, targetVol, lerpTime);
            audioMixer.SetFloat(musicVolumeParam, currentVol);

            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
        audioMixer.SetFloat(musicVolumeParam, targetVol);
    }
}