using UnityEngine;

public class ParrySoundFX : MonoBehaviour
{
    public AudioClip parrySound;
    private AudioSource audioSource;
    public float hitVolume = 1.0f;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayParry()
    {
        if (parrySound != null)
        {
            audioSource.PlayOneShot(parrySound, 1);
        }
    }
}
