using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : DefaultEnermy
{
    [SerializeField] GameObject ChangeSound;
    [SerializeField] GameObject ChangeEffect;
    [SerializeField] AudioClip laserSound;
    [SerializeField] GameObject phase1;
    [SerializeField] GameObject phase2;
    [SerializeField] float secondFireLastTime = 1.25f;
    [SerializeField] bool islaserSounding = false;
    bool charging = false;
    [SerializeField] float shootDelay = 0.15f;
    protected override void PrimeAttack()
    {
        if (health < setmaxHealth * 0.80  && !isFinalBoss)
        {
            gameManager.lastTime = true;
            gameObject.tag = "FinalBoss";
            isFinalBoss = true;
            secondFireDelay *= 0.5f;
            fireDelay *= 0.35f;
            minlaseringModified = 0.75f;
            phase1.SetActive(false);
            phase2.SetActive(true);
        }
        if (!isCoolDown)
        {
            StartCoroutine(PrimeShootMethod());
            isCoolDown = true;
            StartCoroutine(CoolDown());
        }
    }

    protected override void Initialized()
    {
        lost = true;
    }
    IEnumerator PrimeShootMethod()
    {
        fireDelay = shootDelay * 5;
        for (int fireCount = 0; fireCount < firePlace.Length; fireCount++)
        {
            if ((fireCount + 1) % 5 == 0)
            {
                yield return new WaitForSeconds(0.1f);
                unitAudioSource.PlayOneShot(PrimSound, 0.3f);
            }
            myProjectile = Instantiate(bulletPefab[0], firePlace[fireCount].transform.position, firePlace[fireCount].transform.rotation);
            myProjectile.GetComponent<BulletBehavior>().shooter = gameObject;
        }
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
                randomPosition = GetRandomDirection();
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

    protected override void SecondAttack()
    {
        if (isPlayer)
        {
            if (!islaserSounding)
                StartCoroutine(LaserSound());
            Fire(bulletPefab[1], secondFirePlace);
        }
        else
        {
            if (!isSecondFireCoolDown)
            {
                if (!isLasering)
                {
                    StartCoroutine(Lasering());
                }
                else if(isLasering && !charging)
                {
                    Fire(bulletPefab[1], secondFirePlace);
                    if (!islaserSounding)
                        StartCoroutine(LaserSound());
                }
            }
        }
    }
    IEnumerator Lasering()
    {
        isLasering = true;
        charging = true;
        unitAudioSource.PlayOneShot(SecondSound, 2f);
        yield return new WaitForSeconds(2f);
        charging = false;
        yield return new WaitForSeconds(secondFireLastTime);
        isLasering = false;
        StartCoroutine(SecondFireCoolDown());
    }
    IEnumerator LaserSound()
    {
        islaserSounding = true;
        unitAudioSource.PlayOneShot(laserSound, 0.6f);
        yield return new WaitForSeconds(1.75f);
        islaserSounding = false;
    }
    protected override void SpecialSkill()
    {
        if (!isSpecialSkillCoolDown)
        {
            Fire(bulletPefab[2], specialSkillPlace);
            StartCoroutine(SpecialSkillCoolDown());
        }
    }
}
