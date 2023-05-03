using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class CharacterCreator : MonoBehaviour
{
    public GameObject Player;
    public TMP_InputField inputField;
    public GameObject nameWarning;
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
    GameObject currentNose;
    public List<Texture2D> MouthTextures = new List<Texture2D>();
    public Material MouthMaterial;
    public Material SkinMaterial;

    private int hairIndex = 0;
    private int earIndex = 0;
    private int eyeIndex = 0;
    private int noseIndex = 0;
    private int mouthIndex = 0;

    public GameObject CCUI;
    public InputReader inputReader;
    public GameObject HUD;
    public PlayableAsset playable;
    public PlayableDirector playableDirector;
    public CinemachineVirtualCamera createCam;

    public StatsButtons statsButtons;

    private void Start() 
    {
        currentHair = Instantiate(HairPrefabs[hairIndex], HeadBone.transform); 
        currentEars = Instantiate(EarPrefabs[earIndex], HeadBone.transform);
        EyeMaterial.mainTexture = EyeTextures[eyeIndex];
        currentNose = Instantiate(NosePrefabs[noseIndex], HeadBone.transform);
        MouthMaterial.mainTexture = MouthTextures[mouthIndex];
    }

    public void IncreaseHairIndex()
    {
        hairIndex = IncreaseIndex(hairIndex, HairPrefabs.Count);
        if (currentHair != null) Destroy(currentHair);
        if (HairPrefabs[hairIndex] != null)
            currentHair = Instantiate(HairPrefabs[hairIndex], HeadBone.transform);
        else
            currentHair = null;
    }

    public void DecreaseHairIndex()
    {
        hairIndex = DecreaseIndex(hairIndex, HairPrefabs.Count);
        if (currentHair != null) Destroy(currentHair);
        if (HairPrefabs[hairIndex] != null)
            currentHair = Instantiate(HairPrefabs[hairIndex], HeadBone.transform);
        else
            currentHair = null;
    }

    public void IncreaseEarIndex()
    {
        earIndex = IncreaseIndex(earIndex, EarPrefabs.Count);
        Destroy(currentEars);
        currentEars = Instantiate(EarPrefabs[earIndex], HeadBone.transform);
    }

    public void DecreaseEarIndex()
    {
        earIndex = DecreaseIndex(earIndex, EarPrefabs.Count);
        Destroy(currentEars);
        currentEars = Instantiate(EarPrefabs[earIndex], HeadBone.transform);
    }

    public void IncreaseEyeIndex()
    {
        eyeIndex = IncreaseIndex(eyeIndex, EyeTextures.Count);
        EyeMaterial.mainTexture = EyeTextures[eyeIndex];
    }

    public void DecreaseEyeIndex()
    {
        eyeIndex = DecreaseIndex(eyeIndex, EyeTextures.Count);
        EyeMaterial.mainTexture = EyeTextures[eyeIndex];
    }

    public void IncreaseMouthIndex()
    {
        mouthIndex = IncreaseIndex(mouthIndex, MouthTextures.Count);
        MouthMaterial.mainTexture = MouthTextures[mouthIndex];
    }

    public void DecreaseMouthIndex()
    {
        mouthIndex = DecreaseIndex(mouthIndex, MouthTextures.Count);
        MouthMaterial.mainTexture = MouthTextures[mouthIndex];
    }

    public void IncreaseNoseIndex()
    {
        noseIndex = IncreaseIndex(noseIndex, NosePrefabs.Count);
        if (currentNose != null) Destroy(currentNose);
        if (NosePrefabs[noseIndex] != null)
            currentNose = Instantiate(NosePrefabs[noseIndex], HeadBone.transform);
        else
            currentNose = null;
    }

    public void DecreaseNoseIndex()
    {
        noseIndex = DecreaseIndex(noseIndex, NosePrefabs.Count);
        if (currentNose != null) Destroy(currentNose);
        if (NosePrefabs[noseIndex] != null)
            currentNose = Instantiate(NosePrefabs[noseIndex], HeadBone.transform);
        else
            currentNose = null;
    }

    public int IncreaseIndex(int index, int count)
    {
        int i = index;
        i++;
        if (i >= count) i = 0;
        return i;
    }

    public int DecreaseIndex(int index, int count)
    {
        int i = index;
        i--;
        if (i < 0) i = count - 1;
        return i;
    }

    public void RotateRight()
    {
        Vector3 rotation = Player.transform.localEulerAngles;
        rotation += new Vector3(0, -15, 0); 
        Player.transform.localEulerAngles = rotation;
    }

    public void RotateLeft()
    {
        Vector3 rotation = Player.transform.localEulerAngles;
        rotation += new Vector3(0, 15, 0);
        Player.transform.localEulerAngles = rotation;
    }


    public void FinishCharacterCreation()
    {
        string name = inputField.text;
        if (!string.IsNullOrWhiteSpace(name))
        {
            Player.transform.localEulerAngles = Vector3.zero;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            CCUI.SetActive(false);
            playableDirector.Play(playable);
            Player.GetComponent<Attributes>().SetStats(statsButtons.brawn, statsButtons.brains, statsButtons.guts, statsButtons.guile);
            StartCoroutine(CameraPan());
        }
        else
        {
            nameWarning.SetActive(true);
        }
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
