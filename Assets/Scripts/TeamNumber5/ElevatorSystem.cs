using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private float doorSpeed = 1f;
    public IEnumerator OpenDoor(int floor)
    {
        while (true)
        {
            elevator.Doors.left.transform.position -= new Vector3(Time.deltaTime * doorSpeed, 0f, 0f);
            elevator.Doors.right.transform.position += new Vector3(Time.deltaTime * doorSpeed, 0f, 0f);
            yield return null;
        }

    }
}