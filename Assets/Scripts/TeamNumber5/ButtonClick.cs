using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ButtonClick : MonoBehaviour
{
    private RaycastHit hit;

    private TextMeshProUGUI tm;

    Elevator ele;
    // Start is called before the first frame update
    void Start()
    {
        tm.GetComponent<TextMeshProUGUI>();
        ele = FindObjectOfType<Elevator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.gameObject.tag);
               if( hit.transform.tag == "Button")
                {

                hit.transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                } 
            }
        }
    }
}
