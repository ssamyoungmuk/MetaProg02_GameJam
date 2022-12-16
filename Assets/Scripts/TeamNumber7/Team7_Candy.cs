using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team7_Candy : MonoBehaviour
{
    [SerializeField] int expPoint;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Team7_Player>() != null)
        {
            //other.gameObject.GetComponent<Team7_Player>().exp += expPoint;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
