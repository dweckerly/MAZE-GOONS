using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoonGenerator : MonoBehaviour
{
    public GoonParts goonParts;
    public Transform headBone;
    private List<GameObject> parts = new List<GameObject>();

    private void Start() 
    {
        Generate();
    }

    public void Generate()
    {
        DestryOldParts();
        parts.Clear();
        SetColors(goonParts.SkinMaterial, goonParts.SkinColors, goonParts.SkinShadeColors);
        SetColors(goonParts.NoseMaterial, goonParts.NoseColors, goonParts.NoseShadeColors);
        SetColors(goonParts.HairMaterial, goonParts.HairColors, goonParts.HairShadeColors);
        SetTexutre(goonParts.EyeMaterial, goonParts.Eyes);
        SetTexutre(goonParts.MouthMaterial, goonParts.Mouths);
        AddParts(goonParts.Ears);
        AddParts(goonParts.Hair);
        AddParts(goonParts.Noses);
    }

    private void DestryOldParts()
    {
        foreach (GameObject part in parts)
        {
            Destroy(part);
        }
    }

    private void SetColors(Material mat, Color[] baseColors, Color[] shadeColors)
    {
        int index = Random.Range(0, baseColors.Length);
        mat.color = baseColors[index];
        mat.SetColor("_ColorDim", shadeColors[index]);
    }

    private void SetTexutre(Material mat, Texture[] textures)
    {
        int index = Random.Range(0, textures.Length);
        mat.mainTexture = textures[index];
    }

    private void AddParts(GameObject[] gos)
    {
        int index = Random.Range(0, gos.Length);
        parts.Add(Instantiate(gos[index], headBone));
    }
}
