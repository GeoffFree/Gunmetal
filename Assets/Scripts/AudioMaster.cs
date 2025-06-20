using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMaster : MonoBehaviour
{
    public AudioSource soundtrack;
    public AudioSource background;
    public AudioSource artillery;
    public AudioSource voiceOver;

    public void playSoundtrack(AudioClip song)
    {
        soundtrack.clip = song;
        soundtrack.Play();
    }

    public void playBackground(AudioClip bg)
    {
        background.clip = bg;
        background.Play();
    }

    public void playArtillery(AudioClip arty)
    {
        artillery.clip = arty;
        artillery.Play();
    }

    public void playVoiceOver(AudioClip voiceClip)
    {
        voiceOver.clip = voiceClip;
        voiceOver.Play();
    }
}
