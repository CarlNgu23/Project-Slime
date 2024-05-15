using UnityEngine;

public class PlayRandomLandingSound : MonoBehaviour
{
    public AudioClip[] landingSounds; // Array to hold the sound clips
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomSound()
    {
        if (landingSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, landingSounds.Length);
            audioSource.clip = landingSounds[randomIndex];
            audioSource.Play();
        }
    }
}
