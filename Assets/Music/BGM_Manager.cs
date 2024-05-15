using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_Manager : MonoBehaviour
{
    public static BGM_Manager Instance;
    public AudioSource bgm;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            bgm = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(Instance);
    }

    // Update is called once per frame
    void Update()
    {
        //Prevents BGM from restarting on death. Keeps the audio playing at all time.
        if (!bgm.isPlaying)
        {
            bgm.Play();
        }
    }
}
