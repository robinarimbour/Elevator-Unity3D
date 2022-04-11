
using UnityEngine;

[System.Serializable]
public class Clip
{
    public enum ClipName
    {
        ElevatorOpens,
        ElevatorMoving,
        ElevatorMovingStarts,
        ElevatorMovingEnds,
        ButtonPress,
        ElevatorBellRing,
        AlarmBell,
    }

    public ClipName clipName;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(-3f, 3f)]
    public float pitch = 1f;
    [Range(0f, 1f)]
    public float spatialBlend;
    [Range(0f, 5f)]
    public float dopplerLevel = 1f;
    public float maxDistance = 5f;

    public bool loop;

    public GameObject audioSourceObject;
    [HideInInspector]
    public AudioSource source;
}
