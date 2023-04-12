using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public string sceneName;

    public InputReader inputReader;
    public AudioSource music;

    public Image fadeImage;

    float fadeTime = 3f;
    float fadeRate = 0.1f;

    private void Start() 
    {
        inputReader.ConfirmEvent +=   ChangeScene; 
    }

    void OnDestroy()
    {
        inputReader.ConfirmEvent -= ChangeScene;
    }

    public void ChangeScene()
    {
        StartCoroutine(FadeMusicAndScene());
    }

    public IEnumerator FadeMusicAndScene()
    {
        while (music.isPlaying)
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, fadeImage.color.a + (Time.deltaTime / fadeTime));
            music.volume -= Time.deltaTime / fadeTime;
            if (music.volume <= 0)
            {
                music.Stop();
            }
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }
}
