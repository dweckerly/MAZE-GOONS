using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State currentState;

    private void Update()
    {
        currentState?.Tick(Time.deltaTime);
    }

    public void SwitchState(State newState)
    {
        print("Exiting " + currentState + "\nEntering " + newState);
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
}
