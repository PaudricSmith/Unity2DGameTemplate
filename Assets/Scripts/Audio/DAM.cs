using System;
using System.Collections;
using UnityEngine;


/// <summary>
/// DynamicAudioManager (DAM) is responsible for managing all audio-related functionalities.
/// </summary>
public class DAM : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of DynamicAudioManager.
    /// </summary>
    public static DAM One;

    [SerializeField] private AudioClipsSO audioClipsSO;
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    /// <summary>
    /// Initializes the singleton instance.
    /// </summary>
    private void Awake()
    {
        if (One == null)
        {
            One = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (One != this)
        {
            Destroy(gameObject);
            return;
        }
    }


    #region Playback Control

    /// <summary>
    /// Plays music based on the provided track type.
    /// </summary>
    /// <typeparam name="T">Type of the track.</typeparam>
    /// <param name="track">Track to play.</param>
    public void PlayMusic<T>(T track)
    {
        AudioClip clip = audioClipsSO.GetClip(track);
        if (clip)
        {
            musicAudioSource.clip = clip;
            musicAudioSource.Play();
        }
    }

    /// <summary>
    /// Plays sound effects based on the provided track type.
    /// </summary>
    /// <typeparam name="T">Type of the track.</typeparam>
    /// <param name="track">Track to play.</param>
    public void PlaySFX<T>(T track)
    {
        AudioClip clip = audioClipsSO.GetClip(track);
        if (clip)
        {
            sfxAudioSource.PlayOneShot(clip);
        }
    }

    public void PauseMusic()
    {
        if (musicAudioSource.isPlaying)
        {
            musicAudioSource.Pause();
        }
    }

    public void ResumeMusic()
    {
        if (!musicAudioSource.isPlaying)
        {
            musicAudioSource.Play();
        }
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }

    #endregion


    #region Volume Control

    public void SetMusicVolume(float volume)
    {
        musicAudioSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxAudioSource.volume = volume;
    }

    public float GetMusicVolume()
    {
        return musicAudioSource.volume;
    }

    public float GetSFXVolume()
    {
        return sfxAudioSource.volume;
    }

    #endregion


    #region AudioControlMethods

    public void SetMusicLooping(bool shouldLoop)
    {
        musicAudioSource.loop = shouldLoop;
    }

    public void MuteMusic()
    {
        musicAudioSource.mute = true;
    }

    public void UnmuteMusic()
    {
        musicAudioSource.mute = false;
    }

    public void SetMusicPitch(float pitch)
    {
        musicAudioSource.pitch = pitch;
    }

    public void SetSFXPitch(float pitch)
    {
        sfxAudioSource.pitch = pitch;
    }

    public void SetAudio3DSpatialBlend(float spatialBlend)
    {
        musicAudioSource.spatialBlend = spatialBlend;
        sfxAudioSource.spatialBlend = spatialBlend;
    }

    #endregion


    #region Fade Control

    /// <summary>
    /// Fades in music over a specified duration.
    /// </summary>
    /// <typeparam name="T">Type of the track.</typeparam>
    /// <param name="trackType">Track to fade in.</param>
    /// <param name="duration">Duration of the fade-in effect.</param>
    public void FadeInMusic<T>(T trackType, float duration)
    {
        AudioClip clip = audioClipsSO.GetClip(trackType);
        if (clip != null)
        {
            StartCoroutine(FadeInProcess(musicAudioSource, clip, duration));
        }
    }

    /// <summary>
    /// Fades in sound effects over a specified duration.
    /// </summary>
    /// <typeparam name="T">Type of the track.</typeparam>
    /// <param name="trackType">Track to fade in.</param>
    /// <param name="duration">Duration of the fade-in effect.</param>
    public void FadeInSFX<T>(T trackType, float duration)
    {
        AudioClip clip = audioClipsSO.GetClip(trackType);
        if (clip != null)
        {
            StartCoroutine(FadeInProcess(sfxAudioSource, clip, duration));
        }
    }

    /// <summary>
    /// Fades out music over a specified duration.
    /// </summary>
    /// <param name="duration">Duration of the fade-out effect.</param>
    public void FadeOutMusic(float duration)
    {
        StartCoroutine(FadeOutProcess(musicAudioSource, duration));
    }

    /// <summary>
    /// Fades out sound effects over a specified duration.
    /// </summary>
    /// <param name="duration">Duration of the fade-out effect.</param>
    public void FadeOutSFX(float duration)
    {
        StartCoroutine(FadeOutProcess(sfxAudioSource, duration));
    }

    /// <summary>
    /// Transitions between two tracks with a fade-out and fade-in effect.
    /// </summary>
    /// <param name="fromTrack">Track to fade out.</param>
    /// <param name="toTrack">Track to fade in.</param>
    /// <param name="duration">Duration of the transition.</param>
    public void TransitionTracks(Enum fromTrack, Enum toTrack, float duration)
    {
        StartCoroutine(FadeOutProcess(musicAudioSource, duration, () =>
        {
            FadeInMusic(toTrack, duration);
        }));
    }

    #endregion


    #region Coroutines

    private IEnumerator FadeInProcess(AudioSource audioSource, AudioClip newClip, float duration)
    {
        if (newClip)
        {
            audioSource.clip = newClip;
            audioSource.Play();
        }

        audioSource.volume = 0;
        float targetVolume = 1.0f;
        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += targetVolume * Time.deltaTime / duration;
            yield return null;
        }
    }

    private IEnumerator FadeOutProcess(AudioSource audioSource, float duration, Action onFinished = null)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; 

        onFinished?.Invoke();
    }

    private IEnumerator CrossFadeProcess(Enum fromTrack, Enum toTrack, float duration)
    {
        // Fade out the current track
        float startVolume = musicAudioSource.volume;
        while (musicAudioSource.volume > 0)
        {
            musicAudioSource.volume -= startVolume * Time.deltaTime / (duration / 2);
            yield return null;
        }
        musicAudioSource.Stop();

        // Play the new track
        PlayMusic(toTrack);

        // Fade in the new track
        musicAudioSource.volume = 0;
        float targetVolume = 1.0f;
        while (musicAudioSource.volume < targetVolume)
        {
            musicAudioSource.volume += targetVolume * Time.deltaTime / (duration / 2);
            yield return null;
        }
    }

    #endregion


    #region ENUMS

    public enum MenuMusic
    {
        None,
        MainTrack1,
        MainTrack2,
        PauseTrack1,
        PauseTrack2,

    }

    public enum GameMusic
    {
        None,
        IntroTrack,
        Level1Track1,
        Level1Track2,
        Level2Track1,
        Level2Track2,
        CreditsTrack,

    }
    public enum AmbienceMusic
    {
        None,
        Track1,
        Track2,
        Track3,

    }

    public enum UISFX
    {
        None,
        ButtonClick,
        ButtonHover,
        PopupOpen,
        MenuOpen,
        MenuClose,
        Nav1,

    }

    public enum PlayerSFX
    {
        None,
        Walk1,
        Walk2,
        Walk3,
        Shoot1,
        Shoot2,
        Speak1,
        TakeDamage1,
        Death,

    }

    public enum EnemySFX
    {
        None,
        Walk1,
        Walk2,
        Walk3,
        Shoot1,
        Shoot2,
        Speak1,
        TakeDamage1,
        Death,

    }

    #endregion ENUMS
}
