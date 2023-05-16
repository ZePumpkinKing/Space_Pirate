using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandShield : MonoBehaviour
{
    Vector3 targetScale;
    Vector3 beginningScale;
    int destroyedEmitters;
    bool startExpand;
    VoiceLinePlayer voiceLinePlayer;
    [SerializeField] int voiceLineNum;
    [SerializeField] float pulseAmount;
    [SerializeField] float pulseSpeed;
    Collider[] colls;

    float current;

    private void Start()
    {
        voiceLinePlayer = FindObjectOfType<VoiceLinePlayer>();
        colls = GetComponentsInChildren<Collider>();
        beginningScale = transform.localScale;
        targetScale = new Vector3(480, 480, 480);
    }

    private void Update()
    {
        Debug.Log(current);
        if (startExpand)
        {
            current = Mathf.MoveTowards(0, 1, 3 * Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, current);
            StartCoroutine(Deletion());
        } else  
        {
            transform.localScale = new Vector3(beginningScale.x + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount,
                                               beginningScale.y + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount,
                                               beginningScale.z + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount);
        }
        
    }

    private void UpdateEmitterCount()
    {
        destroyedEmitters++;
        if (destroyedEmitters == 4)
        {
            startExpand = true;
            voiceLinePlayer.PlayVoiceLine(voiceLineNum);
            foreach (Collider coll in colls)
            {
                coll.isTrigger = true;
            }
        }
    }

    private IEnumerator Deletion()
    {
        yield return new WaitForSeconds(2);
        
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        ActionEvents.DestroyedEmitter += UpdateEmitterCount;
    }
    private void OnDisable()
    {
        ActionEvents.DestroyedEmitter -= UpdateEmitterCount;
    }
}
