using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
