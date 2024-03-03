using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    public int s_PlayerLevel;
    public int s_currentExp;
    public int s_requiredExp;
    public int s_health;
    public int s_attack;
    public int s_defense;
    public int s_strength;
    public int s_dexterity;
    public int s_Scene;

    public bool isSaved;
    public bool isLoaded;

    public string ToJSON()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string load_data)
    {
        JsonUtility.FromJsonOverwrite(load_data, this);
    }
}

public interface ISave
{
    void Save_Data(SaveData save_data);
}

public interface ILoad
{
    void Load_Data(SaveData load_data);
}
