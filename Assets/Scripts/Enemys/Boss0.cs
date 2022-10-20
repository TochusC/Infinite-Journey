using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss0 : DefaultEnermy
{
    protected override void PrimeAttack()
    {
        if (!isCoolDown)
        {
            if (isPlayer)
                unitAudioSource.PlayOneShot(PrimSound, 0.18f);
            else
                unitAudioSource.PlayOneShot(PrimSound, 0.12f);
            StartCoroutine(PrimeShootMethod());
            isCoolDown = true;
            StartCoroutine(CoolDown());
        }
    }

    protected override void SecondAttack()
    {
        if (!isSecondFireCoolDown)
        {
            unitAudioSource.PlayOneShot(SecondSound, 0.3f);
            StartCoroutine(SecondShootMethod());
            StartCoroutine(SecondFireCoolDown());
            isSecondFireCoolDown = true;
        }
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

            if(health <= maxHealth * 0.5f && !isSpecialSkillCoolDown)
            {
                unitAudioSource.PlayOneShot(SpecialSound, 0.75f);
                StartCoroutine(SpecialSkillCoolDown());
                StartCoroutine(Boost());
            }
        }
    }

    IEnumerator PrimeShootMethod()
    {
        yield return new WaitForSeconds(0.2f);
        Fire(bulletPefab[0], firePlace);
        yield return new WaitForSeconds(0.1f);
        Fire(bulletPefab[0], firePlace);
    }

    IEnumerator SecondShootMethod()
    {
        yield return new WaitForSeconds(1.75f);
        Fire(bulletPefab[1], secondFirePlace);
    }
    protected override void Initialized()
    {
        isBoss = true;
    }
    void EnermyMove()
    {
        if (!gameManager.isPlayerDead)
        {
            if (isArrived)
            {
                if (isRoaming)
                {
                    moveDireciton = (randomPosition - transform.position).normalized;
                    UnitRb.AddForce(moveDireciton * roamSpeed * nonPlayerModified * Time.deltaTime * laseringModified, ForceMode2D.Impulse);
                    if (Vector3.Distance(randomPosition, transform.position) < contactRange)
                    {
                        randomPosition = (transform.position + GetRandomDirection());
                        randomPosition = PositionConstrain(randomPosition);
                    }
                }
                else
                {
                    moveDireciton = (randomPosition - transform.position).normalized;
                    UnitRb.AddForce(moveDireciton * Speed * nonPlayerModified * Time.deltaTime * laseringModified, ForceMode2D.Impulse);
                    if (Vector3.Distance(randomPosition, transform.position) < contactRange)
                    {
                        randomPosition = (Player.transform.position + GetRandomDirection());
                        randomPosition = PositionConstrain(randomPosition);
                        isRoaming = true;
                    }
                }
            }
            else
            {
                randomPosition = transform.position + GetRandomDirection();
                randomPosition = PositionConstrain(randomPosition);
                isArrived = true;
            }
        }
        else
        {
            moveDireciton = (randomPosition - transform.position).normalized;
            UnitRb.AddForce(moveDireciton * roamSpeed * nonPlayerModified * Time.deltaTime * laseringModified, ForceMode2D.Impulse);
            if (Vector3.Distance(randomPosition, transform.position) < contactRange)
            {
                randomPosition = (transform.position + GetRandomDirection());
                randomPosition = PositionConstrain(randomPosition);
            }
        }

    }

    IEnumerator Boost()
    {
        isBoosted = true;
        float rawFireDelay = fireDelay;
        float rawSecondDelay = secondFireDelay;
        fireDelay *= 0.5f;
        secondFireDelay *= 0.5f;
        yield return new WaitForSeconds(6);
        fireDelay = rawFireDelay;
        secondFireDelay = rawSecondDelay;
        isBoosted = false;
    }
}
