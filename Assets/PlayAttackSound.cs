using UnityEngine;

public class AttackSoundEffects : MonoBehaviour
{
    public AudioClip windUpSound;
    public AudioClip hitSound;
    private AudioSource audioSource;
    public float hitVolume = 1.0f;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayWindUpSound()
    {
        if (windUpSound != null)
        {
            audioSource.PlayOneShot(windUpSound);
        }
    }

    public void PlayHitSound()
    {
        if (hitSound != null)
        {
            audioSource.PlayOneShot(hitSound,hitVolume);
        }
    }
}
