using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
public class StartGame : MonoBehaviour
{
    private PlayableDirector director; //responsible for controlling the playback of the timeline
    private bool isPaused = false;
    

    void Awake()
    {
        director = GetComponent<PlayableDirector>();
        director.stopped += DirectorStopped;
    }

    void Update()
    {   
         if (director == null)
        {
             Debug.LogError("PlayableDirector component not found");
        }
        else
        {
            if (isPaused && Input.GetMouseButtonDown(0))
            {
                ResumeTimeline();
            }
        }

       
    }
   
    
    private void DirectorStopped(PlayableDirector obj) //event handler, this function is called when timeline is stopped
    {
      
        SceneManager.LoadScene(2);//runs the game once the timeline stops
    }

    public void StartTimeline()
    {
        director.Play(); //plays the timeline
        
    }

    public void StopTimeline()
    {
        director.Stop(); //terminates the timeline
    }

    public void ResumeTimeline()
    {
        director.Resume();
        isPaused=false;
    }
    public void PauseTimeline()
    {
        director.Pause();
        isPaused=true;
    }
    


}
