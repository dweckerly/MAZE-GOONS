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
        ChangeMaterialColor(SkinMaterial, SkinColors, index);
    }

    public void HairButton(int index)
    {
        ChangeMaterialColor(HairMaterial, HairColors, index);
    }

    public void NoseButton(int index)
    {
        ChangeMaterialColor(NoseMaterial, NoseColors, index);
    }

    void ChangeMaterialColor(Material material, ShadedColor[] colorArr, int index)
    {
        material.color = colorArr[index].main;
        material.SetColor("_ColorDim", colorArr[index].shaded);
    }
}
