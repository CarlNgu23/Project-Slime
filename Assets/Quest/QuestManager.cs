using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

    public List<Quest> quests = new List<Quest>();
    public int maxQuests = 5;// you can only get 5 quest in the same time

    public static QuestManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);  // 
        }
        else if (Instance != this)
        {
            Destroy(Instance);  // 
        }

    }

    public void Update()
    {
        RemoveNullQuests();
    }
    public void AddQuest(Quest newQuest)
    {
        if (HasQuest(newQuest.QuestId))
        {
            Debug.Log("You already have this quest.");
            return;
        }

        if (quests.Count >= maxQuests)
        {
            Debug.Log("You have reached the maximum number of quests.");
            return;
        }

        quests.Add(newQuest);
        RemoveNullQuests();
    }

    public void UpdateQuestRequirement(string identifier, int amount)
    {
        foreach (var quest in quests)       // check all quest in the list
        {
            foreach (var req in quest.requirements) // check all quest requirement
            {
                if (req.targetIdentifier == identifier) 
                {
                    req.currentAmount += amount;
                    quest.CheckComplete();
                    if (quest.isComplete) 
                    {
                        quests.Remove(quest);
                        break;
                    }
                }
            }
        }

    }


    public bool HasQuest(int questId)
    {
        return quests.Any(q => q.QuestId == questId);
    }
    private void RemoveNullQuests()
    {
        quests = quests.Where(q => q != null).ToList();
    }
}
