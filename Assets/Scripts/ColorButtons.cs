using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorButtons : MonoBehaviour
{
    public Material SkinMaterial;
    public Material EarMaterial;
    public Material HairMaterial;
    public Material NoseMaterial;

    void ChangeSkinColor(Color primary, Color secondary)
    {
        SkinMaterial.color = primary;
        EarMaterial.color = primary;
        SkinMaterial.SetColor("_ColorDim", secondary);
        EarMaterial.SetColor("_ColorDim", secondary);
    }

    void ChangeHairColor(Color primary, Color secondary)
    {
        HairMaterial.color = primary;
        HairMaterial.SetColor("_ColorDim", secondary);
    }

    void ChangeNoseColor(Color primary, Color secondary)
    {
        NoseMaterial.color = primary;
        NoseMaterial.SetColor("_ColorDim", secondary);
    }
}
