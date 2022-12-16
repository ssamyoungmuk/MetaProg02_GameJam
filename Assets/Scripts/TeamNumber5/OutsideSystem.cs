using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace ElevatorSimulator
{
    public class OutsideSystem : MonoBehaviour
    {
        [System.Serializable]
        public class UpDownButton
        {
            public GameObject up, down;
        }
        [System.Serializable]
        public class FloorDoor
        {
            public GameObject left, right;
        }

        [SerializeField] private TextMeshPro indicator = null;

        [SerializeField] private UpDownButton upDownButtons = null;

        [SerializeField] private FloorDoor floorDoor = null;

        public TextMeshPro Indicator => indicator;
        public UpDownButton UpDownButtons => upDownButtons;
        public FloorDoor FloorDoors => floorDoor;

        private void Awake()
        {
            indicator = transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>();
            upDownButtons.up = transform.GetChild(1).gameObject;
            upDownButtons.down = transform.GetChild(2).gameObject;
            floorDoor.left = transform.GetChild(3).GetChild(0).gameObject;
            floorDoor.right = transform.GetChild(3).GetChild(1).gameObject;
        }
    }
}