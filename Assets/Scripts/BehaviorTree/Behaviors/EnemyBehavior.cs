using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    Node root;

    void Start()
    {
        root = new Node("Enemy Behavior");
        Node lookForPlayer = new Node("Look For Player");
        Node moveToPlayer = new Node("Move To Player");
        Node attackPlayer = new Node("Attack Player");

        root.AddChild(lookForPlayer);
        root.AddChild(moveToPlayer);
        root.AddChild(attackPlayer);
        root.PrintTree();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
