using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public enum Status { SUCCESS, RUNNING, FAILURE }
    public Status status;
    public List<Node> children = new List<Node>();
    public int currentChild = 0;
    public string name;

    public Node() 
    {
        name = "Root";
    }

    public Node(string _name)
    {
        name = _name;
    }

    public void AddChild(Node n)
    {
        children.Add(n);
    }

    struct NodeLevel
    {
        public int level;
        public Node node;
    }

    public void PrintTree()
    {
        string treePrintout = "";
        Stack<NodeLevel> nodeStack = new Stack<NodeLevel>();
        Node currentNode = this;
        nodeStack.Push(new NodeLevel { level = 0, node = currentNode});

        while (nodeStack.Count > 0)
        {
            NodeLevel nextNode = nodeStack.Pop();
            treePrintout += new string('-', nextNode.level) + nextNode.node.name + "\n";
            for (int i = nextNode.node.children.Count - 1; i > -1; i--)
            {
                nodeStack.Push(new NodeLevel { level = nextNode.level + 1, node = nextNode.node.children[i]});
            }
        }
        Debug.Log(treePrintout);
    }
}
