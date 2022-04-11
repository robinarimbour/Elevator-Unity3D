
using UnityEngine;

public class ElevatorDoorSensor : MonoBehaviour
{
    Elevator elevator;
    ElevatorDoor elevatorDoor;

    private void Start()
    {
        elevator = GameObject.FindGameObjectWithTag("Elevator").GetComponent<Elevator>();
        elevatorDoor = GameObject.Find("Elevator Door").GetComponent<ElevatorDoor>();
    }

    private void OnTriggerStay(Collider other)
    { 
        if (elevatorDoor.isClosing)
        {
            elevator.OpenDoors();
        }
    }
}
