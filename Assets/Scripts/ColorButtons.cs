using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorButtons : MonoBehaviour
{
    public Material SkinMaterial;
    public Material HairMaterial;
    public Material NoseMaterial;

    public ShadedColor[] SkinColors;
    public ShadedColor[] HairColors;
    public ShadedColor[] NoseColors;

    public void SkinButton(int index)
    {
        ChangeMaterialColor(SkinMaterial, index);
    }

    void ChangeMaterialColor(Material material, int index)
    {
        material.color = SkinColors[index].main;
        material.SetColor("_ColorDim", SkinColors[index].shaded);
    }
}
