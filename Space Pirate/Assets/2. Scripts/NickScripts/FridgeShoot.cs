using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeShoot : MonoBehaviour, IDamageable
{

    public Animator fridgeAnimator;

    public void TakeDamage(float dmg)
    {
        fridgeAnimator.SetBool("Shot", true);
        StartCoroutine(ShotTimer());

    }

    IEnumerator ShotTimer()
    {
        yield return new WaitForSeconds(10);
        fridgeAnimator.SetBool("Shot", false);
    }

}
