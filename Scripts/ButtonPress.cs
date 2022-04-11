
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonPress : MonoBehaviour
{
    protected Button button;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ClickedButton);
    }

    protected virtual void ClickedButton()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        ElevatorAudio.Instance.Play(Clip.ClipName.ButtonPress);
    }
}
