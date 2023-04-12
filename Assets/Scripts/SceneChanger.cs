using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneName;

    public InputReader inputReader;

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
        SceneManager.LoadScene(sceneName);
    }
}
