using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CPU_Movement : MonoBehaviour
{
    public GameObject rightLimitGameObject;
    public GameObject leftLimitGameObject;
    public Rigidbody2D rigidBody2D;
    public LayerMask rightMask;
    public LayerMask leftMask;
    public Attack attack;
    public Detection detection;
    public One_Eye_Dog One_Eye_Dog;
    public Spawner spawner;
    public bool toTheRight;
    public bool toTheLeft;
    public bool isWaiting;
    public bool isIdling;
    public float rightRayLength;
    public float leftRayLength;
    public float RestartTime;
    public float idleTimeMin;
    public float idleTimeMax;
    public float distanceBetweenLimit;
    public float newRightLimitPositionX;
    public float newLeftLimitPositionX;
    public float xAxisControlForLimitObjects;
    public float yAxisControlForLimitObjects;

    // Start is called before the first frame update
    void Start()
    {
        spawner = GameObject.Find("SpawnManager").GetComponent<Spawner>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        rightLimitGameObject.SetActive(true);
        leftLimitGameObject.SetActive(false);
        attack = GetComponentInChildren<Attack>();
        detection = gameObject.GetComponent<Detection>();
        One_Eye_Dog = gameObject.GetComponent<One_Eye_Dog>();
        toTheRight = true;
        toTheLeft = false;
        isWaiting = false;
        idleTimeMax = 7f;
        idleTimeMin = 1f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(detection.isCPUMove);
        if (detection.isCPUMove)
        {
            Detect();
        }
        else if (!detection.isDetected)
        {
            if (!isWaiting)
            {
                Debug.Log("isWAITGING");
                isWaiting = true;
                StartCoroutine(Restart());
            }
        }
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(RestartTime);
        detection.isCPUMove = true;
        isWaiting = false;
    }

    public void Detect()
    {
        if (toTheRight && !attack.isAttacking_Ref && !One_Eye_Dog.isDying_Ref && !detection.isDetected && !isIdling)
        {
            leftLimitGameObject.SetActive(false);
            rigidBody2D.velocity = new Vector2(detection.moveSpeed, 0f);
            Check_Right_Distance();

        }
        if (toTheLeft && !attack.isAttacking_Ref && !One_Eye_Dog.isDying_Ref && !detection.isDetected && !isIdling)
        {
            rightLimitGameObject.SetActive(false);
            rigidBody2D.velocity = new Vector2(-detection.moveSpeed, 0f);
            Check_Left_Distance();
        }
    }


    private void Check_Right_Distance()
    {

        if (Vector2.Distance(transform.position, rightLimitGameObject.transform.position) <= distanceBetweenLimit && !isIdling)
        {
            isIdling = true;
            rigidBody2D.velocity = new Vector2(0f, 0f);
            rightLimitGameObject.SetActive(false);
            StartCoroutine(RightWait());
        }
    }

    IEnumerator RightWait()
    {
        yield return new WaitForSeconds(Random.Range(idleTimeMin, idleTimeMax));
        MoveLeftLimitObject();
        leftLimitGameObject.SetActive(true);
        isIdling = false;
        toTheRight = false;
        toTheLeft = true;
    }

    private void MoveRightLimitObject()
    {
        newRightLimitPositionX = Random.Range(transform.position.x, xAxisControlForLimitObjects);
        rightLimitGameObject.transform.position = new Vector2(newRightLimitPositionX, yAxisControlForLimitObjects);

    }

    private void Check_Left_Distance()
    {
        if (Vector2.Distance(transform.position, leftLimitGameObject.transform.position) <= distanceBetweenLimit && !isIdling)
        {
            isIdling = true;
            rigidBody2D.velocity = new Vector2(0f, 0f);
            leftLimitGameObject.SetActive(false);
            StartCoroutine(LeftWait());
        }
    }

    IEnumerator LeftWait()
    {
        yield return new WaitForSeconds(Random.Range(idleTimeMin, idleTimeMax));
        MoveRightLimitObject();
        rightLimitGameObject.SetActive(true);
        isIdling = false;
        toTheLeft = false;
        toTheRight = false;
    }


    private void MoveLeftLimitObject()
    {
        newLeftLimitPositionX = Random.Range(transform.position.x, -xAxisControlForLimitObjects);
        leftLimitGameObject.transform.position = new Vector2(newLeftLimitPositionX, yAxisControlForLimitObjects);
    }
}
