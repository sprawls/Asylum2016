using UnityEngine;
using System.Collections.Generic;



public enum ESFXType
{
    ESFXType_GAMEPLAY,
    ESFXType_PLAYER
};


public class SoundManager : MonoBehaviour {

    [Tooltip("RandomSounds")]
    [SerializeField]
    private AudioClip[] randomSounds;

    //Class members
    private static SoundManager _instance;
    public static SoundManager  Instance { get { return _instance; } }

    [SerializeField]
    private AudioSource _musicAudioSource;
    [SerializeField]
    private AudioSource _ambientAudioSource;
    [SerializeField]
    private AudioSource _gameplaySFXAudioSource;
    [SerializeField]
    private AudioSource _playerSFXAudioSource;


    [Header("Random Range for Pitch when playing random sfx.")]
    [Range(.5f,1.0f)]
    [SerializeField]
    private float minPitchSFXRange = 0.90f;

    [Range(1.0f, 1.5f)]
    [SerializeField]
    private float maxPitchSFXRange = 1.1f;

    //Class functions
    void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySingleSFX(AudioClip clip, ESFXType type)
    {
        //Resets the pitch
        AudioSource source = null;
        switch (type)
        {
            case ESFXType.ESFXType_PLAYER:
                source = _playerSFXAudioSource;
                break;

            case ESFXType.ESFXType_GAMEPLAY:
                source = _gameplaySFXAudioSource;
                break;
        }

        if(source)
        {
            source.pitch = 1.0f;
            source.clip = clip;
            source.Play();
        }
        
    }

    public void PlayRandomGameplaySFX (params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(minPitchSFXRange, maxPitchSFXRange);

        _gameplaySFXAudioSource.pitch = randomPitch;
        _gameplaySFXAudioSource.clip = clips[randomIndex];
        _gameplaySFXAudioSource.Play();
    }

    public void ChangeMusic(AudioClip musicClip)
    {
        _musicAudioSource.clip = musicClip;
        _musicAudioSource.Play();
    }

    public void PlayMusic()
    {
        _musicAudioSource.Play();
    }

    public void StopMusic()
    {
        _musicAudioSource.Stop();
    }

    public void ChangeAmbient(AudioClip ambientClip)
    {
        _ambientAudioSource.clip = ambientClip;
        _ambientAudioSource.Play();
    }

    public void PlayAmbient()
    {
        _ambientAudioSource.Play();
    }

    public void StopAmbient()
    {
        _ambientAudioSource.Stop();
    }

    public void PlayRandomSFXFromRandomSounds()
    {
        PlayRandomGameplaySFX(randomSounds);
    }
    
}