using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuVFX : MonoBehaviour
{
    [SerializeField] float horizontalInput;
    [SerializeField] float verticalInput;
    [SerializeField] float minView = -0.2f;
    [SerializeField] float maxView = 0.2f;
    [SerializeField] float rotateSpeed = 10f;
    void Update()
    {
        verticalInput = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * -verticalInput * Time.deltaTime * rotateSpeed);
        float rotationY = Mathf.Clamp(transform.rotation.y, minView, maxView);
        Quaternion a = new Quaternion(0, rotationY, 0, transform.rotation.w);
        transform.rotation = a;
    }
}
