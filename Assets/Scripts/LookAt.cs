using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Vector3 mpos;
    public Unit Status;
    [SerializeField] bool isPlayer = false;
    Rigidbody rb;
    float laseringModified;
    GameManager gameManager;
    [SerializeField] protected float turnSpeed = 480f;
    [SerializeField] protected float nonPlayerModified = 0.5f;
    Quaternion rawQuaternion;
    Quaternion lookAtQuaternion;
    bool targeted = false;
    int target;
    float lerpSpeed;
    GameObject[] enemys;
    float lermDy;
    Vector3 priPos;
     public GameObject targetGameObject;
    // Update is called once per frame
    private void Start()
    {
        Status = GetComponentInParent<Unit>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameObject.AddComponent<Rigidbody>().useGravity = false;
        rb = GetComponent<Rigidbody>();
    }

    void InitialRotate(Vector3 targetLocation)
    {
        priPos = targetLocation;
        rawQuaternion = transform.rotation;
        transform.LookAt(targetLocation, Vector3.back);
        lookAtQuaternion = transform.rotation;
        transform.rotation = rawQuaternion;
        float rotateAngle = Quaternion.Angle(rawQuaternion, lookAtQuaternion);
        if(isPlayer)
            lerpSpeed = turnSpeed * laseringModified / rotateAngle;
        else
            lerpSpeed = turnSpeed * nonPlayerModified * laseringModified / rotateAngle;
        lermDy = 0f;
    }

    void Update()
    {
        if (!gameManager.isGamePaused)
        {
            if (Status != null)
            {
                laseringModified = Status.laseringModified;
                isPlayer = Status.isPlayer;
                if (isPlayer)
                {
                    mpos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                    mpos = new Vector3(mpos.x, mpos.y, 0);
                    TurnTo(mpos);
                }
                else if (Status.isFriend)
                {
                    enemys = GameObject.FindGameObjectsWithTag("Enermy");
                    if (enemys.Length > 0)
                    { 
                        if (!targeted)
                        {
                            enemys = GameObject.FindGameObjectsWithTag("Enermy");
                            target = Random.Range(0, enemys.Length);
                            targetGameObject = enemys[target];
                            targeted = true;
                        }
                        else
                        {
                            if(targetGameObject == null)
                            {
                                targeted = false;
                            }
                            else
                            {
                                TurnTo(targetGameObject.transform.position);
                            }
                        }
                    }
                }
                else
                {
                    if(GameObject.FindGameObjectsWithTag("FinalBoss").Length == 0)
                    {
                        enemys = GameObject.FindGameObjectsWithTag("Friend");
                        if (enemys.Length > 0)
                        {
                            if (!targeted)
                            {
                                enemys = GameObject.FindGameObjectsWithTag("Friend");
                                target = Random.Range(0, enemys.Length);
                                targetGameObject = enemys[target];
                                targeted = true;
                            }
                            else
                            {
                                if (targetGameObject == null)
                                {
                                    targeted = false;
                                }
                                else
                                {
                                    TurnTo(targetGameObject.transform.position);
                                }
                            }
                        }
                        else
                        {
                            if (Status.Player != null)
                                TurnTo(Status.Player.transform.position);
                        }
                    }
                    else
                    {
                        TurnTo(GameObject.FindGameObjectWithTag("FinalBoss").transform.position);
                    }
                }
                   
            }
        }
        
    }

    void TurnTo(Vector3 targetPosition)
    {
        InitialRotate(targetPosition);
        lermDy += Time.deltaTime * lerpSpeed;
        transform.rotation = Quaternion.Lerp(rawQuaternion, lookAtQuaternion, lermDy);
        if (lermDy >= 1)
        {
            transform.rotation = lookAtQuaternion;
        }
    }
}
