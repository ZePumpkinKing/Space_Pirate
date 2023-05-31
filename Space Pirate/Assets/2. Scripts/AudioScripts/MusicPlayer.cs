using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioSource battleMusic, ambientMusic;

    private void OnEnable()
    {
        ActionEvents.EnteredBattleRoom += ManageEnterVolume;
        ActionEvents.ExitBattleRoom += ManageExitVolume;

    }
    private void OnDisable()
    {
        ActionEvents.EnteredBattleRoom -= ManageEnterVolume;
        ActionEvents.ExitBattleRoom -= ManageExitVolume;
    }

    private void ManageExitVolume()
    {
        IncreaseVolume(ambientMusic);
        DecreaseVolume(battleMusic);
    }
    private  void ManageEnterVolume()
    {
        IncreaseVolume(battleMusic);
        DecreaseVolume(ambientMusic);
    }
    private async void IncreaseVolume(AudioSource audioSrc)
    {
        while (audioSrc.volume < .7)
        {
            audioSrc.volume += Time.deltaTime;
            await Task.Yield();
        }
    }

    private async void DecreaseVolume(AudioSource audioSrc)
    {
        while (audioSrc.volume > 0)
        {
            audioSrc.volume -= Time.deltaTime;
            await Task.Yield();
        }
    }

}
