using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public int sceneNumber;

    public GameObject logicGO;

    public int delayTime;

    public void Awake()
    {
        logicGO.gameObject.SetActive(false);
    }

    public void SceneTransition()
    {
        StartCoroutine (MainTransition());
    }

    IEnumerator MainTransition()
    {
        logicGO.gameObject.SetActive(true);
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene(sceneNumber);
    }

}
