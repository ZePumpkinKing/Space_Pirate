using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateDeathOverlay : MonoBehaviour
{
    RawImage overlay;
    Player player;
    private void Awake()
    {
        overlay = GetComponent<RawImage>();
        player = FindObjectOfType<Player>();
        overlay.enabled = false;
    }

    private void Update()
    {
        if (player.currentState == Player.states.dead)
        {
            overlay.enabled = true;
        }
    }

}
