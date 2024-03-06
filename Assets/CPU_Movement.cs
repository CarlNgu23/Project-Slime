using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPU_Movement : MonoBehaviour
{
    public GameObject rightLimitGameObject;
    public GameObject leftLimitGameObject;
    public Rigidbody2D rigidBody2D;
    public LayerMask rightMask;
    public LayerMask leftMask;
    public float rightRayLength;
    public float leftRayLength;
    public float distanceBetweenLimit;
    public float RestartTime;
    private bool isWaiting;

    public float newRightLimitPosition;
    public float newLeftLimitPosition;
    public float yAxisControlForLimitObjects;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        Detection.isCPUMove = true;
        isWaiting = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Detection.isCPUMove)
        {
            Detect();
        }
        else if (!Detection.isDetected)
        {
            if (!isWaiting)
            {
                isWaiting = true;
                StartCoroutine(Restart());
            }
        }
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(RestartTime);
        Detection.isCPUMove = true;
        isWaiting = false;
    }

    public void Detect()
    {
        RaycastHit2D right_hit = Physics2D.Raycast(transform.position, Vector2.right, rightRayLength, rightMask);
        Debug.DrawRay(right_hit.point, right_hit.normal, Color.blue);
        RaycastHit2D left_hit = Physics2D.Raycast(transform.position, -Vector2.right, leftRayLength, leftMask);
        Debug.DrawRay(left_hit.point, left_hit.normal, Color.blue);
        if (right_hit && !Attack.isAttacking_Ref && !One_Eye_Dog.isDying_Ref && !Detection.isDetected)
        {
            leftLimitGameObject.SetActive(false);
            rigidBody2D.velocity = new Vector2(Detection.moveSpeed_Ref, 0f);
            Check_Right_Distance();
        }
        if (left_hit && !Attack.isAttacking_Ref && !One_Eye_Dog.isDying_Ref && !Detection.isDetected)
        {
            rightLimitGameObject.SetActive(false);
            rigidBody2D.velocity = new Vector2(-Detection.moveSpeed_Ref, 0f);
            Check_Left_Distance();
        }
    }


    private void Check_Right_Distance()
    {
        if (Vector2.Distance(transform.position, rightLimitGameObject.transform.position) <= distanceBetweenLimit)
        {
            rigidBody2D.velocity = new Vector2(0f, 0f);
            rightLimitGameObject.SetActive(false);
            leftLimitGameObject.SetActive(true);
            MoveLeftLimitObject();
        }
    }

    private void Check_Left_Distance()
    {
        if (Vector2.Distance(transform.position, leftLimitGameObject.transform.position) <= distanceBetweenLimit)
        {
            rigidBody2D.velocity = new Vector2(0f, 0f);
            leftLimitGameObject.SetActive(false);
            rightLimitGameObject.SetActive(true);
            MoveRightLimitObject();
        }
    }
    private void MoveRightLimitObject()
    {
        newRightLimitPosition = Random.Range(transform.position.x, 14.0f);
        rightLimitGameObject.transform.position = new Vector2(newRightLimitPosition, yAxisControlForLimitObjects);
    }

    private void MoveLeftLimitObject()
    {
        newLeftLimitPosition = Random.Range(transform.position.x, -14.0f);
        leftLimitGameObject.transform.position = new Vector2(newLeftLimitPosition, yAxisControlForLimitObjects);
    }
}
