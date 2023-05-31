using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void LoadSceneWithDelay(int sceneId, int delaySeconds)
    {
        var scene = SceneManager.LoadSceneAsync(sceneId);
        scene.allowSceneActivation = false;
        await Task.Delay(delaySeconds * 1000);
        Debug.Log("Alan is done speaking xd");
        scene.allowSceneActivation = true;
        while (!scene.isDone)
        {
            await Task.Yield();
        }
    }
    public async void LoadScene(int sceneId)
    {
        var scene = SceneManager.LoadSceneAsync(sceneId);
        while (!scene.isDone)
        {
            await Task.Yield();
        }
    }

}
