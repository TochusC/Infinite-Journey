using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] float DestroyTime = 10f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SelfDestory());
    }
    private IEnumerator SelfDestory()
    {
        yield return new WaitForSeconds(DestroyTime);
        Destroy(gameObject);
    }
}
