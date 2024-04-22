using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestAssign : MonoBehaviour
{

    [Header("Quest")]
    public GameObject questPrefab;
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
        Quest[] existingQuests = GameObject.FindObjectsOfType<Quest>();
        bool shouldInstantiate = true;
        foreach (Quest existingQuest in existingQuests)
        {
            if (existingQuest.QuestId == questPrefab.GetComponent<Quest>().QuestId)
            {
                shouldInstantiate = false;
                Debug.Log("Existing Quest No instantiation.");
                break;
            }
        }

        Quest newQuest;

        //get the quest
        if (shouldInstantiate)
        {
            newQuest = Instantiate(questPrefab).GetComponent<Quest>();

            if (questManager.HasQuest(newQuest.QuestId))
            {
                Debug.Log("You already have this quest.");
                return;
            }

            questManager.AddQuest(newQuest);
            Debug.Log("Quest added: " + newQuest.QuestName);

        }
        //Destroy(gameObject); // deleted this object after you get a quest


    }
}
