using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScene : MonoBehaviour
{

    public GameObject camRotateObj;

    public GameObject flightLogicObj;

    public int rotationDelay = 3;

    public int sceneDelay = 5;

    public Transform start;

    public Transform end;

    float speed = 0.075f;

    float timeCount = 0.0f;

    bool postDelay = false;

    private void Awake()
    {
        flightLogicObj.SetActive(false);
        StartCoroutine(EndingCoroutine());
    }

    IEnumerator EndingCoroutine()
    {
        yield return new WaitForSeconds(rotationDelay);

        ///Debug.LogWarning("coroutine running :D");

        postDelay = true;

        yield return new WaitForSeconds(1);

        flightLogicObj.SetActive(true);

        yield return new WaitForSeconds(sceneDelay);

        SceneManager.LoadScene(0);
    }

    void Update()
    {
        if (postDelay == true)
        {
            camRotateObj.transform.rotation = Quaternion.Lerp(start.rotation, end.rotation, timeCount * speed);
            timeCount = timeCount + Time.deltaTime;
        }
        
    }
}
