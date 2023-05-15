using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSequences : MonoBehaviour
{
    private AudioSource AIAudio;
    private Collider collisionDetect;

    // Start is called before the first frame update
    void Start()
    {
        AIAudio = GetComponent<AudioSource>();
        collisionDetect = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        AIAudio.Play();
        StartCoroutine("destroyCollider");
    }

    private IEnumerator destroyCollider()
    {
        yield return new WaitForSeconds(1.0f);
        collisionDetect.enabled = !collisionDetect.enabled;
    } 
}
