using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPodMovement : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;

    [SerializeField] float TrainSpeed;

    float interpolateAmount;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(5);
        LevelManager.Instance.LoadSceneWithDelay(3, 12);
    }
    private void Update()
    {
        if (interpolateAmount != 1)
        {
            interpolateAmount = Mathf.MoveTowards(interpolateAmount, 1, Time.deltaTime * TrainSpeed);
            gameObject.transform.position = Vector3.Lerp(start.position, end.position, curve.Evaluate(interpolateAmount));
        }
    }


}
