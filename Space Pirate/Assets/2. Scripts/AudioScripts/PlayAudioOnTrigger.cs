using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnTrigger : MonoBehaviour
{
    VoiceLinePlayer audioBank;
    [SerializeField] int voiceLineNum;
    private bool entered;

    private void Start()
    {
        audioBank = FindObjectOfType<VoiceLinePlayer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!entered)
            {
                audioBank.PlayVoiceLine(voiceLineNum);
                entered = true;
            }
        }
        
    }
}
