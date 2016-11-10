using UnityEngine;
using System.Collections.Generic;



public enum ESFXType
{
    ESFXType_GAMEPLAY,
    ESFXType_PLAYER
};


public class SoundManager : MonoBehaviour {

    [Header("AudioClips")]
    [Tooltip("RandomSounds")]
    [SerializeField]
    private AudioClip[] randomSounds;

    const int RANDOM_PITCH_SOURCES_SIZE = 5;
    private AudioSource[] _randomPitchSources = new AudioSource[RANDOM_PITCH_SOURCES_SIZE];
    private int _currentRandomSourceIndex = 0;

    //Class members
    private static SoundManager _instance;
    public static SoundManager  Instance { get { return _instance; } }

    [Header("AudioSources")]
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

        for(int i=0; i< RANDOM_PITCH_SOURCES_SIZE; ++i)
        {
            _randomPitchSources[i] = gameObject.AddComponent<AudioSource>();
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
            source.PlayOneShot(clip);
        }
        
    }

    public void PlaySingleSFXWithRandomPitch(AudioClip clip, ESFXType type)
    {
        float randomPitch = Random.Range(minPitchSFXRange, maxPitchSFXRange);

        _randomPitchSources[_currentRandomSourceIndex].pitch = randomPitch;
        _randomPitchSources[_currentRandomSourceIndex].clip = clip;
        _randomPitchSources[_currentRandomSourceIndex].Play();

        _currentRandomSourceIndex = (_currentRandomSourceIndex + 1) % RANDOM_PITCH_SOURCES_SIZE;
    }

    public void PlayRandomGameplaySFX (params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(minPitchSFXRange, maxPitchSFXRange);

        _randomPitchSources[_currentRandomSourceIndex].pitch = randomPitch;
        _randomPitchSources[_currentRandomSourceIndex].clip = clips[randomIndex];
        _randomPitchSources[_currentRandomSourceIndex].Play();

        _currentRandomSourceIndex = (_currentRandomSourceIndex + 1) % RANDOM_PITCH_SOURCES_SIZE;
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