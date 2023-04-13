using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class CharacterCreator : MonoBehaviour
{
    public GameObject HeadBone;
    public List<GameObject> HairPrefabs = new List<GameObject>();
    public Material HairMaterial;
    GameObject currentHair;
    public List<GameObject> EarPrefabs = new List<GameObject>();
    GameObject currentEars;
    public List<Texture2D> EyeTextures = new List<Texture2D>();
    public Material EyeMaterial;
    public List<GameObject> NosePrefabs = new List<GameObject>();
    public Material NoseMaterial;
    public List<Texture2D> MouthTextures = new List<Texture2D>();
    public Material MouthMaterial;
    public Material SkinMaterial;

    private int hairIndex = 0;
    private int earIndex = 0;

    public GameObject CCUI;
    public InputReader inputReader;
    public GameObject HUD;
    public PlayableAsset playable;
    public PlayableDirector playableDirector;
    public CinemachineVirtualCamera createCam;

    private void Start() 
    {
        currentHair = Instantiate(HairPrefabs[hairIndex], HeadBone.transform); 
        currentEars = Instantiate(EarPrefabs[earIndex], HeadBone.transform);  
    }

    public void IncreaseHairIndex()
    {
        hairIndex = IncreaseIndex(hairIndex, HairPrefabs);
        Destroy(currentHair);
        currentHair = Instantiate(HairPrefabs[hairIndex], HeadBone.transform);
    }

    public void DecreaseHairIndex()
    {
        hairIndex = DecreaseIndex(hairIndex, HairPrefabs);
        Destroy(currentHair);
        currentHair = Instantiate(HairPrefabs[hairIndex], HeadBone.transform);
    }

    public void IncreaseEarIndex()
    {
        earIndex = IncreaseIndex(earIndex, EarPrefabs);
        Destroy(currentEars);
        currentEars = Instantiate(EarPrefabs[earIndex], HeadBone.transform);
    }

    public void DecreaseEarIndex()
    {
        earIndex = DecreaseIndex(earIndex, EarPrefabs);
        Destroy(currentEars);
        currentEars = Instantiate(EarPrefabs[earIndex], HeadBone.transform);
    }

    public int IncreaseIndex(int index, List<GameObject> bpList)
    {
        int i = index;
        i++;
        if (i >= bpList.Count) i = 0;
        return i;
    }

    public int DecreaseIndex(int index, List<GameObject> bpList)
    {
        int i = index;
        i--;
        if (i <= 0) i = bpList.Count - 1;
        return i;
    }


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
