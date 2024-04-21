using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Quest : MonoBehaviour
{
    public int QuestId;
    public string QuestName;
    public string QuestDescription;
    public bool isComplete = false;
    public int rewardExp;
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
            Destroy(gameObject);
        }
    }

     void Awake()
    {
        expManager = GameObject.Find("ExpManager").GetComponent<ExpManager>();
       
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
