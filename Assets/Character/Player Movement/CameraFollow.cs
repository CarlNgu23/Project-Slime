using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
   
    [SerializeField] public Transform target;   //player
    [SerializeField] public float smoothings;
    //[SerializeField] private Camera mainCamera;


    [SerializeField] public Vector2 minPostion;
    [SerializeField] public Vector2 maxPostion;



    // use late update to do camera follow the player
     void LateUpdate()
    {
        if (target != null) {  // dected player if died and player obj destrotyed
            if (transform.position != target.position){ 
            Vector3 targetPos = target.position;

                //to limit camera move area
                targetPos.x = Mathf.Clamp(targetPos.x, minPostion.x, maxPostion.x);
                targetPos.y = Mathf.Clamp(targetPos.y, minPostion.y, maxPostion.y);

                //this function make two postion moving  be smoothing, only change smoothings value
                transform.position = Vector3.Lerp(transform.position, targetPos, smoothings);
                

            }
        
        }
    }




    //DONT EDIT IT. FOR SET LIMIT BY OTHER CLASS.
    public void setCamposLimit(Vector2 minPos, Vector2 maxPos) 
    {
        minPostion = minPos;
        maxPostion = maxPos;
    }

}
