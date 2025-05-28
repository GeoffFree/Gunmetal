using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMaster : MonoBehaviour
{
    public AudioSource soundtrack;
    public AudioSource player;
    public AudioSource background;

    public void playSoundtrack(AudioClip song)
    {
        soundtrack.clip = song;
        soundtrack.Play();
    }

    public void playPlayer(AudioClip effect)
    {
        player.clip = effect;
        player.Play();
    }

    public void playBackground(AudioClip bg)
    {
        background.clip = bg;
        background.Play();
    }
}
