using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainBehavior : MonoBehaviour
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;
    [SerializeField] private Transform Train;

    public float TrainSpeed;

    private float interpolateAmount;

    private void Update()
    {
        TrainTravel();
    }

    private void TrainFunction()
    {
        StartCoroutine("TrainSpawn");
        TrainTravel();
    }
    private void TrainTravel()
    {
        interpolateAmount = (interpolateAmount + Time.fixedDeltaTime * TrainSpeed) % 1;
        Train.position = Vector3.Lerp(start.position, end.position, interpolateAmount);
        if (Train.position == end.position)
        {
            StartCoroutine("TrainDelay");
        }
    }

    private IEnumerator TrainDelay()
    {
        yield return new WaitForSeconds(5.0f);
    }
}
