using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 10f;
    Unit stauts;
    void Start()
    {
        stauts = GetComponentInParent<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        if(stauts.isFinalBoss)
            transform.Rotate(Vector3.forward, rotateSpeed * stauts.laseringModified * Time.deltaTime * 1.5f);
        transform.Rotate(Vector3.forward, rotateSpeed * stauts.laseringModified * Time.deltaTime);
    }
}
