using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainBehavior : MonoBehaviour
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;
    [SerializeField] private Transform Train;
    AudioSource audio;
    [SerializeField] AudioClip horn;
    GameManager gm;

    [SerializeField] float TrainSpeed;

    float interpolateAmount;

    [SerializeField] float delayTime;

    bool stopped;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        audio = GetComponent<AudioSource>();
    }
    private void Update()
    {
        TrainTravel();
    }

    private void TrainTravel()
    {
        if (!stopped)
        {
            if (gm != null)
            {
                if (!gm.paused)
                {
                    interpolateAmount = interpolateAmount + Time.fixedDeltaTime * TrainSpeed;
                }
            } else
            {
                interpolateAmount = interpolateAmount + Time.fixedDeltaTime * TrainSpeed;
            }
            
            
            if (interpolateAmount > 1)
            {
                StartCoroutine(TrainDelay());
                stopped = true;
            }
        }
        Train.position = Vector3.Lerp(start.position, end.position, interpolateAmount);
    }

    private IEnumerator TrainDelay()
    {
        yield return new WaitForSeconds(delayTime);
        interpolateAmount = 0;
        stopped = false;
        audio.PlayOneShot(horn);
    }
}
