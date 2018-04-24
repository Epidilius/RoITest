using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vehicle : MonoBehaviour
{
    GameObject Home;
    GameObject Destination;
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetHome(GameObject home)
    {
        Home = home;
    }
    public void SetDestination(GameObject destination)
    {
        Destination = destination;  //TODO: Do this? Or just set the AI Agent's target?
    }
    public void SetTransform(Transform newTransform)
    {
        gameObject.GetComponent<NavMeshAgent>().Warp(newTransform.position);    //TODO: Do I need to do gameObject.transform?
    }

    public void StartDriving()
    {
        gameObject.GetComponent<NavMeshAgent>().SetDestination(Destination.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Destination")
        {
            //TODO: Pause, then continue
            //TODO: Differentiate between Consumer and Producer destinations. Maybe have two endpoint types?
        }
    }
}
