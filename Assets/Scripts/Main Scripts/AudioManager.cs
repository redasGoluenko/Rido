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
        fadeCoroutine = StartCoroutine(FadeOutCoroutine(duration));
    }   
    private IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume = Background.volume;

        while (Background.volume > 0)
        {
            Background.volume -= startVolume * Time.deltaTime / duration;

            yield return null;
        }

        Background.Stop();
        Background.volume = startVolume;
    }

    // Method to start fading in the background music
    public void FadeInBackgroundMusic(float duration)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeInCoroutine(duration));
    }

    // Coroutine to gradually increase the volume of the background music
    private IEnumerator FadeInCoroutine(float duration)
    {       
        Background.volume = 0f;
        Background.Play();

        while (Background.volume < 1.0f)
        {
            Background.volume += Time.deltaTime / duration;

            yield return null;
        }

        Background.volume = 1.0f; // Ensure volume is set to max after fading in
    }
}
