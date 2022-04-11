
using UnityEngine;
using UnityEngine.EventSystems;

public class ElevatorButton : ButtonPress
{
    public int floor;       // Input the floor number in inspector.
    Elevator elevator;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        elevator = GameObject.FindGameObjectWithTag("Elevator").GetComponent<Elevator>();
        button.onClick.AddListener(ClickedButton);
    }

    protected override void ClickedButton()
    {
        if (EventSystem.current.IsPointerOverGameObject())
           return;

        if (elevator.previousButton != null)
        {
            if (button == elevator.previousButton)
            {
                return;
            }
        }

        base.ClickedButton();
        button.interactable = false;
        elevator.queue.Add(button);
        elevator.previousButton = button;
        StartCoroutine(elevator.ButtonCooldown());
    }
}
