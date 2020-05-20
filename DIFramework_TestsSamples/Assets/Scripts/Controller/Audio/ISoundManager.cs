using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISoundManager
{
    void Play(AudioSource audio);
    bool IsPlayingSound(AudioSource audio);
}

public class SoundManager : ISoundManager
{
    public void Play(AudioSource audio)
    {
        audio.Play();
    }

    public bool IsPlayingSound(AudioSource audio)
    {
        return audio.isPlaying;
    }
}
