using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignTrigger : MonoBehaviour
{

    [SerializeField] public GameObject dialogBox;
    [SerializeField] public TextMeshProUGUI dialogBoxText; 
    [SerializeField] public string signText;    //this text actullya show
     private bool isPlayerInSign;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && isPlayerInSign) {
            dialogBox.SetActive(true);
           // dialogBoxText.text = signText;
        }
    }


     void OnTriggerEnter2D(Collider2D other) 
    {
        //Debug.Log("Enter Sign area");
       

        if (other.gameObject.CompareTag("Player")) 
        {
            isPlayerInSign = true;
        }

    }

    private void OnTriggerExit2D(Collider2D other) 
    {

        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInSign = false;
            dialogBox.SetActive(false);
        }
    }

}
