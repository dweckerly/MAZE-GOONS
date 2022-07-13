using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    public Sequence(string _name)
    {
        name = _name;
    }

    public override Status Process()
    {
        Status childStatus = children[currentChild].Process();
        if (childStatus == Status.RUNNING) return childStatus;
        if(childStatus == Status.FAILURE) 
        {
            foreach (Node n in children) n.Reset();
            return Status.FAILURE;
        }
        
        currentChild++;
        if(currentChild >= children.Count)
        {
            currentChild = 0;
            return Status.SUCCESS;
        }
        return Status.RUNNING;
    }
}
