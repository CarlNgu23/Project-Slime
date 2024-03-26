//Developed by Carl Ngu
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpManager : MonoBehaviour
{
    public static ExpManager Instance;
    public delegate void ExpRewardDelegator(int expAmount);
    public event ExpRewardDelegator OnReward;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    //When ExpRewardDelegator is called, upon reward give exp amount.
    public void GiveExp(int expAmount)
    {
        OnReward?.Invoke(expAmount);
    }
}
