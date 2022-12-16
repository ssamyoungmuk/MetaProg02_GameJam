using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum State
{
    stop, isUp, isDown
}
public class Elevator : MonoBehaviour
{
    [System.Serializable]
    public class ElevatorButton // Open Close buttons
    {
        public GameObject open, close;
    }
    [System.Serializable]
    public class FloorButton // Elevator's floor buttons
    {
        public GameObject[] button = new GameObject[8];
    }
    [System.Serializable]
    public class ElevatorDoor // Elevator's door
    {
        public GameObject left, right;
    }
    [SerializeField] private TextMeshPro indicator = null;

    [SerializeField] private ElevatorButton buttons = null;
    [SerializeField] private FloorButton floorButtons = null;

    [SerializeField] private ElevatorDoor doors = null;

    [SerializeField] private OutsideSystem[] outside = new OutsideSystem[8];

    public TextMeshPro Indicator => indicator;
    public ElevatorButton Buttons => buttons;
    public FloorButton FloorButtons => floorButtons;
    public ElevatorDoor Doors => doors;
    public OutsideSystem[] Outside => outside;

    // Current elevator's state
    private State myState = State.stop;
    public State GetMyState() => myState;

    public List<float> upDestination = new List<float>();
    public List<float> downDestination = new List<float>();
    private Dictionary<float, int> dest_y = new Dictionary<float, int>()
    { { 0f, 0 }, { -16f, 1 }, { -10f, 2 }, { -4f, 3 }, { 2f, 4 }, {8f, 5 }, {14f, 6 } , {20f, 7 } };

    [SerializeField] private float moveSpeed = 1f;
    private Coroutine myCoroutine = null;
    private void Start()
    {
        myCoroutine ??= StartCoroutine(MoveElevator());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SortAndAddDestination(-4f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SortAndAddDestination(-16f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SortAndAddDestination(-10f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SortAndAddDestination(2f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SortAndAddDestination(8f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SortAndAddDestination(14f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SortAndAddDestination(20f);
        }
    }
    private float offset = 0f;
    private void SortAndAddDestination(float dest)
    {
        if (myState == State.isUp)
        {
            if (dest < transform.position.y)
            {
                downDestination.Add(dest);
                downDestination.Sort();
                downDestination.Reverse();
                return;
            }
            if (dest >= transform.position.y + offset)
            {
                upDestination.Add(dest);
                upDestination.Sort();
            }
        }
        else if (myState == State.isDown)
        {
            if (dest <= transform.position.y - offset)
            {
                downDestination.Add(dest);
                downDestination.Sort();
                downDestination.Reverse();
            }
           
        }
        else
        {
            if(dest > transform.position.y)
            {
                upDestination.Add(dest);
            }
            else if (dest < transform.position.y)
            {
                downDestination.Add(dest);
            }
            else
            {
                // Open Directly
                StartCoroutine(ElevatorSystem.Instance.OpenDoor(dest_y[dest]));
            }
        }
    }
    private IEnumerator MoveElevator()
    {
        while (true)
        {
            if (upDestination.Count > 0)
            {
                myState = State.isUp;
                if (transform.position.y >= upDestination[0])
                {
                    transform.position = new Vector3(transform.position.x, upDestination[0], transform.position.z);
                    StartCoroutine(ElevatorSystem.Instance.OpenDoor(dest_y[upDestination[0]]));
                    upDestination.RemoveAt(0);
                    myState = State.stop;
                    yield return new WaitForSecondsRealtime(3f);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, upDestination[0], transform.position.z), Time.deltaTime * moveSpeed);
                }
            }
            else if (downDestination.Count > 0)
            {
                // Down
                myState = State.isDown;
                if (transform.position.y <= downDestination[0])
                {
                    transform.position = new Vector3(transform.position.x, downDestination[0], transform.position.z);
                    StartCoroutine(ElevatorSystem.Instance.OpenDoor(dest_y[downDestination[0]]));
                    downDestination.RemoveAt(0);
                    myState = State.stop;
                    yield return new WaitForSecondsRealtime(3f);
                }
                else
                {
                    transform.position -= new Vector3(0f, Time.deltaTime * moveSpeed, 0f);
                }
            }
            else
            {
                myState = State.stop;
            }
            yield return null;
        }
    }
}
