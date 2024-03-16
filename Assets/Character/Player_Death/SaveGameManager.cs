using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SaveGameManager : MonoBehaviour, ISave
{
    public int c_Scene;
    public static SaveGameManager Instance;

    private Button saveSlotButton_1;
    private Button saveSlotButton_2;
    private Button saveSlotButton_3;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        saveSlotButton_1 = root.Q<Button>("Save_Slot_1");
        saveSlotButton_2 = root.Q<Button>("Save_Slot_2");
        saveSlotButton_3 = root.Q<Button>("Save_Slot_3");
        saveSlotButton_1.clicked += () => SaveDataToJson(Instance, "SaveData1.dat");
        saveSlotButton_2.clicked += () => SaveDataToJson(Instance, "SaveData2.dat");
        saveSlotButton_3.clicked += () => SaveDataToJson(Instance, "SaveData3.dat");
    }

    // Update is called once per frame
    void Update()
    {
        c_Scene = SceneManager.GetActiveScene().buildIndex;
    }

    private static void SaveDataToJson(SaveGameManager i_SaveGameManager, string filename)
    {
        SaveData data = new SaveData();
        i_SaveGameManager.Save_Data(data);
        FileManager.WriteToFile(filename, data.ToJSON());
        Debug.Log(filename + " Save Successfully");
    }

    public void Save_Data(SaveData saveData)
    {
        saveData.isSaved = true;
        saveData.isLoaded = true;
        saveData.s_PlayerLevel = Stats.Instance.level;
        saveData.s_currentExp= Stats.Instance.currentExp;
        saveData.s_requiredExp= Stats.Instance.requiredExp;
        saveData.s_health= Stats.Instance.health;
        saveData.s_attack = Stats.Instance.attack;
        saveData.s_defense= Stats.Instance.defense;
        saveData.s_strength= Stats.Instance.strength;
        saveData.s_dexterity= Stats.Instance.dexterity;
        saveData.s_Scene = c_Scene;
    }
}
