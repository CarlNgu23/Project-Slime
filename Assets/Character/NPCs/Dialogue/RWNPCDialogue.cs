using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RWNPCDialogue : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;

    public string[] dialogue;  // contains text dialogues
    private int index = 0;     // index on dialogues
    public char[] dialogueChar;   //contains the characters of the text dialogue
    public int charIndex=0; 

    public GameObject endButton;    //indicator button for user to end the dialogue
    public GameObject contButton;   //indicator button for the user to press E for next dialogue
    public float wordSpeed;         // 0.05 is fast  <----->  0.1 is slow
    public bool playerIsClose;
    private bool isTyping = false;   //checks if its still typing

    void Awake()
    {
        contButton.SetActive(false);
    }
    void Start()
    {
        dialogueText.text = "";
        dialoguePanel.SetActive(false);                         //by default --> dialoguePanel is not active
    }

    // Update is called once per frame
    void Update()
    {  
           
    
        if(Input.GetKeyDown(KeyCode.Q) && playerIsClose && !isTyping)
        {
            if (dialoguePanel.activeInHierarchy)               
            {  
                RemoveText();       //quits the dialogue 
            }
            
        }
        
 
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose && !isTyping )       // when player is in range and press 'E' and when the dialogue isn't typing
        {  

            if(index<dialogue.Length)                               //makes sure it doesn't go out of bounds
            {
                dialogueChar = dialogue[index].ToCharArray();       //an int of an array of characters of the dialogue
            }
            
           
            
            if (!dialoguePanel.activeInHierarchy)               
            {   
               //Debug.Log("Here2");
                dialoguePanel.SetActive(true);                  //set dialoguePanel to true 

                StartCoroutine(Typing());                       //begins typing the text
                

                
            }
            
            else if (dialoguePanel.activeInHierarchy)           //if user press e again
            {   //Debug.Log("Here3");
                charIndex=0;                                    //resets the charIndex so that the its in sync with dialogue char
                NextLine();                                     //go to next line 
                
            }

            else
            {
                RemoveText();
            }
             
           // Debug.Log(index + " Update");


        }
        
       
    }

    public void RemoveText()
    {
        dialogueText.text = "";
        index = 0;
        charIndex=0;
        dialoguePanel.SetActive(false);
        
    }

    IEnumerator Typing()
    {  
        contButton.SetActive(false);                        //hides the button
        foreach(char letter in dialogueChar)
        {   
            
            isTyping=true;                                  //typing becomes true
            dialogueText.text += letter;
            charIndex++;                                    //counts the characters 
            yield return new WaitForSeconds(wordSpeed);

        }
        index++;                                            // once foreach loop ends, it goes to the next dialogue string

        contButton.SetActive(true);                         //once its finished typing then the button will appear
        isTyping=false;                                     //typing becoming false
        
       
    }

    public void NextLine()
    {
       
        if (index < dialogue.Length )
        {
            dialogueText.text = "";                         //resets the text
            StartCoroutine(Typing());                       //starts typing
            //Debug.Log("Here4");
        }

        else
        {
            RemoveText();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            RemoveText();
        }
        
    }
    
}