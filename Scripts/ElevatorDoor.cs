
using System.Collections;
using UnityEngine;

public class ElevatorDoor : MonoBehaviour
{
    const float speed = 0.5f;
    const float waitTime = 5f;
    const float xOpenedPos = 3.5f;
    const float xClosedPos = 1.505f;
    const float xSlowSpeedPos = 1.7f;
    const float slowSpeedFactor = 0.25f;

    // There are four states: Door Opening, Closing, Opened, Closed
    [HideInInspector]
    public bool isOpening = false;
    [HideInInspector]
    public bool isClosing = false;
    [HideInInspector]
    public bool isClosed = true;

    Transform elevatorDoorLeft;
    Transform elevatorDoorRight;

    const string ELEVATOR_INSIDEDOOR_NAME = "Elevator Door";

    private void Start()
    {
        elevatorDoorLeft = transform.GetChild(0);
        elevatorDoorRight = transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        // Return if game is paused
        if (Time.timeScale == 0)
            return;
        
        if (!isOpening && !isClosing && gameObject.name == ELEVATOR_INSIDEDOOR_NAME)
        {
            ElevatorAudio.Instance.Stop(Clip.ClipName.ElevatorOpens);
        }
    }

    IEnumerator OpenDoors()
    {
        float distance;
        while (elevatorDoorLeft.position.x < xOpenedPos)
        {
            distance = speed * Time.deltaTime;
            elevatorDoorLeft.Translate(distance * Vector3.right);
            elevatorDoorRight.Translate(distance * Vector3.left);
            yield return null;
        }
        DoorOpened();
    }

    IEnumerator CloseDoors()
    {
        float distance;
        while (elevatorDoorLeft.position.x > xClosedPos)
        {
            if (elevatorDoorLeft.position.x <= xSlowSpeedPos) {
                distance = speed * slowSpeedFactor * Time.deltaTime;
            }
            else {
                distance = speed * Time.deltaTime;
            }
            elevatorDoorRight.Translate(distance * Vector3.right);
            elevatorDoorLeft.Translate(distance * Vector3.left);
            yield return null;
        }
        DoorClosed();
    }

    IEnumerator DelayClose()
    {
        float delayTime = 0f;
        while (delayTime < waitTime)
        {
            delayTime += Time.deltaTime;
            yield return null;
        }
        CloseElevatorTrigger();
    }

    public void OpenElevatorTrigger()
    {
        isOpening = true;
        isClosing = false;
        isClosed = false;
        StopCoroutine(nameof(CloseDoors));
        StopCoroutine(nameof(DelayClose));
        StartCoroutine(nameof(OpenDoors));
        ElevatorAudio.Instance.Play(Clip.ClipName.ElevatorOpens);
    }

    public void CloseElevatorTrigger()
    {
        isClosing = true;
        isOpening = false;
        StopCoroutine(nameof(OpenDoors));
        StopCoroutine(nameof(DelayClose));
        StartCoroutine(nameof(CloseDoors));
        ElevatorAudio.Instance.Play(Clip.ClipName.ElevatorOpens);
    }

    void DoorOpened()
    {
        isOpening = false;
        elevatorDoorLeft.position = new Vector3(xOpenedPos, elevatorDoorLeft.position.y, elevatorDoorLeft.position.z);
        elevatorDoorRight.position = new Vector3(-xOpenedPos, elevatorDoorRight.position.y, elevatorDoorRight.position.z);
        StartCoroutine(nameof(DelayClose));
    }

    void DoorClosed()
    {
        isClosing = false;
        isClosed = true;
        elevatorDoorLeft.position = new Vector3(xClosedPos, elevatorDoorLeft.position.y, elevatorDoorLeft.position.z);
        elevatorDoorRight.position = new Vector3(-xClosedPos, elevatorDoorRight.position.y, elevatorDoorRight.position.z);
    }
}
