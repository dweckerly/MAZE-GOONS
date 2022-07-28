using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goon : ScriptableObject
{
    public GameObject go;
    Transform headBone;
    public Material skinMaterial;
    public Material noseMaterial;
    public Material hairMaterial;
    public Material eyeMaterial;
    public Material mouthMaterial;
    GameObject ears;
    GameObject hair;
    GameObject nose;

    public Goon(GameObject original, GenParams p)
    {
        go = Instantiate(original, new Vector3(p.position.x, p.position.y, p.position.z), Quaternion.Euler(new Vector3(0, p.yRotation, 0)));
        go.GetComponent<InputReader>().enabled = false;
        go.GetComponent<Inventory>().enabled = false;
        headBone = go.gameObject.transform.GetChild(0).GetChild(7).GetChild(2).GetChild(0).GetChild(0).GetChild(0);
        Material[] materials = go.transform.GetChild(1).GetComponent<Renderer>().materials;
        skinMaterial = materials[0];
        eyeMaterial = materials[2];
        mouthMaterial = materials[3];
    }

    public void AddEars(GameObject[] gos)
    {
        int index = Random.Range(0, gos.Length);
        ears = Instantiate(gos[index], headBone);
        ears.GetComponent<Renderer>().materials[0] = skinMaterial;
    }

    public void AddHair(GameObject[] gos)
    {
        int index = Random.Range(0, gos.Length);
        hair = Instantiate(gos[index], headBone);
        hairMaterial = hair.GetComponent<Renderer>().material;
    }

    public void AddNose(GameObject[] gos)
    {
        int index = Random.Range(0, gos.Length);
        nose = Instantiate(gos[index], headBone);
        noseMaterial = nose.GetComponent<Renderer>().material;
    }
}
