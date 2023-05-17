using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeShoot : MonoBehaviour, IDamageable
{

    public Animator fridgeAnimator;
    bool activated;

    public void TakeDamage(float dmg)
    {
        if (!activated)
        {
            fridgeAnimator.SetBool("Shot", true);
            StartCoroutine(ShotTimer());
            activated = true;
        }
    }
    IEnumerator ShotTimer()
    {
        yield return new WaitForSeconds(10);
        fridgeAnimator.SetBool("Shot", false);
        activated = false;
    }

}
