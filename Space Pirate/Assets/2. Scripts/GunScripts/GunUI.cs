using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunUI : MonoBehaviour
{
    [SerializeField] private Text magCapacityText;
    [SerializeField] private Text currentAmmoText;
    [SerializeField] private GameObject pistolIndicator;
    [SerializeField] private GameObject shotgunIndicator;
    private Gun gun;

    private void Awake()
    {
        gun = FindObjectOfType<Gun>();
    }
    private void Update()
    {
        magCapacityText.text = gun.currentGun.magCapacity.ToString();
        currentAmmoText.text = gun.currentGun.currentAmmo.ToString();

        switch (gun.currentGun.name)
        {
            case "Pistol":

                shotgunIndicator.SetActive(false);
                pistolIndicator.SetActive(true);
                break;
            case "Shotgun":
                shotgunIndicator.SetActive(true);
                pistolIndicator.SetActive(false);
                break;
        }
    }
}
