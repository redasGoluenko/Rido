using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip tokenPickupSound;   
    public AudioClip tokenExitSound;
   
    private AudioSource SFX;
    public static AudioManager Instance { get; private set; }


    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SFX = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayTokenPickupSound()
    {
        SFX.PlayOneShot(tokenPickupSound);
    }  
    public void PlayTokenExitSound() {
        SFX.PlayOneShot(tokenExitSound);
    }
}
