using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NPC will follow the Player's direction
public class NPCDialogue : MonoBehaviour
{   
    private Transform Player;
    private SpriteRenderer speechBubble;

    //checking if this works
    [SerializeField]private GameObject dialoguePanel;
    [SerializeField] private GameObject dialogueText;

    
    void Awake()
    {
        dialoguePanel.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        speechBubble=GetComponent<SpriteRenderer>();
        speechBubble.enabled=false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            //speech bubble on
            speechBubble.enabled=true;
            dialoguePanel.SetActive(true);
            Player=coll.gameObject.GetComponent<Transform>();


            
            //if player is the right side and NPC is facing left
            if(Player.position.x>transform.position.x && transform.parent.localScale.x<0) 
            {
                Flip();
            }
            else if(Player.position.x<transform.position.x && transform.parent.localScale.x>0)
            {
                Flip();
            }
            
        }
    }
    private void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            //speech bubble on
            speechBubble.enabled=false;
             dialoguePanel.SetActive(false);
            Player=coll.gameObject.GetComponent<Transform>();

            //if player is the right side and NPC is facing left
            if(Player.position.x>transform.position.x && transform.parent.localScale.x<0) 
            {
                Flip();
            }
            else if(Player.position.x<transform.position.x && transform.parent.localScale.x>0)
            {
                Flip();
            }
        }
    }

    private void Flip()
    {
        Vector3 currentScale = transform.parent.localScale;
        currentScale.x *= -1;
        transform.parent.localScale= currentScale; 
    }
}
