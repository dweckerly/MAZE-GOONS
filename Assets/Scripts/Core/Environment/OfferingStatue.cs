using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfferingStatue : Interactable
{
    public override InteractableType type { get { return InteractableType.Statue; } }

    public override void Interact(PlayerStateMachine stateMachine) {}
}
