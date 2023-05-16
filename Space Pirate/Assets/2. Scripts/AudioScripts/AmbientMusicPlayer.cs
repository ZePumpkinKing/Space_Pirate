using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientMusicPlayer : MonoBehaviour
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
        ActionEvents.EnteredBattleRoom += DecreaseVolume;
        ActionEvents.ExitBattleRoom += IncreaseVolume;
    }
    private void OnDisable()
    {
        ActionEvents.EnteredBattleRoom -= DecreaseVolume;
        ActionEvents.ExitBattleRoom -= IncreaseVolume;
    }

    private void Update()
    {
        if (increaseVol)
        {

            audioSrc.volume += Time.deltaTime;
            if (audioSrc.volume > .5f)
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
