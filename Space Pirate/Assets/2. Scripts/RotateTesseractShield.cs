using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTesseractShield : MonoBehaviour
{
    // Start is called before the first frame update
    bool rotating;
    bool cacheRotation = false;
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
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, 0);
        if (cacheRotation)
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
            Debug.Log("Rotation cached");
            cacheRotation = false;
        }
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
        rotating = true;
        cacheRotation = true;
        yield return new WaitForSeconds(2.5f);
        rotating = false;

        StartCoroutine(Rotate());
    }
}
