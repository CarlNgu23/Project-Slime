using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RWNPCDialogue : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;

    public string[] dialogue;  // contains text dialogues
    public char[] dialogueChar;
    private int index = 0;     // index on dialogues
    public int charIndex=0; 

    public GameObject endButton;    //indicator button for user to end the dialogue
    public GameObject contButton;   //indicator button for the user to press E for next dialogue
    public float wordSpeed;         // 0.05 is fast  <----->  0.1 is slow
    public bool playerIsClose;


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
           
    
        if(Input.GetKeyDown(KeyCode.Q) && playerIsClose)
        {
            if (dialoguePanel.activeInHierarchy)               
            {  
                RemoveText();       //quits the dialogue 
            }
        }

        
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose)       // when player is in range and press 'E'
        {   
            
             dialogueChar = dialogue[index].ToCharArray(); 
           
             if (!dialoguePanel.activeInHierarchy)               
            {   
                
                dialoguePanel.SetActive(true);                  //set dialoguePanel to true 

                StartCoroutine(Typing());                       //begins typing the text
               
                               
            }


           
            if (charIndex==dialogueChar.Length)    
            {  
                
                contButton.SetActive(true);
            } 

            if (dialogueText.text == dialogue[index])    
            {   
               
                NextLine();
                
            }
         
             Debug.Log(index);
    

        }
        

       

    }

    public void RemoveText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
        charIndex=0;
    }

    IEnumerator Typing()
    {  
        
        foreach(char letter in dialogueChar)
        {   
            charIndex++;
            
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