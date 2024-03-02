using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class AdvanceDialogue : ScriptableObject
{
    //public DialogueActors[] actors; //allows us which actors are speaking in each line

    [Header("Random Actor Info")] //any minor characters 
    public string randomActorName;
    public Sprite randomActorImage;


    [Header("Dialogue")]
    public string[] dialogue;

    
    //public AdvanceDialogue op1;

}
