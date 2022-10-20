using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEnermy : Unit
{
    protected override void AttackPlayer()
    {
        EnermyMove();
    }

    IEnumerator Active()
    {
        yield return new WaitForSeconds(Random.Range(AttackDelay, AttackDelay + 2));
        isRest = false;
    }

    protected override void PrimeAttack()
    {
        if (!isCoolDown)
        {
            if(isPlayer)
                unitAudioSource.PlayOneShot(PrimSound, 0.18f);
            Fire(bulletPefab[0], firePlace);
            isCoolDown = true;
            StartCoroutine(CoolDown());
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
                    if (Vector3.Distance(randomPosition, transform.position) < contactRange )
                    {
                        randomPosition = (transform.position + GetRandomDirection());
                        randomPosition = PositionConstrain(randomPosition);
                    }
                }
                else
                {
                    moveDireciton = (randomPosition - transform.position).normalized;
                    UnitRb.AddForce(moveDireciton * Speed * nonPlayerModified * Time.deltaTime * laseringModified, ForceMode2D.Impulse);
                    if (Vector3.Distance(randomPosition, transform.position) < contactRange )
                    {
                        randomPosition = (Player.transform.position + GetRandomDirection());
                        randomPosition = PositionConstrain(randomPosition);
                        isRoaming = true;
                    }
                }
            }
            else
            {
                randomPosition = Player.transform.position + GetRandomDirection();
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
}

