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

    public GameObject endButton;    //indicator button for user to end the dialogue
    public GameObject contButton;   //indicator button for the user to press E for next dialogue
    public float wordSpeed;         // 0.05 is fast  <----->  0.1 is slow
    public bool playerIsClose;


    
    void Start()
    {
        dialogueText.text = "";
        dialoguePanel.SetActive(false);                         //by default --> dialoguePanel is not active
    }

    // Update is called once per frame
    void Update()
    {   if(Input.GetKeyDown(KeyCode.Q) && playerIsClose)
        {
            if (dialoguePanel.activeInHierarchy)               
            {  
                RemoveText();       
            }
        }
        
       
        else if (Input.GetKeyDown(KeyCode.E) && playerIsClose)       // when player is in range and press 'E'
        {   
            

             if (!dialoguePanel.activeInHierarchy)               
            {   
                
                dialoguePanel.SetActive(true);                  //set dialoguePanel to true 

                StartCoroutine(Typing());                       //begins typing the text
                
                               
            }
           
            if (dialogueText.text == dialogue[index])    
            {   
               
                NextLine();

            }
           

    

        }
        

        if(dialogueText.text == dialogue[index])                // when it reaches to the end of the sentence
        {
            contButton.SetActive(true);                         // then 'press e' will appear
        }

    }

    public void RemoveText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
    {  
        
        foreach(char letter in dialogue[index].ToCharArray() )
        {   
            

            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
            
                
           
        }
    }
        
        
    

    public void NextLine()
    {
       
        if (index < dialogue.Length-1 )
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
            contButton.SetActive(false);
            
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