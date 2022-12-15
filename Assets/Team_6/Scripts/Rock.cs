using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private void OnEnable()
    {
        transform.position = FindObjectOfType<Player>().rockStartPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
