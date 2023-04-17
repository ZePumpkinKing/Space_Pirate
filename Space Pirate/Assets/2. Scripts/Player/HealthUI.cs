using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    private Image img;
    private PlayerHealth player;
    private void Awake()
    {
        player = FindObjectOfType<PlayerHealth>();
        img = GetComponent<Image>();
    }
    private void Update()
    {
        img.fillAmount = Mathf.Lerp(img.fillAmount, player.health * 0.01f, Time.deltaTime * 10);
    }
}
