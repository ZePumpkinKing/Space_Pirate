using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTesseractShield : MonoBehaviour
{
    // Start is called before the first frame update
    bool rotating;
    float targetXRot;
    int numberOfRots;
    
    float current;
    [SerializeField] float timeBetweenRotations;
    Quaternion targetQuaternion;
    void Start()
    {
        StartCoroutine(Rotate());
    }

    // Update is called once per frame
    void Update()
    {
        if (rotating)
        {
            current = Mathf.MoveTowards(0, 1, 3 * Time.deltaTime);
            targetQuaternion = Quaternion.Euler(targetXRot, 0, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetQuaternion, current);
        }
        else current = 0;
    }

    private IEnumerator Rotate()
    {
        yield return new WaitForSeconds(timeBetweenRotations);
        SetRotation();
        rotating = true;
        yield return new WaitForSeconds(2.5f);
        rotating = false;
        StartCoroutine(Rotate());
    }

    private void SetRotation()
    {
        numberOfRots++;
        if (numberOfRots != 4)
        {
            targetXRot = 90 * numberOfRots;
        }
        else
        {
            targetXRot = 0;
            numberOfRots = 0;
        }
    }

}
