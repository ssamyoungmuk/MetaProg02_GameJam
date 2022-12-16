using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace ElevatorSimulator
{
    public class ButtonClick : MonoBehaviour
    {
        private RaycastHit hit;

        private TextMeshProUGUI tm;

        private Color origin;

        public bool isSelected = false;

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
                if (Physics.Raycast(ray, out hit))
                {

                    if (hit.transform.tag == "Button")
                    {
                        if (isSelected)
                            return;
                        if (!isSelected)
                        {
                            origin = hit.transform.GetChild(0).GetComponent<TextMeshPro>().color;
                            hit.transform.GetChild(0).GetComponent<TextMeshPro>().color = Color.red;
                            ElevatorSystem.Instance.Elevator.OutSortAndAddDestination(hit.transform.parent.gameObject.name, hit.transform.gameObject.name);
                            Debug.Log(hit.transform.parent.gameObject.name); // 층수확인
                            Debug.Log(hit.transform.gameObject.name); // 업다운인지확인
                            isSelected = true;
                        }
                        else
                        {

                            hit.transform.GetChild(0).GetComponent<TextMeshPro>().color = origin;
                            Debug.Log(hit.transform.gameObject);
                            //col = hit.transform.GetComponent<Color>();
                            //hit.transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                            isSelected = false;
                        }

                    }
                }
            }
        }
    }
}