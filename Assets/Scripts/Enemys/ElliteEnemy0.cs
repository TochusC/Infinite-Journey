using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElliteEnemy0 : DefaultEnermy
{
    [SerializeField] AudioClip laserSound;
    bool islaserSounding = false;
    [SerializeField] float secondFireLastTime = 1.25f;

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
                Fire(bulletPefab[1], secondFirePlace);
                if (!isLasering)
                {
                    StartCoroutine(Lasering());
                }
            }
        }
    }

    IEnumerator Lasering()
    {
        isLasering = true;
        yield return new WaitForSeconds(secondFireLastTime);
        isSecondFireCoolDown = true;
        StartCoroutine(SecondFireCoolDown());
    }
    protected override void PlayerAttack()
    {
        if (Input.GetMouseButton(0))
        {
            PrimeAttack();
        }
        if (Input.GetMouseButton(1))
        {
            isLasering = true;
            SecondAttack();
        }
        if (Input.GetMouseButtonUp(1))
        {
            isLasering = false;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpecialSkill();
        }
    }

    protected override void SpecialSkill()
    {
        LookAt lookAt = GetComponentInChildren<LookAt>();
        if (!isSpecialSkillCoolDown)
        {
            UnitRb.AddForce(lookAt.mpos.normalized * 8f, ForceMode2D.Impulse);
            StartCoroutine(Immune());
            StartCoroutine(SpecialSkillCoolDown());
        }       
    }
    
    IEnumerator Immune()
    {
        isImmune = true;
        yield return new WaitForSeconds(3);
        isImmune = false;
    }
    IEnumerator LaserSound()
    {
        islaserSounding = true;
        yield return new WaitForSeconds(0.1f);
        unitAudioSource.PlayOneShot(laserSound, 0.2f);
        islaserSounding = false;
    }
}
