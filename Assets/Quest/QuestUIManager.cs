using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class NewBehaviourScript : MonoBehaviour
{

    public QuestManager questManager;
    public Text questListMainText;
    public Text QuestListText;

    public GameObject Panel;
    public GameObject textGroup;

    public int questItemHeight = 50;
    public RectTransform panelRect;

    // Start is called before the first frame update
    void Start()
    {
        questManager = FindObjectOfType<QuestManager>();
        if (questManager == null)
        {
            Debug.LogError("QuestManager not found.");
        }
        UpdateQuestListUI();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateQuestListUI();
    }


    public void UpdateQuestListUI()
    {
        // reset
        questListMainText.text = "";
        QuestListText.text = "";

        Panel.SetActive(true);
        textGroup.SetActive(true);

        List<Quest> quests = questManager.quests;
        if (quests == null || quests.Count == 0)
        {
            Panel.SetActive(false);
            textGroup.SetActive(false);
            return;
        }
          
        questListMainText.text = "Quests:";

        AdjustHeight(questManager.quests.Count);

        string text = "";
        foreach (Quest quest in quests) //to show each quest name and requirement
        {
            text += " " + quest.QuestName + ":\n        " + quest.requirements[0].targetIdentifier + "  " + quest.requirements[0].currentAmount +
                " / "+ quest.requirements[0].requiredAmount +  "\n\n";
        }

        QuestListText.text = text;

    }

    public void AdjustHeight(int questCount)    //to AdjustHeight base on number of quest
    {
         //set total height
        int totalHeight = questCount * questItemHeight ;

        panelRect.sizeDelta = new Vector2(panelRect.sizeDelta.x, totalHeight);
    }
   }
