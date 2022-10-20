using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElliteEnemy1 : DefaultEnermy
{
    protected override void PrimeAttack()
    {
        if (!isCoolDown)
        {
            if (isPlayer)
                unitAudioSource.PlayOneShot(PrimSound, 0.13f);
            Fire(bulletPefab[0], firePlace);
            isCoolDown = true;
            StartCoroutine(CoolDown());
        }
    }

    IEnumerator Boost()
    {
        isBoosted = true;
        float rawFireDelay = fireDelay;
        float rawSecondDelay = secondFireDelay;
        secondFireDelay *= 0.5f;
        fireDelay *= 0.8f;
        yield return new WaitForSeconds(4);
        fireDelay = rawFireDelay;
        secondFireDelay = rawSecondDelay;
        isBoosted = false;
    }
    protected override void SpecialSkill()
    {
        if (isPlayer)
        {
            if (!isSpecialSkillCoolDown)
            {
                unitAudioSource.PlayOneShot(SpecialSound, 0.75f);
                StartCoroutine(SpecialSkillCoolDown());
                StartCoroutine(Boost());
            }
        }
        else
        {
            if (health <= maxHealth * 0.5f && !isSpecialSkillCoolDown)
            {
                StartCoroutine(SpecialSkillCoolDown());
                StartCoroutine(Boost());
            }
        }
    }

    protected override void SecondAttack()
    {
        if(!isSecondFireCoolDown)
        {
            if(isPlayer)
                unitAudioSource.PlayOneShot(SecondSound, 0.35f);
            for (int cnt = 0; cnt < secondFirePlace.Length; cnt++)
            {
                myProjectile = Instantiate(bulletPefab[1], secondFirePlace[cnt].transform.position, secondFirePlace[cnt].transform.rotation);
                myProjectile.GetComponent<Derivant>().owner = gameObject;
            }
            StartCoroutine(SecondFireCoolDown());
        }
    }
}
