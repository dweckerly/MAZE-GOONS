using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DependencySequence : Node
{
    Node dependency;

    public DependencySequence(string _name, Node _dependency)
    {
        name = _name;
        dependency = _dependency;
    }

    public override Status Process()
    {
        if (dependency.Process() == Status.FAILURE)
        {
            foreach (Node n in children) n.Reset();
            return Status.FAILURE;
        } 

        Status childStatus = children[currentChild].Process();
        if (childStatus == Status.RUNNING || childStatus == Status.FAILURE) return childStatus;

        currentChild++;
        if (currentChild >= children.Count)
        {
            currentChild = 0;
            return Status.SUCCESS;
        }
        return Status.RUNNING;
    }
}
