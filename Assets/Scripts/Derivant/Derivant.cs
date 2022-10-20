using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Derivant : DefaultEnermy
{
    public GameObject owner;
    Unit ownerStatus;
    LookAt lookAtStatus;
    GameObject target;

    protected override void Initialized()
    {
        lookAtStatus = GetComponentInChildren<LookAt>();
        ownerStatus = owner.GetComponent<Unit>();
        if (ownerStatus.isPlayer)
        {
            gameObject.tag = "Friend";
            isFriend = true;
        }
    }

    protected override void AllyMove()
    {
        if(owner == null)
        {
            Instantiate(destroyedEffect);
            Destroy(gameObject);
        }
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
            randomPosition = Player.transform.position + GetRandomDirection();
            randomPosition = PositionConstrain(randomPosition);
            isArrived = true;
        }
    }

    protected override void AllyAttack()
    {
        target = lookAtStatus.targetGameObject;
        if (target != null)
        {
            PrimeAttack();
            SecondAttack();
            SpecialSkill();
        }
    }
}
