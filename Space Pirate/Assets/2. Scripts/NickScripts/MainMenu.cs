using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public int sceneNumber;

    public GameObject logicGO;

    public int delayTime;

    public GameObject creditsParent;
    public GameObject mainParent;
    public GameObject bumperClip;

    public float fadeTime;

    Color videoRendererColor = Color.white;

    public Graphic videoRenderer;

    public bool isFading = false;

    public void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        mainParent.gameObject.SetActive(true);
        creditsParent.gameObject.SetActive(false);
        logicGO.gameObject.SetActive(false);
        StartCoroutine(VideoEnd());
    }

    IEnumerator VideoEnd()
    {
        yield return new WaitForSeconds(7);
        isFading = true;
        yield return new WaitForSeconds(1);
        bumperClip.gameObject.SetActive(false);
    }


    public void Update()
    {
        videoRenderer.color = videoRendererColor;
        if (isFading == true)
        {
            videoRendererColor = Color.LerpUnclamped(Color.white, Color.black, (fadeTime += Time.deltaTime));
        }
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

    public void ExitGame()
    {
        ///Debug.Log("Bye~!");
        Application.Quit();
    }

    public void ShowCredits()
    {
        creditsParent.gameObject.SetActive(true);
        mainParent.gameObject.SetActive(false);
    }

    public void HideCredits()
    {
        mainParent.gameObject.SetActive(true);
        creditsParent.gameObject.SetActive(false);
    }
}
