
using UnityEngine.EventSystems;

public class AlarmButton : ButtonPress
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!ElevatorAudio.Instance.CheckAudioSourcePlaying(Clip.ClipName.AlarmBell))
        {
            ElevatorAudio.Instance.Play(Clip.ClipName.AlarmBell);
        }
    }

    private void OnMouseUp()
    {
        ElevatorAudio.Instance.Stop(Clip.ClipName.AlarmBell);
    }
}
