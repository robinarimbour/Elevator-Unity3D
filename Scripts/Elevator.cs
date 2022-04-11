
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Elevator : MonoBehaviour
{
    int targetFloor;
    [SerializeField] int currentFloor = 0;  // Floor level starts from 0.

    const float speed = 0.5f;
    const float openDelay = 2f;
    const float floorHeight = 5f;           // Height of each floor/elevator.

    bool isMoving = false;
    bool isDelayOpen = false;

    [HideInInspector]
    public List<Button> queue;              // Queue of buttons pressed.
    bool queueNext = true;
    
    Button targetButton;
    [HideInInspector]
    public Button previousButton;           // Avoid successive button press.
    const float prevButtonCooldown = 5f;

    [SerializeField] ElevatorDoor insideDoor;
    const string ELEVATOR_DOOR_NAME = "Elevator Door ";

    [SerializeField] List<TextMeshProUGUI> floorTexts;  // Update every elevator panels.

    private void Start()
    {
        foreach (TextMeshProUGUI text in floorTexts)
        {
            text.SetText(currentFloor.ToString());
        }
    }

    private void Update()
    {
        // Return if game is paused
        if (Time.timeScale == 0)
            return;

        if (queue.Count != 0 && queueNext && insideDoor.isClosed && !isDelayOpen)
        {
            SetTargetFloor();
        }
    }

    public IEnumerator ButtonCooldown()
    {
        float delayTime = 0f;
        while (delayTime < prevButtonCooldown)
        {
            delayTime += Time.deltaTime;
            yield return null;
        }
        previousButton = null;
    }

    void SetTargetFloor()
    {
        Button minButton = queue[0];
        int minFloor = queue[0].GetComponent<ElevatorButton>().floor;

        // Set the closest floor as the target floor.
        foreach (Button button in queue)
        {
            int tempFloor = button.GetComponent<ElevatorButton>().floor;
            if (Mathf.Abs(currentFloor - tempFloor) < Mathf.Abs(currentFloor - minFloor))
            {
                minFloor = tempFloor;
                minButton = button;
            }
        }

        targetFloor = minFloor;
        targetButton = minButton;

        queueNext = false;
        CallElevator();
    }

    void CallElevator()
    {
        if (currentFloor == targetFloor)
        {
            targetButton.interactable = true;
            ElevatorAudio.Instance.Play(Clip.ClipName.ElevatorBellRing);
            isDelayOpen = true;
            StartCoroutine(nameof(DelayOpen));
            queue.Remove(targetButton);
            targetButton = null;
            queueNext = true;
        }
        else
        {
            isMoving = true;
            ElevatorAudio.Instance.Play(Clip.ClipName.ElevatorMovingStarts);
            ElevatorAudio.Instance.Play(Clip.ClipName.ElevatorMoving);
            StartCoroutine(nameof(MoveElevator));
        }
    }

    public void OpenDoors()
    {
        if (!insideDoor.isOpening && !isMoving)
        {
            ElevatorDoor outsideDoor = GameObject.Find(ELEVATOR_DOOR_NAME + currentFloor).GetComponent<ElevatorDoor>();
            insideDoor.OpenElevatorTrigger();
            outsideDoor.OpenElevatorTrigger();
        }
    }

    public void CloseDoors()
    {
        if (!insideDoor.isClosing && !insideDoor.isOpening && !insideDoor.isClosed)
        {
            ElevatorDoor outsideDoor = GameObject.Find(ELEVATOR_DOOR_NAME + currentFloor).GetComponent<ElevatorDoor>();
            insideDoor.CloseElevatorTrigger();
            outsideDoor.CloseElevatorTrigger();
        }
    }

    IEnumerator DelayOpen()
    {
        float delayTime = 0f;
        while (delayTime < openDelay)
        {
            delayTime += Time.deltaTime;
            yield return null;
        }
        OpenDoors();
        isDelayOpen = false;
    }

    IEnumerator MoveElevator()
    {
        float targetHeight = targetFloor * floorHeight;
        if (currentFloor > targetFloor)
        {
            while (transform.position.y > targetHeight)
            {
                transform.Translate(speed * Time.deltaTime * Vector3.down);
                UpdateFloorTexts();
                yield return null;
            }
        }
        else
        {
            while (transform.position.y < targetHeight)
            {
                transform.Translate(speed * Time.deltaTime * Vector3.up);
                UpdateFloorTexts();
                yield return null;
            }
        }
        ReachedFloor();
    }

    void ReachedFloor()
    {
        transform.position = new Vector3(transform.position.x, targetFloor * floorHeight, transform.position.z);
        isMoving = false;
        targetButton.interactable = true;
        ElevatorAudio.Instance.Play(Clip.ClipName.ElevatorBellRing);
        ElevatorAudio.Instance.Stop(Clip.ClipName.ElevatorMoving);
        ElevatorAudio.Instance.Play(Clip.ClipName.ElevatorMovingEnds);
        isDelayOpen = true;
        StartCoroutine(nameof(DelayOpen));
        queue.Remove(targetButton);
        targetButton = null;
        queueNext = true;
    }

    void UpdateFloorTexts()
    {
        float tempCalc = transform.position.y / floorHeight;
        currentFloor = (int)tempCalc;
        foreach (TextMeshProUGUI text in floorTexts)
        {
            text.SetText(currentFloor.ToString());
        }
    }
}
