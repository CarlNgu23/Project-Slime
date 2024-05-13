using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
public class StartGame : MonoBehaviour
{
    private PlayableDirector director; //responsible for controlling the playback of the timeline
   
    

    void Awake()
    {
        director = GetComponent<PlayableDirector>();
        director.stopped += DirectorStopped;
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
        director.Stop(); //stops the timeline
    }

    


}
