using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorButtons : MonoBehaviour
{
    public Material SkinMaterial;
    public Material HairMaterial;
    public Material NoseMaterial;

    public void SkinButton0()
    {
        Color32 primary = new Color32(188, 153, 105, 255);
        Color32 secondary = new Color32(113, 93, 67, 255);
        ChangeMaterialColor(SkinMaterial, primary, secondary);
    }

    public void SkinButton1()
    {
        Color32 primary = new Color32(188, 119, 105, 255);
        Color32 secondary = new Color32(113, 62, 52, 255);
        ChangeMaterialColor(SkinMaterial, primary, secondary);
    }

    public void SkinButton2()
    {
        Color32 primary = new Color32(245, 220, 184, 255);
        Color32 secondary = new Color32(96, 87, 75, 255);
        ChangeMaterialColor(SkinMaterial, primary, secondary);
    }

    public void SkinButton3()
    {
        Color32 primary = new Color32(173, 111, 78, 255);
        Color32 secondary = new Color32(79, 51, 36, 255);
        ChangeMaterialColor(SkinMaterial, primary, secondary);
    }

    public void SkinButton4()
    {
        Color32 primary = new Color32(202, 160, 85, 255);
        Color32 secondary = new Color32(96, 80, 50, 255);
        ChangeMaterialColor(SkinMaterial, primary, secondary);
    }

    public void SkinButton5()
    {
        Color32 primary = new Color32(108, 91, 83, 255);
        Color32 secondary = new Color32(53, 44, 40, 255);
        ChangeMaterialColor(SkinMaterial, primary, secondary);
    }

    public void SkinButton6()
    {
        Color32 primary = new Color32(152, 66, 51, 255);
        Color32 secondary = new Color32(82, 50, 44, 255);
        ChangeMaterialColor(SkinMaterial, primary, secondary);
    }

    public void SkinButton7()
    {
        Color32 primary = new Color32(255, 187, 174, 255);
        Color32 secondary = new Color32(99, 78, 74, 255);
        ChangeMaterialColor(SkinMaterial, primary, secondary);
    }

    void ChangeMaterialColor(Material material, Color primary, Color secondary)
    {
        material.color = primary;
        material.SetColor("_ColorDim", secondary);
    }
}
