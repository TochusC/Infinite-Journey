using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public GameObject shooter;
    public bool doNotDestoryOnHit = false;
    [SerializeField] public float pushForce = 10f;
    [SerializeField] LookAt lookAt;
    [SerializeField] GameObject target;
    [SerializeField] bool willSelfDestory = true;
    [SerializeField] Material playerMaterial;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Material enemyMaterial;
    [SerializeField] Material FinalBossMaterial;
    [SerializeField] float bulletSpeed = 20f;
    [SerializeField] float nonPlayerModified = 0.5f;
    [SerializeField] float DestoryTime = 2f;
    bool push = true;
    [SerializeField] public float bulletDamage = 5;
    private void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        if(willSelfDestory)
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
            if(gameObject.tag == "Bullet" || gameObject.tag == "Laser")
            {
                if (shooter.CompareTag("Enermy"))
                {
                    transform.Translate(Vector3.left * bulletSpeed * Time.deltaTime * nonPlayerModified);
                    meshRenderer.material = enemyMaterial;
                }
                else if (shooter.CompareTag("FinalBoss"))
                {
                    transform.Translate(Vector3.left * bulletSpeed * Time.deltaTime * nonPlayerModified);
                    meshRenderer.material = FinalBossMaterial;
                }
                else
                {
                    transform.Translate(Vector3.left * bulletSpeed * Time.deltaTime);
                    meshRenderer.material = playerMaterial;
                }
            }
            else if(gameObject.tag == "Shockwave")
            {
                if(push)
                    transform.localScale = new Vector3(transform.localScale.x +( bulletSpeed * Time.deltaTime), transform.localScale.y + (bulletSpeed * Time.deltaTime), 1);
                else
                    transform.localScale = new Vector3(transform.localScale.x - (bulletSpeed * Time.deltaTime), transform.localScale.y - (bulletSpeed * Time.deltaTime), 1);
            }
            else if(gameObject.tag == "Missle")
            {

            }
        }
    }

    private IEnumerator SelfDestory()
    {
        yield return new WaitForSeconds(DestoryTime/2);
        push = false;
        yield return new WaitForSeconds(DestoryTime / 2);
        Destroy(gameObject);
    }
}
