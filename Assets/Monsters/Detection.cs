using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Detection : MonoBehaviour
{
    [SerializeField] private float rayDistance;
    [SerializeField] private float limit_distance;
    [SerializeField] LayerMask playerMask;
    [SerializeField] private GameObject player;
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool isFacingRight;

    private Rigidbody2D oneEyeDog_RB;

    // Start is called before the first frame update
    void Start()
    {
        oneEyeDog_RB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (oneEyeDog_RB.velocity.x > 0f && !isFacingRight)      //Checks for contradictions in monster's right direction
        {
            Flip();
        }
        else if (oneEyeDog_RB.velocity.x < 0f && isFacingRight)  //Checks for contradictions in monster's left direction
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        Detect();
    }

    private void Detect()
    {
        RaycastHit2D right_hit = Physics2D.Raycast(transform.position, Vector2.right, rayDistance, playerMask);
        Debug.DrawRay(right_hit.point, right_hit.normal, Color.red);
        RaycastHit2D left_hit = Physics2D.Raycast(transform.position, -Vector2.right, rayDistance, playerMask);
        Debug.DrawRay(left_hit.point, left_hit.normal, Color.red);

        if (right_hit)
        {
            oneEyeDog_RB.velocity = new Vector2(moveSpeed, 0f);
            Check_Distance();
            if (!isFacingRight)      //Checks for contradictions in monster's right direction
            {
                Flip();
            }
        }
        else if (left_hit)
        {
            oneEyeDog_RB.velocity = new Vector2(-moveSpeed, 0f);
            Check_Distance();
            if (isFacingRight)  //Checks for contradictions in monster's left direction
            {
                Flip();
            }
        }
        else
        {
            oneEyeDog_RB.velocity = new Vector2(0f, 0f);
        }
    }

    private void Check_Distance()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < limit_distance)
        {
            oneEyeDog_RB.velocity = new Vector2(0f, 0f);
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
}
