using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class AudioManager : MonoBehaviour
{
    private Coroutine fadeCoroutine;

    [Header("Audio Clips")]
    public AudioClip mainMenu;
    public AudioClip buttonClick;
    public AudioClip whoosh;
    public AudioClip standardClick;
    public AudioClip tokenPickup;

    [Header("Audio Sources")]
    public AudioSource SFX;
    public AudioSource Background;
    public static AudioManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            if (Background != null)
            {
                Background.clip = mainMenu;
                Background.loop = true;
                Background.Play();   
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }   
    
    public void PlaySFX(AudioClip clip)
    {      
        SFX.PlayOneShot(clip);
    } 
    public void FadeOutBackgroundMusic(float duration)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeOutCoroutine(duration, Background));
    }   
    public void FadeOutSFX(float duration)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeOutCoroutine(duration, SFX));
    }
    private IEnumerator FadeOutCoroutine(float duration, AudioSource audioSource)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    // Method to start fading in the background music
    public void FadeInBackgroundMusic(float duration)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeInCoroutine(duration, Background, 1f));
    }
    public void FadeInSFX(float duration)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeInCoroutine(duration, SFX, 0.2f));
    }

    // Coroutine to gradually increase the volume of the background music
    private IEnumerator FadeInCoroutine(float duration, AudioSource audioSource, float volume)
    {       
        audioSource.volume = 0f;
        audioSource.Play();

        while (audioSource.volume < 1.0f)
        {
            audioSource.volume += Time.deltaTime / duration;

            yield return null;
        }

        audioSource.volume = volume;
    }
}
