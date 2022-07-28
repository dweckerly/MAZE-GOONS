using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Position
{
    public float x;
    public float y;
    public float z;
}

[System.Serializable]
public class GenParams
{
    public Position position;
    public float yRotation;
}

public class GoonGenerator : MonoBehaviour
{
    public GenParams[] spawnPoints;
    public GoonParts goonParts;
    public GameObject prefab;

    private  List<Goon> goons = new List<Goon>();
    private List<GameObject> parts = new List<GameObject>();

    public void MakeGoons()
    {
        DestryOldGoons();
        goons.Clear();
        PopulateGoons();
        foreach (Goon goon in goons)
        {
            Generate(goon);
        }
    }

    private void Generate(Goon goon)
    {
        goon.AddHair(goonParts.Hair);
        goon.AddNose(goonParts.Noses);
        goon.AddEars(goonParts.Ears);
        SetTexutre(goon.eyeMaterial, goonParts.Eyes);
        SetTexutre(goon.mouthMaterial, goonParts.Mouths);
        SetSkinMaterial(goon);
        SetColors(goon.noseMaterial, goonParts.NoseColors, goonParts.NoseShadeColors);
        SetColors(goon.hairMaterial, goonParts.HairColors, goonParts.HairShadeColors);
    }

    private void PopulateGoons()
    {
        foreach (GenParams gp in spawnPoints)
        {
            goons.Add(new Goon(prefab, gp));
        }
    }

    private void DestryOldGoons()
    {
        foreach (Goon goon in goons)
        {
            Destroy(goon.go);
            Destroy(goon);
        }
    }

    private void SetSkinMaterial(Goon goon)
    {
        int index = Random.Range(0, goonParts.SkinColors.Length);
        goon.skinMaterial.color = goonParts.SkinColors[index];
        goon.earMaterial.color = goonParts.SkinColors[index];
        goon.skinMaterial.SetColor("_ColorDim", goonParts.SkinShadeColors[index]);
        goon.earMaterial.SetColor("_ColorDim", goonParts.SkinShadeColors[index]);
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
}
