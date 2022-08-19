using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goon : ScriptableObject
{
    public GameObject go;
    Transform headBone;
    public Material skinMaterial;
    public Material earMaterial;
    public Material noseMaterial;
    public Material hairMaterial;
    public Material eyeMaterial;
    public Material mouthMaterial;
    GameObject ears;
    GameObject hair;
    GameObject nose;

    public void Init(GameObject original, Transform transform, float scale)
    {
        go = Instantiate(original, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation); 
        go.transform.localScale = go.transform.localScale * scale;
        headBone = go.gameObject.transform.GetChild(0).GetChild(7).GetChild(2).GetChild(0).GetChild(0).GetChild(0);
        Material[] materials = go.transform.GetChild(1).GetComponent<Renderer>().materials;
        skinMaterial = materials[0];
        earMaterial = skinMaterial;
        eyeMaterial = materials[2];
        mouthMaterial = materials[3];
    }

    public void AddEars(GameObject[] gos)
    {
        int index = Random.Range(0, gos.Length);
        ears = Instantiate(gos[index], headBone);
        earMaterial = ears.GetComponent<Renderer>().materials[0];
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
