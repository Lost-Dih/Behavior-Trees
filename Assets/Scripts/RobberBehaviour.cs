using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobberBehaviour : MonoBehaviour
{
    BehaviourTree tree;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tree = new BehaviourTree();
        Node steal = new Node("Steal Something");
        Node goToDiamond = new Node("Go to Diamond");
        Node goToVan = new Node("Go to Van");

        steal.AddChild(goToDiamond);
        steal.AddChild(goToVan);
        tree.AddChild(steal);

        tree.PrintTree();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
