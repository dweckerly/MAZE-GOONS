using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CharacterCreator : MonoBehaviour
{
    public GameObject CCUI;
    public InputReader inputReader;
    public GameObject HUD;
    public PlayableAsset playable;
    public PlayableDirector playableDirector;

    public void FinishCharacterCreation()
    {
        CCUI.SetActive(false);
        playableDirector.Play(playable);
        StartCoroutine(CameraPan());
    }

    IEnumerator CameraPan()
    {
        while(playableDirector.state == PlayState.Playing)
        {
            yield return null;
        }
        OnCameraFinished();
    }

    private void OnCameraFinished()
    {
        print("CameraFinished triggered...");
        inputReader.enabled = true;
        HUD.SetActive(true);
    }
}
