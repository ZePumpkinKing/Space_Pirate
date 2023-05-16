using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPipeAudio : MonoBehaviour
{
    AudioSource audioSrc;
    [SerializeField] GameObject particles;
    GameObject particleRef;
    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        ActionEvents.DestroyPipeSwitchGravity += PlaySfx;
        ActionEvents.EnteredBattleRoom += DestroyParticles;
    }
    private void OnDisable()
    {
        ActionEvents.DestroyPipeSwitchGravity -= PlaySfx;
        ActionEvents.EnteredBattleRoom -= DestroyParticles;
    }
    private void PlaySfx()
    {
        StartCoroutine(Delay());
        particleRef = Instantiate(particles, new Vector3(-122.464996f, 190.158997f, 135.29039f), Quaternion.identity);
    }

    private void DestroyParticles()
    {
        Destroy(particleRef);
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(.3f);
        audioSrc.Play();
    }
    
}
