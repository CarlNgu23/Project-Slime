using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImagePool : MonoBehaviour
{
    [SerializeField]private GameObject afterimagePrefab;
    private Queue<GameObject> available= new Queue<GameObject>();
    public static AfterImagePool Instance{get;private set;}

    private void Awake()
    {
        Instance=this;
        GrowPool();
    }

    private void GrowPool()
    {
        for(int i=0; i<10;i++)
        {
            var instanceToAdd=Instantiate(afterimagePrefab);
            instanceToAdd.transform.SetParent(transform);
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        available.Enqueue(instance);
    }

    public GameObject GetFromPool()
    {
        if(available.Count==0)
        {
            GrowPool();
        }
        var instance=available.Dequeue();
        instance.SetActive(true);
        return instance;
    }
}
