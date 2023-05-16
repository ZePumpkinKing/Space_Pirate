using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTesseract : MonoBehaviour, IDamageable
{
    int destroyedEmitters;
    bool destructible;
    [SerializeField] GameObject explosion;
    VoiceLinePlayer voiceLinePlayer;
    [SerializeField] int voiceLineNum;

    private void Start()
    {
        voiceLinePlayer = FindObjectOfType<VoiceLinePlayer>();
    }
    public void TakeDamage(float dmg)
    {
        if (destructible)
        {
            ActionEvents.DestroyedTesseract();
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
            voiceLinePlayer.PlayVoiceLine(voiceLineNum);
        }
    }

    private void UpdateEmitterCount()
    {
        destroyedEmitters++;
        if (destroyedEmitters == 4)
        {
            destructible = true;
        }
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
