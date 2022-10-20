using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    MeshRenderer meshRenderer;
    [SerializeField] Material playerMatetial;
    [SerializeField] Material enemyMaterial;
    [SerializeField] Material immuneMaterial;
    [SerializeField] Material boostedMaterial;
    [SerializeField] Material friendMaterial;
    [SerializeField] Material finalBossMaterial;
    [SerializeField] Unit unitStatus;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        unitStatus = GetComponentInParent<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        if (unitStatus.isImmune)
            meshRenderer.material = immuneMaterial;
        else if (unitStatus.isBoosted)
            meshRenderer.material = boostedMaterial;
        else
        {
            if (unitStatus.isFriend)
                meshRenderer.material = friendMaterial;
            else if (unitStatus.isPlayer)
                meshRenderer.material = playerMatetial;
            else if (unitStatus.isFinalBoss)
                meshRenderer.material = finalBossMaterial;
            else
                meshRenderer.material = enemyMaterial;
        }
    }
}
