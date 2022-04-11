
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonFunctionality : ButtonPress
{
    [SerializeField] UnityEvent functionToCall;

    protected override void ClickedButton()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        base.ClickedButton();
        functionToCall.Invoke();
    }
}
