using System;
using System.Collections;
using System.Collections.Generic;

public static class Utility 
{
    private static Random rng = new Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static bool IsSameArray<T>(this T[] arr0, T[] arr1)
    {
        if (arr0.Length != arr1.Length) return false;
        for(int i = 0; i < arr0.Length; i++)
        {
            if (!arr0[i].Equals(arr1[i])) return false;
        }
        return true;
    }

    // private void FleeFromTarget(float deltaTime, GameObject target)
    // {
    //     if (stateMachine.Agent.isOnNavMesh)
    //     {
    //         bool isDirectionSafe = false;
    //         while (!isDirectionSafe)
    //         {
    //             Vector3 directionToPlayer = stateMachine.gameObject.transform.position - target.transform.position;
    //             Vector3 newPosition = stateMachine.gameObject.transform.position + directionToPlayer;
    //             newPosition = Quaternion.Euler(0, vRotation, 0) * newPosition;
    //             bool isHit = Physics.Raycast(stateMachine.gameObject.transform.position, newPosition, out RaycastHit hit, wallDetectionDistance);
    //             if (isHit && hit.transform.CompareTag("Wall"))
    //             {
    //                 int lor = Random.Range(0, 2);
    //                 if (lor == 0) vRotation += Random.Range(30, 90);
    //                 else vRotation += Random.Range(-90, -30);
    //                 isDirectionSafe = false;
    //             }
    //             else
    //             {
    //                 stateMachine.Agent.destination = newPosition;
    //                 Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
    //                 stateMachine.gameObject.transform.rotation = Quaternion.LookRotation(newPosition);
    //                 isDirectionSafe = true;
    //             }
    //         }
    //     }
    //     stateMachine.Agent.velocity = stateMachine.Controller.velocity;
    // }
}
