using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : MonoBehaviour
{
    BehaviourTree tree;
    public GameObject diamond;
    public GameObject van;
    public GameObject backdoor;
    public GameObject frontdoor;
    NavMeshAgent agent;

    public enum ActionState { IDLE, WORKING };

    ActionState state = ActionState.IDLE;

    Node.Status treeStatus = Node.Status.RUNNING;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();

        tree = new BehaviourTree();
        Sequence steal = new Sequence("Steal Something");
        Leaf goToDiamond = new Leaf("Go to Diamond", GoToDiamond);
        Leaf goToBackdoor = new Leaf("Go to Backdoor", GoToBackdoor);
        Leaf goToVan = new Leaf("Go to Van", GoToVan);
        Leaf goToFrontDoor = new Leaf("Go to Front Door", GoToFrontDoor);
        Selector openDoor = new Selector("Open Door");

        openDoor.AddChild(goToFrontDoor);
        openDoor.AddChild(goToBackdoor);

        steal.AddChild(openDoor);
        steal.AddChild(goToDiamond);
        steal.AddChild(goToVan);

        tree.AddChild(steal);

        tree.PrintTree();
    }

    public Node.Status GoToFrontDoor()
    {
        return GoToDoor(frontdoor);
    }

    public Node.Status GoToDiamond()
    {
        Node.Status s = GoToLocation(diamond.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            diamond.transform.parent = this.gameObject.transform;
        }
        return s;
    }  

    public Node.Status GoToBackdoor()
    {
        return GoToDoor(backdoor);
    }

    public Node.Status GoToVan()
    {
        return GoToLocation(van.transform.position);
    }

    public Node.Status GoToDoor(GameObject door)
    {
        Node.Status s = GoToLocation(door.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            if(!door.GetComponent<Lock>().isLocked)
            {
                door.SetActive(false);
                return Node.Status.SUCCESS;
            }
                return Node.Status.FAILURE;
        }
        else return s;
    }

    Node.Status GoToLocation(Vector3 destination)
    {
        float distanceToTarget = Vector3.Distance(this.transform.position, destination);
        if (state == ActionState.IDLE)
        {
            agent.SetDestination(destination);
            state = ActionState.WORKING;
        }
        else if (Vector3.Distance(agent.pathEndPosition, destination) >= 2)
        {
            state = ActionState.IDLE;
            return Node.Status.FAILURE;
        }
        else if (distanceToTarget < 2)
        {
            state = ActionState.IDLE;
            return Node.Status.SUCCESS;
        }
        return Node.Status.RUNNING;
    }

    // Update is called once per frame
    void Update()
    {
        if(treeStatus == Node.Status.RUNNING)
        {
            treeStatus = tree.Process();
        }
    }
}
