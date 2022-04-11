using System;
using UnityEngine;

public class ElevatorAudio : MonoBehaviour
{
    public static ElevatorAudio Instance;

    public Clip[] clips;
    // Demo Audio Source for roll off Animation Curve
    AudioSource demoAudioSource;

    void Awake()
    {
        Instance = this;

        if (TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            demoAudioSource = audioSource;
        }

        foreach (Clip s in clips)
        {
            s.source = s.audioSourceObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatialBlend;
            if (s.spatialBlend > 0)
            {
                s.source.maxDistance = s.maxDistance;
                s.source.rolloffMode = AudioRolloffMode.Custom;
                s.source.SetCustomCurve(AudioSourceCurveType.CustomRolloff, demoAudioSource.GetCustomCurve(AudioSourceCurveType.CustomRolloff));
            }
        }
    }

    public void Play(Clip.ClipName name)
    {
        Clip c = Array.Find(clips, clip => clip.clipName == name);
        if (c == null)
        {
            Debug.LogWarning("Clip: " + name + " not found!");
            return;
        }
        c.source.volume = c.volume;
        c.source.pitch = c.pitch;

        c.source.Play();
    }

    public void Stop(Clip.ClipName name)
    {
        Clip c = Array.Find(clips, clip => clip.clipName == name);

        if (c == null)
        {
            Debug.LogError("Clip " + name + " Not Found!");
            return;
        }

        c.source.Stop();
    }

    public bool CheckAudioSourcePlaying(Clip.ClipName name)
    {
        Clip c = Array.Find(clips, clip => clip.clipName == name);

        if (c == null)
        {
            Debug.LogError("Clip " + name + " Not Found!");
            return false;
        }

        return c.source.isPlaying;
    }
}
