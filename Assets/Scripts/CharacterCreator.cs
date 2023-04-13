using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class CharacterCreator : MonoBehaviour
{
    public GameObject CCUI;
    public InputReader inputReader;
    public GameObject HUD;
    public PlayableAsset playable;
    public PlayableDirector playableDirector;
    public CinemachineVirtualCamera createCam;

    public void FinishCharacterCreation()
    {
        CCUI.SetActive(false);
        playableDirector.Play(playable);
        StartCoroutine(CameraPan());
    }

    IEnumerator CameraPan()
    {
        createCam.Priority = 0;
        while(playableDirector.state == PlayState.Playing)
        {
            yield return null;
        }
        OnCameraFinished();
    }

    private void OnCameraFinished()
    {
        inputReader.enabled = true;
        HUD.SetActive(true);
    }
}
