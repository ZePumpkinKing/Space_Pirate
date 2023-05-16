using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPipeAudio : MonoBehaviour
{
    AudioSource audioSrc;
    [SerializeField] GameObject particles;
    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        ActionEvents.DestroyPipeSwitchGravity += PlaySfx;
    }
    private void OnDisable()
    {
        ActionEvents.DestroyPipeSwitchGravity -= PlaySfx;
    }
    private void PlaySfx()
    {
        StartCoroutine(Delay());
        Instantiate(particles, new Vector3(-122.464996f, 190.158997f, 135.29039f), Quaternion.identity);
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(.3f);
        audioSrc.Play();
    }
    
}
