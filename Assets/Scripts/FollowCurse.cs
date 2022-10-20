using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCurse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mpos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        mpos = new Vector3(mpos.x, mpos.y, 0);
        transform.position = mpos;

    }
}
