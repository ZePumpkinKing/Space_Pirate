using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMusicPlayer : MonoBehaviour
{
    AudioSource audioSrc;
    bool increaseVol;
    bool decreaseVol;
    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        ActionEvents.EnteredBattleRoom += IncreaseVolume;
        ActionEvents.ExitBattleRoom += DecreaseVolume;
    }

    private void OnDisable()
    {
        ActionEvents.EnteredBattleRoom -= IncreaseVolume;
        ActionEvents.ExitBattleRoom -= DecreaseVolume;
    }

    private void Update()
    {
        if (increaseVol)
        {

            audioSrc.volume += Time.deltaTime;
            if (audioSrc.volume >= .7)
            {
                increaseVol = false;
            }
        }
        if (decreaseVol)
        {

            audioSrc.volume -= Time.deltaTime;
            if (audioSrc.volume == 0)
            {
                decreaseVol = false;
            }
        }
    }
    private void IncreaseVolume()
    {
        increaseVol = true;
    }

    private void DecreaseVolume()
    {
        decreaseVol = true;
    }

}
