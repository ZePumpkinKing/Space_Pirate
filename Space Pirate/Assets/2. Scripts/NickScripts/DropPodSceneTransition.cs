using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DropPodSceneTransition : MonoBehaviour
{

    public int delayTime ;

    private void Awake()
    {
        StartCoroutine(SceneTransition());
    }

    IEnumerator SceneTransition()
    {
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene(2);
    }

}
