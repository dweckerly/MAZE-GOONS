using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BodyPartMapReference
{
    public GameObject bodyPart;
    public BodyMapping bodyPositionReference;
}

public class BodyPartMap : MonoBehaviour
{
    public BodyPartMapReference[] bodyMap;
}
