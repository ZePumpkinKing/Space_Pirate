using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLinePlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] voiceLines;
    AudioSource audioPlayer;

    private void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }
    public void PlayVoiceLine(int voiceLineNumber)
    {
        audioPlayer.PlayOneShot(voiceLines[voiceLineNumber]);
    }
}
