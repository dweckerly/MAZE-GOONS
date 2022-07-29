using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMaskHandler : MonoBehaviour
{
    public void ApplyLayerWeight(Animator animator, int layerIndex, bool apply)
    {
        if (apply) animator.SetLayerWeight(layerIndex, 1f);
        else animator.SetLayerWeight(layerIndex, 0);
    }
}
