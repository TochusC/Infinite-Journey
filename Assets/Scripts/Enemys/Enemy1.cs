using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : DefaultEnermy
{
    protected override void PrimeAttack()
    {
        if (!isCoolDown)
        {
            if (isPlayer)
                unitAudioSource.PlayOneShot(PrimSound, 0.13f);
            StartCoroutine(PrimeShootMethod());
            isCoolDown = true;
            StartCoroutine(CoolDown());
        }
    }

    IEnumerator PrimeShootMethod()
    {
        yield return new WaitForSeconds(0.175f);
        Fire(bulletPefab[0], firePlace);
        yield return new WaitForSeconds(0.1f);
        Fire(bulletPefab[0], firePlace);
    }
}
