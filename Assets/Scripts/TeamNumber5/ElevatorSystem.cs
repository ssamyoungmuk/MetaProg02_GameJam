using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ElevatorSimulator
{
    public class ElevatorSystem : MonoBehaviour
    {
        private static volatile ElevatorSystem instance = null;
        private static object lockObj = new object();
        public static ElevatorSystem Instance
        {
            get
            {
                lock (lockObj)
                {
                    instance ??= FindObjectOfType<ElevatorSystem>();
                    instance ??= new GameObject(typeof(ElevatorSystem).ToString(), typeof(ElevatorSystem)).GetComponent<ElevatorSystem>();
                }
                return instance;
            }
        }
        private void Awake()
        {
            instance = this;
        }

        [SerializeField] private Elevator elevator = null;
        public Elevator Elevator => elevator;

        [SerializeField] private float doorSpeed = 3f;
        public IEnumerator OpenDoor(int floor)
        {
            float deltaDoor = 0f;
            while (true)
            {
                if (deltaDoor < 2.0f)
                {
                    elevator.Doors.left.transform.position += new Vector3(Time.deltaTime * doorSpeed, 0f, 0f);
                    elevator.Doors.right.transform.position -= new Vector3(Time.deltaTime * doorSpeed, 0f, 0f);
                    elevator.Outside[floor].FloorDoors.left.transform.position += new Vector3(Time.deltaTime * doorSpeed, 0f, 0f);
                    elevator.Outside[floor].FloorDoors.right.transform.position -= new Vector3(Time.deltaTime * doorSpeed, 0f, 0f);
                    deltaDoor += Time.deltaTime * doorSpeed;
                }
                else
                {
                    yield return new WaitForSecondsRealtime(2f);
                    break;
                }
                yield return null;
            }
            deltaDoor = 0f;
            while (true)
            {
                if (deltaDoor < 2.0f)
                {
                    elevator.Doors.left.transform.position -= new Vector3(Time.deltaTime * doorSpeed, 0f, 0f);
                    elevator.Doors.right.transform.position += new Vector3(Time.deltaTime * doorSpeed, 0f, 0f);
                    elevator.Outside[floor].FloorDoors.left.transform.position -= new Vector3(Time.deltaTime * doorSpeed, 0f, 0f);
                    elevator.Outside[floor].FloorDoors.right.transform.position += new Vector3(Time.deltaTime * doorSpeed, 0f, 0f);
                    deltaDoor += Time.deltaTime * doorSpeed;
                }
                else
                {
                    yield return new WaitForSecondsRealtime(1f);
                    yield break;
                }
                yield return null;
            }
        }
    }
}