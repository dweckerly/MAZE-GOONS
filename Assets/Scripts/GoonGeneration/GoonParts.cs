using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoonParts", menuName = "Goon Parts", order = 1)]
public class GoonParts : ScriptableObject
{
    public Material SkinMaterial;
    public Color[] SkinColors;
    public Color[] SkinShadeColors;
    public Material ChestMaterial;
    public Texture ChestTexture;
    public Material EyeMaterial;
    public Texture[] Eyes;
    public Material MouthMaterial;
    public Texture[] Mouths;
    public GameObject[] Ears;
    public Material EarMaterial;
    public GameObject[] Noses;
    public Material NoseMaterial;
    public Color[] NoseColors;
    public Color[] NoseShadeColors;
    public GameObject[] Hair;
    public Material HairMaterial;
    public Color[] HairColors;
    public Color[] HairShadeColors;
}
