using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehavior : MonoBehaviour
{
    public GameObject shooter;
    public bool doNotDestoryOnHit = false;
    [SerializeField] bool willSelfDestory = true;
    [SerializeField] Material playerMaterial;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Material enemyMaterial;
    [SerializeField] float bulletSpeed = 20f;
    [SerializeField] float nonPlayerModified = 0.5f;
    [SerializeField] float DestoryTime = 2f;
    [SerializeField] public float bulletDamage = 5;
    // Start is called before the first frame update
    void Start()
    {
        if (willSelfDestory)
            StartCoroutine(SelfDestory());

    }
    void Update()
    {
        if (shooter == null)
        {
            Destroy(gameObject);
        }
        else
        {
            if (shooter.CompareTag("Enermy"))
            {
                transform.Translate(Vector3.left * bulletSpeed * Time.deltaTime * nonPlayerModified);
                meshRenderer.material = enemyMaterial;
            }
            else
            {
                transform.Translate(Vector3.left * bulletSpeed * Time.deltaTime);
                meshRenderer.material = playerMaterial;
            }
        }
    }

    private IEnumerator SelfDestory()
    {
        yield return new WaitForSeconds(DestoryTime);
        Destroy(gameObject);
    }
}
