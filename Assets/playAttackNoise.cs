using UnityEngine;

public class PlayAttackNoise : MonoBehaviour
{
    public AudioClip windUpSound;
    public AudioClip hitSound;
    private AudioSource audioSource;
    public float hitVolume = .5f;
    public float windupVolume = 1.0f;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayWindUpSound()
    {
        if (windUpSound != null)
        {
            audioSource.PlayOneShot(windUpSound,windupVolume);
        }
    }

    public void PlayHitSound()
    {
        if (hitSound != null)
        {
            audioSource.PlayOneShot(hitSound, hitVolume);
        }
    }
}
