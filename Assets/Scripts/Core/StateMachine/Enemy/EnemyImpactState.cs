using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyImpactState : EnemyBaseState
{
    private readonly int ImpactHash = Animator.StringToHash("Impact");
    private readonly int Impact2Hash = Animator.StringToHash("Impact-2");
    private readonly int Impact3Hash = Animator.StringToHash("Impact-3");

    private float duration = 0.3f;
    private Color impactColor = Color.red;
    protected List<Color> materialColors = new List<Color>();

    public EnemyImpactState(EnemyStateMachine _stateMachine) : base(_stateMachine) { }

    public override void Enter()
    {
        int[] animArray = new int[3];
        animArray[0] = ImpactHash;
        animArray[1] = Impact2Hash;
        animArray[2] = Impact3Hash;
        int animSelection = Random.Range(0, 3);
        stateMachine.animator.CrossFadeInFixedTime(animArray[animSelection], CrossFadeDuration);
        SetImpactColor();
    }

    public override void Exit() 
    {
        RestoreColor();
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        duration -= deltaTime;
        if (duration <= 0f) stateMachine.SwitchState(new EnemyFightingState(stateMachine));
    }

    private void SetImpactColor()
    {
        foreach (Renderer renderer in stateMachine.Renderers)
        {
            foreach (Material material in renderer.materials)
            {
                materialColors.Add(material.color);
                material.color = impactColor;
            }
        }
        
    }

    private void RestoreColor()
    {
        int offset = 0;
        foreach (Renderer renderer in stateMachine.Renderers)
        {
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                renderer.materials[i].color = materialColors[i + offset];
            }
            offset += renderer.materials.Length;
        }
        materialColors.Clear();
    }
}
