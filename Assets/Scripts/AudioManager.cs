using System.Collections;
using System.Collections.Generic;
using PureFunctions.UnitySpecific;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : Singleton<AudioManager>
{
    private static AudioSource _backGroundAudioSource;
    private static readonly Queue<AudioSource> AudioSources = new ();
    [SerializeField] private int maximumAudioSources;
    [SerializeField] private GameObject audioSourcePrefab;
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip success;
    [SerializeField] private AudioClip failure;

    public void Initialise()
    {
        ResolveDependencies();
        AssignAllButtonsTheClickSound();
        PlayBackgroundMusic();
    }

    private void ResolveDependencies()
    {
        _backGroundAudioSource = GetComponent<AudioSource>(); //Secured by the require component attribute.
        for (var i = 0; i < maximumAudioSources; i++)
        {
            AudioSources.Enqueue(Instantiate(audioSourcePrefab, transform).GetComponent<AudioSource>());
        }
    }

    private static void AssignAllButtonsTheClickSound()
    {
        var allButtons = FindObjectsOfType<Button>();
        foreach (var button in allButtons)
        {
            //button.onClick.RemoveListener(PlayButtonClickSound); //Avoid any risk of duplicate sounds.
            button.onClick.AddListener(PlayButtonClickSound);
        }
    }
    
    private static void PlayBackgroundMusic()
    {
        _backGroundAudioSource.Play();
    }

    private static void PlayButtonClickSound()
    {
        PlayClip(Instance.buttonClick);
    }
    
    public void PlaySuccess()
    {
        PlayClip(Instance.success);
    }
    
    public void PlayFailure()
    {
        PlayClip(Instance.failure);
    }
    
    protected static void PlayClip(AudioClip clip)
    {
        var audioSource = ReturnFirstUnusedAudioSource();
        audioSource.clip = clip;
        audioSource.Play();
    }

    private static AudioSource ReturnFirstUnusedAudioSource()
    {
        var audioSource = AudioSources.Dequeue();
        AudioSources.Enqueue(audioSource);
        return audioSource;
    }


    public static void SetGlobalVolumeForPause(float volume)
    {
        AudioListener.volume = volume;
    }
}
