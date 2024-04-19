using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : MonoBehaviour
{   
    [SerializeField]
    private GameObject interaction; // "E" to interact

    [SerializeField]
    public GameObject signdialoguePanel; //takes in the sign's DialoguePanel for interaction checking

    [SerializeField]
    SpriteRenderer signObj; //sign object
    private bool isInteract;    //checks for interaction
    public bool playerIsClose;  //checks if the player is close to the sign
    // Start is called before the first frame update
    void Start()
    {
        interaction.SetActive(false);
        signdialoguePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose)           // only activates when the player interacts with it 
        {
            isInteract=true;   
            interaction.SetActive(false);           //interaction key will inactive
            signdialoguePanel.SetActive(true);      //Panel will active    
                     
        }
        else if (Input.GetKeyDown(KeyCode.Q) && playerIsClose)   
        {
            isInteract=true;   
            interaction.SetActive(true);           //interaction key will inactive
            signdialoguePanel.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
            interaction.SetActive(true);            //interaction key will active 
            signdialoguePanel.SetActive(false);     //Panel will be inactive until user presses the key
        }
    }
     private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            interaction.SetActive(false);
            signdialoguePanel.SetActive(false);  
        }
    }
}
