using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CPU_Movement : MonoBehaviour
{
    public GameObject leftMonsterBoundaryGameObject;
    public GameObject rightMonsterBoundaryGameObject;
    public MonsterManager monster;
    public Rigidbody2D monster_RB2D;
    public Attack attack;
    public Detection detection;
    public Spawner spawner;
    public bool toTheRight;
    public bool toTheLeft;
    public bool isWaiting;
    public bool isIdling;
    public float RestartTime;                          //The wait time for CPU movements to restart.
    public float idleTimeMin;                          //The minimum idle time after the monster completed it's movement.
    public float idleTimeMax;                          //The maximum idle time after the monster completed it's movement.
    public float distanceBetweenBoundary;              //The distance between the monster and the boundary.
    public float newRightLimitPositionX;
    public float newLeftLimitPositionX;
    public float min_X_AxisControlForBoundaryObjects;       //The left X value that the right or left boundary can be moved.
    public float max_X_AxisControlForBoundaryObjects;       //The right X value that the right or left boundary can be moved.
    public float yAxisControlForBoundaryObjects;       //The maximum and Minimum Y value that the right or left boundary can be moved.

    // Start is called before the first frame update
    void Start()
    {
        detection = gameObject.GetComponent<Detection>();
        spawner = GameObject.Find("SpawnManager").GetComponent<Spawner>();
        monster = detection.monster;
        monster_RB2D = detection.monster_RB;
        attack = GetComponentInChildren<Attack>();
        rightMonsterBoundaryGameObject.SetActive(true);
        leftMonsterBoundaryGameObject.SetActive(false);
        toTheRight = true;
        toTheLeft = false;
        isWaiting = false;
    }

    // Update is called once per frame
    void Update()
    {
        yAxisControlForBoundaryObjects = monster_RB2D.transform.position.y;
        if (detection.isCPUMove)
        {
            Detect();
        }
        else if (!detection.isDetected)
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
        detection.isCPUMove = true;
        isWaiting = false;
    }

    public void Detect()
    {
        if (toTheRight && !attack.isAttacking_Ref && !monster.isDying_Ref && !detection.isDetected && !isIdling)
        {
            leftMonsterBoundaryGameObject.SetActive(false);
            monster_RB2D.velocity = new Vector2(detection.moveSpeed, 0f);
            Check_Right_Distance();

        }
        if (toTheLeft && !attack.isAttacking_Ref && !monster.isDying_Ref && !detection.isDetected && !isIdling)
        {
            rightMonsterBoundaryGameObject.SetActive(false);
            monster_RB2D.velocity = new Vector2(-detection.moveSpeed, 0f);
            Check_Left_Distance();
        }
    }


    private void Check_Right_Distance()
    {

        if ((Vector2.Distance(transform.position, rightMonsterBoundaryGameObject.transform.position) <= distanceBetweenBoundary && !isIdling) || (transform.position.x > rightMonsterBoundaryGameObject.transform.position.x && !isIdling))
        {
            isIdling = true;
            monster_RB2D.velocity = new Vector2(0f, 0f);
            rightMonsterBoundaryGameObject.SetActive(false);
            StartCoroutine(RightWait());
        }
    }

    IEnumerator RightWait()
    {
        yield return new WaitForSeconds(Random.Range(idleTimeMin, idleTimeMax));
        MoveLeftLimitObject();
        leftMonsterBoundaryGameObject.SetActive(true);
        isIdling = false;
        toTheRight = false;
        toTheLeft = true;
    }

    private void MoveRightLimitObject()
    {

        newRightLimitPositionX = Random.Range(transform.position.x, max_X_AxisControlForBoundaryObjects);
        rightMonsterBoundaryGameObject.transform.position = new Vector2(newRightLimitPositionX, yAxisControlForBoundaryObjects);

    }

    private void Check_Left_Distance()
    {
        if ((Vector2.Distance(transform.position, leftMonsterBoundaryGameObject.transform.position) <= distanceBetweenBoundary && !isIdling) || (transform.position.x < leftMonsterBoundaryGameObject.transform.position.x && !isIdling))
        {
            isIdling = true;
            monster_RB2D.velocity = new Vector2(0f, 0f);
            leftMonsterBoundaryGameObject.SetActive(false);
            StartCoroutine(LeftWait());
        }
    }

    IEnumerator LeftWait()
    {
        yield return new WaitForSeconds(Random.Range(idleTimeMin, idleTimeMax));
        MoveRightLimitObject();
        rightMonsterBoundaryGameObject.SetActive(true);
        isIdling = false;
        toTheLeft = false;
        toTheRight = true;
    }


    private void MoveLeftLimitObject()
    {
        newLeftLimitPositionX = Random.Range(transform.position.x, min_X_AxisControlForBoundaryObjects);
        leftMonsterBoundaryGameObject.transform.position = new Vector2(newLeftLimitPositionX, yAxisControlForBoundaryObjects);
    }
}