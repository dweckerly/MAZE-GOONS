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

public class GoonGenerator : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GoonParts goonParts;
    public Weapon[] weapons;
    public Armor[] armors;
    public GameObject prefab;

    private  List<Goon> goons = new List<Goon>();
    private List<GameObject> parts = new List<GameObject>();

    private void Start() 
    {
        MakeGoons();    
    }

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

    private void PopulateGoons()
    {
        foreach (Transform transform in spawnPoints)
        {
            Goon goon = ScriptableObject.CreateInstance("Goon") as Goon;
            goon.Init(prefab, transform);
            goons.Add(goon);
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
        GiveWeapon(goon);
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

    private void GiveWeapon(Goon goon)
    {
        int weaponIndex = Random.Range(0, weapons.Length);
        goon.go.GetComponent<EnemyStateMachine>().WeaponHandler.EquipWeapon(weapons[weaponIndex]);
    }
}
