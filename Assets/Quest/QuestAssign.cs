using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class QuestAssign : MonoBehaviour
{

    [Header("Quest")]
    public QuestManager questManager;


    void Awake()
    {
        questManager = FindObjectOfType<QuestManager>();
        if (questManager == null)
        {
            Debug.LogError("Failed to find the QuestManager.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AssignQuest()
    {
        //check all existing quest that has a name
        Quest newQuest;

        //get the quest
            newQuest = GetComponent<Quest>();

            if (questManager.HasQuest(newQuest.QuestId))
            {
                Debug.Log("You already have this quest.");
                return;
            }

            questManager.AddQuest(newQuest);
            Debug.Log("Quest added: " + newQuest.QuestName);

        
        //Destroy(gameObject); // deleted this object after you get a quest


    }
}
