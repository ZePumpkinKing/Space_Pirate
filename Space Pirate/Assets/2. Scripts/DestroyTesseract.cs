using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTesseract : MonoBehaviour, IDamageable
{
    int destroyedEmitters;
    bool destructible;
    float health = 50;
    [SerializeField] GameObject explosion;
    VoiceLinePlayer voiceLinePlayer;
    [SerializeField] int voiceLineNum;

    private void Start()
    {
        voiceLinePlayer = FindObjectOfType<VoiceLinePlayer>();
    }

    private void Update()
    {
        if (health < 0)
        {
            ActionEvents.DestroyedTesseract();
            Destroy(gameObject);
            voiceLinePlayer.PlayVoiceLine(voiceLineNum);
            Instantiate(explosion, transform.position, transform.rotation);
        }
        
    }
    public void TakeDamage(float dmg)
    {
        if (destructible)
        {
            health -= dmg;
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
