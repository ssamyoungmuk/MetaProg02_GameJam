using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ButtonClick : MonoBehaviour
{
    private RaycastHit hit;

    private TextMeshProUGUI tm;
    // Start is called before the first frame update
    void Start()
    {
        tm.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.gameObject.name);
                
                hit.transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
    }
}
