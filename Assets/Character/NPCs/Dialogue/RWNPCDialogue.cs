//Created by Jady
using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;


public class RWNPCDialogue : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;

    public string[] dialogue;  // contains text dialogues
    private int index = 0;     // index on dialogues
    public char[] dialogueChar;   //contains the characters of the text dialogue
    
    public GameObject endButton;    //indicator button for user to end the dialogue
    public GameObject contButton;   //indicator button for the user to press E for next dialogue
    private float wordSpeed;         // 0.05 is fast  <----->  0.1 is slow
    public float slowDialogueDelay;
    public float fastDialogueDelay;
    public bool playerIsClose;
    private bool isTyping = false;   //checks if its still typing

    
    [Header("Quest")]
    public GameObject questPrefab;
    public QuestManager questManager;


    void Awake()
    {
        contButton.SetActive(false);
        questManager = FindObjectOfType<QuestManager>();
        if (questManager == null)
        {
            Debug.LogError("Failed to find the QuestManager.");
        }
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
                if (dialogue[index].Substring(0, 5) == "slow:")//Detect dialogues that needs to be slower.
                {
                    dialogueChar = dialogue[index].Substring(5, dialogue[index].Length - 5).ToCharArray();
                    wordSpeed= slowDialogueDelay;
                }
                else
                {
                    dialogueChar = dialogue[index].ToCharArray();       //an int of an array of characters of the dialogue
                    wordSpeed= fastDialogueDelay;
                }
           }

            if (!dialoguePanel.activeInHierarchy)               
            {   
                dialoguePanel.SetActive(true);                  //set dialoguePanel to true 
                StartCoroutine(Typing());                       //begins typing the text
            }
            else if (dialoguePanel.activeInHierarchy)           //if user press e again
            {
                NextLine();                                     //go to next line 
            }
            else
            {
                RemoveText();
            }

            //get the quest
            Quest newQuest = Instantiate(questPrefab).GetComponent<Quest>();
            Destroy(gameObject);

            if (questManager.HasQuest(newQuest.QuestId))
            {
                Debug.Log("You already have this quest.");
                return;
            }

            questManager.AddQuest(newQuest);
                Debug.Log("Quest added: " + newQuest.QuestName);           

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
        contButton.SetActive(false);                        //hides the button
        foreach(char letter in dialogueChar)
        {   
            isTyping=true;                                  //typing becomes true
            dialogueText.text += letter;
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