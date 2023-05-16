using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTrigger : MonoBehaviour
{
    Input input;
    Camera playerCam;
    GameObject player;
    private bool inTrigger;
    public int sceneId;
    bool switched;
    bool movingCam;
    private float _current;
    private float _currentRot;
    float target = 1;
    DropPodOpen dropPod;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] float camSpeed;
    [SerializeField] float camRotSpeed;
    [SerializeField] GameObject dropPodCam;
    GameObject currentDropCam;
    [SerializeField] Transform camFinalPos;

    float timerCap = 2.5f;
    float timer;
    Vector3 Playerpos;
    Quaternion playerRot;

    private void Awake()
    {
        input = new Input();
        input.Gameplay.Interact.performed += context => SwitchScene();
    }
    

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
    private void Start()
    {
        timer = timerCap;
        dropPod = FindObjectOfType<DropPodOpen>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerCam = Camera.main;
    }
    private void Update()
    {
        if (inTrigger)
        {
            timer -= Time.deltaTime;
        }
        if (movingCam)
        {
            _current = Mathf.MoveTowards(_current, target, camSpeed * Time.deltaTime);
            _currentRot = Mathf.MoveTowards(_currentRot, target, camRotSpeed * Time.deltaTime);
            currentDropCam.transform.position = Vector3.Lerp(Playerpos, camFinalPos.position, curve.Evaluate(_current));
            currentDropCam.transform.rotation = Quaternion.Lerp(playerRot, camFinalPos.rotation, curve.Evaluate(_current));
            if (currentDropCam.transform.position == camFinalPos.position)
            {
                movingCam = false;
            }
        }
    }
    private void SwitchScene()
    {
        if (inTrigger && !switched && timer < 0)
        {
            StartCoroutine(CameraCutsceneEvent());
            switched = true;
        }
    }

    IEnumerator CameraCutsceneEvent()
    {
        Playerpos = playerCam.transform.position;
        playerRot = playerCam.transform.rotation;
        Destroy(player);
        
        currentDropCam = Instantiate(dropPodCam, Playerpos, playerRot);
        movingCam = true;
        yield return new WaitUntil(() => !movingCam);
        dropPod.dropPodAnimator.SetBool("IsInRange", false);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneId);
    }

    private void OnTriggerEnter(Collider other)
    {
        timer = timerCap;
        if (other.CompareTag("Player"))
        {
            inTrigger = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = false;
        }
    }
}
