using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    InputReader inputReader;

    private void Awake() 
    {
        inputReader = GetComponent<InputReader>();    
    }
}
