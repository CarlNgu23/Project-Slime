using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Quest : MonoBehaviour
{
    public int QuestId;
    public string QuestName;
    public string QuestDescription;
    public bool isComplete = false;
    public int rewardExp;
    public bool repeatable = false;
    //public int reward other 
    public QuestRequirement[] requirements;

    public ExpManager expManager;

    internal IEnumerable<object> requirement;

    public void CheckComplete()
    {
        isComplete = requirements.All(r => r.IsSatisfied());

        if (isComplete)
        {
            expManager.GiveExp(rewardExp);
            Debug.Log("quest done");
            if (repeatable)
            {
                repeat();   //make it repeat
            }
            else
            {
               // Destroy(gameObject);    //destroy object include dialog
            }
        }
    }

     void Awake()
    {
        expManager = GameObject.Find("ExpManager").GetComponent<ExpManager>();
       
    }

    private void repeat()
    {
        isComplete = false;
        foreach (QuestRequirement requirement in requirements)
        {
            requirement.currentAmount = 0;
        }
        //Reward decreases after repeating 
        if (rewardExp >= 0)
        {
            rewardExp -= 1;
        }
    }
}

[System.Serializable]
public class QuestRequirement
{
    public enum RequirementType { Collect, Defeat, Reach }
    public RequirementType type;
    public string targetIdentifier;
    public int requiredAmount;
    public int currentAmount;

    public bool IsSatisfied()
    {
        return currentAmount >= requiredAmount;
    }
}
