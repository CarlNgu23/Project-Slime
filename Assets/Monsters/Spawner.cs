using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject clones_Ref;
    [SerializeField] private Vector3 pos1;
    private GameObject clone;
    // Start is called before the first frame update
    void Start()
    {
        clone = Instantiate(clones_Ref, pos1, Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
