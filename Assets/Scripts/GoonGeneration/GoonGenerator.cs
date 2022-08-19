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
    [Range(0, 100)]
    public int weaponChance = 50;
    public Weapon[] weapons;
    [Range(0, 100)]
    public int armorChance = 50;
    public Armor[] armors;
    [Range(0.1f, 3f)]
    public float scaleMin = 0.75f;
    [Range(0.1f, 3f)]
    public float scaleMax = 1.25f;
    private float scale;
    public GameObject prefab;
    private EnemyStateMachine stateMachine;

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
            scale = Random.Range(scaleMin, scaleMax);
            goon.Init(prefab, transform, scale);
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
        stateMachine = goon.go.GetComponent<EnemyStateMachine>();
        stateMachine.scaleFactor = scale;
        GiveWeapon(goon);
        GiveArmor(goon);
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
        if (Random.Range(0, 100) < weaponChance)
        {
            int weaponIndex = Random.Range(0, weapons.Length);
            stateMachine.WeaponHandler.EquipWeapon(weapons[weaponIndex]);
        }
    }

    private void GiveArmor(Goon goon)
    {
        if (Random.Range(0, 100) < armorChance)
        {
            int armorIndex = Random.Range(0, armors.Length);
            stateMachine.ArmorHandler.EquipArmor(armors[armorIndex]);
        }
    }
}
