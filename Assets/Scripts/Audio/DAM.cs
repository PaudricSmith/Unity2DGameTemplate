using System;
using System.Collections;
using UnityEngine;


/// <summary>
/// DynamicAudioManager (DAM) is responsible for managing all audio-related functionalities.
/// </summary>
public class DAM : MonoBehaviour
{
    private bool isFadingGameMusic = false;
    private bool isFadingAmbienceMusic = false;
    
    [SerializeField] private AudioClipsSO audioClipsSO;
    [SerializeField] private AudioSource gameMusicSource1;
    [SerializeField] private AudioSource gameMusicSource2;
    [SerializeField] private AudioSource ambienceMusicSource1;
    [SerializeField] private AudioSource ambienceMusicSource2;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource uiSource;


    /// <summary>
    /// Singleton instance of DynamicAudioManager.
    /// </summary>
    public static DAM One;

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


    #region Properties

    // Properties to expose the playing state of the audio sources
    public bool IsGameMusicSource1Playing => gameMusicSource1.isPlaying;
    public bool IsGameMusicSource2Playing => gameMusicSource2.isPlaying;
    public bool IsAmbienceMusicSource1Playing => ambienceMusicSource1.isPlaying;
    public bool IsAmbienceMusicSource2Playing => ambienceMusicSource2.isPlaying;
    public bool IsSFXSourcePlaying => sfxSource.isPlaying;
    public bool IsUISourcePlaying => uiSource.isPlaying;

    #endregion Properties


    #region Setters

    public void SetAudioSettings(GameSettingsDataSO gameSettings)
    {
        AudioListener.volume = gameSettings.masterVolume;
        SetGameMusicVolume(gameSettings.gameMusicVolume);
        SetAmbienceMusicVolume(gameSettings.ambienceMusicVolume);
        SetSFXVolume(gameSettings.sfxVolume);
        SetUISFXVolume(gameSettings.uiSfxVolume);
    }

    #endregion Setters


    #region Playback Control

    /// <summary>
    /// Plays a game music track based on the provided track enumeration.
    /// </summary>
    /// <param name="track">The track to be played.</param>
    /// <param name="audioSource">Optional AudioSource through which the music will be played. Defaults to the class's gameMusicSource1.</param>
    public void PlayGameMusic(GameMusic track, AudioSource audioSource = null)
    {
        AudioClip clip = audioClipsSO.GetGameMusicClip(track);
        if (clip)
        {
            AudioSource targetSource = (audioSource != null) ? audioSource : gameMusicSource1;
            targetSource.clip = clip;
            targetSource.Play();
        }
    }

    /// <summary>
    /// Plays a game music track with a specified delay.
    /// </summary>
    /// <param name="track">The track to be played.</param>
    /// <param name="delay">The delay time in seconds before the track starts playing.</param>
    /// <param name="audioSource">Optional AudioSource through which the music will be played. Defaults to the class's gameMusicSource1.</param>
    public void PlayGameMusicDelayed(GameMusic track, float delay, AudioSource audioSource = null)
    {
        AudioClip clip = audioClipsSO.GetGameMusicClip(track);
        if (clip)
        {
            AudioSource targetSource = (audioSource != null) ? audioSource : gameMusicSource1;
            targetSource.clip = clip;
            targetSource.PlayDelayed(delay);
        }
    }

    /// <summary>
    /// Plays an ambience track based on the provided track enumeration.
    /// </summary>
    /// <param name="track">The track to be played.</param>
    /// <param name="audioSource">Optional AudioSource through which the music will be played. Defaults to the class's gameMusicSource1.</param>
    public void PlayAmbienceMusic(AmbienceMusic track, AudioSource audioSource = null)
    {
        AudioClip clip = audioClipsSO.GetAmbienceMusicClip(track);
        if (clip)
        {
            AudioSource targetSource = (audioSource != null) ? audioSource : ambienceMusicSource1;
            targetSource.clip = clip;
            targetSource.Play();
        }
    }

    /// <summary>
    /// Plays an ambience music track with a specified delay.
    /// </summary>
    /// <param name="track">The track to be played.</param>
    /// <param name="delay">The delay time in seconds before the track starts playing.</param>
    /// <param name="audioSource">Optional AudioSource through which the music will be played. Defaults to the class's gameMusicSource1.</param>
    public void PlayAmbienceMusicDelayed(AmbienceMusic track, float delay, AudioSource audioSource = null)
    {
        AudioClip clip = audioClipsSO.GetAmbienceMusicClip(track);
        if (clip)
        {
            AudioSource targetSource = (audioSource != null) ? audioSource : ambienceMusicSource1;
            targetSource.clip = clip;
            targetSource.PlayDelayed(delay);
        }
    }

    /// <summary>
    /// Plays a sound effect (SFX) based on the provided track enumeration.
    /// </summary>
    /// <param name="track">The track to be played.</param>
    public void PlaySFX(SFX track)
    {
        AudioClip clip = audioClipsSO.GetSFXClip(track);
        if (clip)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// Plays a sound effect (SFX) using a specified AudioSource and track enumeration.
    /// </summary>
    /// <param name="audioSource">The AudioSource component through which the sound effect will be played.</param>
    /// <param name="track">The track to be played.</param>
    public void PlaySFXFromSource(SFX track, AudioSource audioSource)
    {
        AudioClip clip = audioClipsSO.GetSFXClip(track);
        if (clip)
        {
            audioSource.PlayOneShot(clip); 
        }
    }

    /// <summary>
    /// Plays a sound effect (SFX) with a specified delay.
    /// </summary>
    /// <param name="track">The track to be played.</param>
    /// <param name="delay">The delay time in seconds before the track starts playing.</param>
    public void PlaySFXDelayed(SFX track, float delay)
    {
        AudioClip clip = audioClipsSO.GetSFXClip(track);
        if (clip)
        {
            sfxSource.clip = clip; 
            sfxSource.PlayDelayed(delay);
        }
    }

    /// <summary>
    /// Plays a sound effect with a specified delay.
    /// </summary>
    /// <param name="track">The track to be played.</param>
    /// <param name="delay">The delay time in seconds before the track starts playing.</param>
    /// <param name="audioSource">Optional AudioSource through which the sound effect will be played. Defaults to the class's sfxSource.</param>
    public void PlaySFXDelayedFromSource(SFX track, float delay, AudioSource audioSource)
    {
        AudioClip clip = audioClipsSO.GetSFXClip(track);
        if (clip)
        {
            audioSource.clip = clip;
            audioSource.PlayDelayed(delay);
        }
    }

    /// <summary>
    /// Plays a UI sound effect (UISFX) based on the provided track enumeration.
    /// </summary>
    /// <param name="track">The track to be played.</param>
    public void PlayUISFX(UISFX track)
    {
        AudioClip clip = audioClipsSO.GetUISFXClip(track);
        if (clip)
        {
            uiSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// Plays a UI sound effect (UISFX) using a specified AudioSource and track enumeration.
    /// </summary>
    /// <param name="audioSource">The AudioSource component through which the sound effect will be played.</param>
    /// <param name="track">The track to be played.</param>
    public void PlayUISFXFromSource(UISFX track, AudioSource audioSource)
    {
        AudioClip clip = audioClipsSO.GetUISFXClip(track);
        if (clip)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// Plays a UI sound effect (UISFX) with a specified delay.
    /// </summary>
    /// <param name="track">The track to be played.</param>
    /// <param name="delay">The delay time in seconds before the track starts playing.</param>
    public void PlayUISFXDelayed(UISFX track, float delay)
    {
        AudioClip clip = audioClipsSO.GetUISFXClip(track);
        if (clip)
        {
            uiSource.clip = clip;
            uiSource.PlayDelayed(delay);
        }
    }

    /// <summary>
    /// Plays a UI sound effect (UISFX) with a specified delay.
    /// </summary>
    /// <param name="track">The track to be played.</param>
    /// <param name="delay">The delay time in seconds before the track starts playing.</param>
    /// <param name="audioSource">Optional AudioSource through which the sound effect will be played. Defaults to the class's sfxSource.</param>
    public void PlayUISFXDelayedFromSource(UISFX track, float delay, AudioSource audioSource)
    {
        AudioClip clip = audioClipsSO.GetUISFXClip(track);
        if (clip)
        {
            audioSource.clip = clip;
            audioSource.PlayDelayed(delay);
        }
    }

    /// <summary>
    /// Pauses the currently playing game music track.
    /// </summary>
    /// <param name="audioSource">Optional AudioSource through which the music is being played. Defaults to the class's gameMusicSource1.</param>
    public void PauseGameMusic(AudioSource audioSource = null)
    {
        AudioSource targetSource = (audioSource != null) ? audioSource : gameMusicSource1;
        if (targetSource.isPlaying)
        {
            targetSource.Pause();
        }
    }

    /// <summary>
    /// Pauses the currently playing ambience music track.
    /// </summary>
    /// <param name="audioSource">Optional AudioSource through which the music is being played. Defaults to the class's gameMusicSource1.</param>
    public void PauseAmbienceMusic(AudioSource audioSource = null)
    {
        AudioSource targetSource = (audioSource != null) ? audioSource : ambienceMusicSource1;
        if (targetSource.isPlaying)
        {
            targetSource.Pause();
        }
    }

    /// <summary>
    /// Resumes the currently paused game music track.
    /// </summary>
    /// <param name="audioSource">Optional AudioSource through which the music is being played. Defaults to the class's gameMusicSource1.</param>
    public void ResumeGameMusic(AudioSource audioSource = null)
    {
        AudioSource targetSource = (audioSource != null) ? audioSource : gameMusicSource1;
        if (!targetSource.isPlaying)
        {
            targetSource.Play();
        }
    }

    /// <summary>
    /// Resumes the currently paused ambience music track.
    /// </summary>
    /// <param name="audioSource">Optional AudioSource through which the music is being played. Defaults to the class's gameMusicSource1.</param>
    public void ResumeAmbienceMusic(AudioSource audioSource = null)
    {
        AudioSource targetSource = (audioSource != null) ? audioSource : ambienceMusicSource1;
        if (!targetSource.isPlaying)
        {
            targetSource.Play();
        }
    }

    /// <summary>
    /// Stops the currently playing game music track.
    /// </summary>
    /// <param name="audioSource">Optional AudioSource through which the music is being played. Defaults to the class's gameMusicSource1.</param>
    public void StopGameMusic(AudioSource audioSource = null)
    {
        AudioSource targetSource = (audioSource != null) ? audioSource : gameMusicSource1;
        targetSource.Stop();
    }

    /// <summary>
    /// Stops the currently playing ambience music track.
    /// </summary>
    /// <param name="audioSource">Optional AudioSource through which the music is being played. Defaults to the class's gameMusicSource1.</param>
    public void StopMusic(AudioSource audioSource = null)
    {
        AudioSource targetSource = (audioSource != null) ? audioSource : ambienceMusicSource1;
        targetSource.Stop();
    }

    #endregion


    #region Volume Control

    /// <summary>
    /// Sets the volume for game music playback.
    /// </summary>
    /// <param name="volume">The volume level between 0 and 1.</param>
    /// <param name="audioSource">Optional AudioSource to set the volume for. Defaults to the class's game music sources.</param>
    public void SetGameMusicVolume(float volume, AudioSource audioSource = null)
    {
        // Clamp the volume to be within the range [0, 1]
        volume = Mathf.Clamp(volume, 0, 1);

        if (audioSource == null)
        {
            gameMusicSource1.volume = volume;
            gameMusicSource2.volume = volume;
        }
        else
        {
            audioSource.volume = volume;
        }
    }

    /// <summary>
    /// Sets the volume for ambience music playback.
    /// </summary>
    /// <param name="volume">The volume level between 0 and 1.</param>
    /// <param name="audioSource">Optional AudioSource to set the volume for. Defaults to the class's gameMusicSource1.</param>
    public void SetAmbienceMusicVolume(float volume, AudioSource audioSource = null)
    {
        // Clamp the volume to be within the range [0, 1]
        volume = Mathf.Clamp(volume, 0, 1);

        if (audioSource == null)
        {
            ambienceMusicSource1.volume = volume;
            ambienceMusicSource2.volume = volume;
        }
        else
        {
            audioSource.volume = volume;
        }
    }

    /// <summary>
    /// Sets the volume for general sound effects.
    /// </summary>
    /// <param name="volume">The volume level between 0 and 1.</param>
    public void SetSFXVolume(float volume)
    {
        // Clamp the volume to be within the range [0, 1]
        volume = Mathf.Clamp(volume, 0, 1);

        sfxSource.volume = volume;
    }

    /// <summary>
    /// Sets the volume of a sound effect from a specified AudioSource.
    /// </summary>
    /// <param name="volume">The volume level to set, between 0 and 1.</param>
    /// <param name="audioSource">The AudioSource component through which the sound effect is being played.</param>
    public void SetSFXVolumeFromSource(float volume, AudioSource audioSource)
    {
        // Clamp the volume to be within the range [0, 1]
        volume = Mathf.Clamp(volume, 0, 1);

        audioSource.volume = volume;
    }

    /// <summary>
    /// Sets the volume for UI sound effects.
    /// </summary>
    /// <param name="volume">The volume level between 0 and 1.</param>
    public void SetUISFXVolume(float volume)
    {
        // Clamp the volume to be within the range [0, 1]
        volume = Mathf.Clamp(volume, 0, 1);

        uiSource.volume = volume;
    }

    /// <summary>
    /// Sets the volume of a UI sound effect from a specified AudioSource.
    /// </summary>
    /// <param name="volume">The volume level to set, between 0 and 1.</param>
    /// <param name="audioSource">The AudioSource component through which the sound effect is being played.</param>
    public void SetUISFXVolumeFromSource(float volume, AudioSource audioSource)
    {
        // Clamp the volume to be within the range [0, 1]
        volume = Mathf.Clamp(volume, 0, 1);

        audioSource.volume = volume;
    }

    /// <summary>
    /// Gets the current volume level of the game music track.
    /// </summary>
    /// <param name="audioSource">Optional AudioSource through which the music is being played. Defaults to the class's gameMusicSource1.</param>
    /// <returns>Current volume level.</returns>
    public float GetGameMusicVolume(AudioSource audioSource = null)
    {
        AudioSource targetSource = (audioSource != null) ? audioSource : gameMusicSource1;
        return targetSource.volume;
    }

    /// <summary>
    /// Gets the current volume level of the ambience music track.
    /// </summary>
    /// <param name="audioSource">Optional AudioSource through which the music is being played. Defaults to the class's gameMusicSource1.</param>
    /// <returns>Current volume level.</returns>
    public float GetAmbienceMusicVolume(AudioSource audioSource = null)
    {
        AudioSource targetSource = (audioSource != null) ? audioSource : ambienceMusicSource1;
        return targetSource.volume;
    }

    /// <summary>
    /// Gets the current volume level of the general sound effects.
    /// </summary>
    /// <returns>Current volume level.</returns>
    public float GetSFXVolume()
    {
        return sfxSource.volume;
    }

    /// <summary>
    /// Gets the current volume level of the sound effects from a specified AudioSource.
    /// </summary>
    /// <param name="audioSource">The AudioSource component through which the sound effect is being played.</param>
    /// <returns>Current volume level.</returns>
    public float GetSFXVolumeFromSource(AudioSource audioSource)
    {
        return audioSource.volume;
    }

    /// <summary>
    /// Gets the current volume level of the UI sound effects.
    /// </summary>
    /// <returns>Current volume level.</returns>
    public float GetUISFXVolume()
    {
        return uiSource.volume;
    }

    /// <summary>
    /// Gets the current volume level of the UI sound effects from a specified AudioSource.
    /// </summary>
    /// <param name="audioSource">The AudioSource component through which the sound effect is being played.</param>
    /// <returns>Current volume level.</returns>
    public float GetUISFXVolumeFromSource(AudioSource audioSource)
    {
        return audioSource.volume;
    }

    #endregion


    #region AudioControlMethods

    /// <summary>
    /// Sets whether the game music should loop.
    /// </summary>
    /// <param name="shouldLoop">True if the music should loop, false otherwise.</param>
    /// <param name="audioSource">Optional AudioSource to set the looping for. Defaults to the class's gameMusicSource1.</param>
    public void SetGameMusicLooping(bool shouldLoop, AudioSource audioSource = null)
    {
        AudioSource targetSource = (audioSource != null) ? audioSource : gameMusicSource1;
        targetSource.loop = shouldLoop;
    }

    /// <summary>
    /// Mutes the game music.
    /// </summary>
    /// <param name="audioSource">Optional AudioSource to mute. Defaults to the class's gameMusicSource1.</param>
    public void MuteGameMusic(AudioSource audioSource = null)
    {
        AudioSource targetSource = (audioSource != null) ? audioSource : gameMusicSource1;
        targetSource.mute = true;
    }

    /// <summary>
    /// Unmutes the game music.
    /// </summary>
    /// <param name="audioSource">Optional AudioSource to unmute. Defaults to the class's gameMusicSource1.</param>
    public void UnmuteGameMusic(AudioSource audioSource = null)
    {
        AudioSource targetSource = (audioSource != null) ? audioSource : gameMusicSource1;
        targetSource.mute = false;
    }

    /// <summary>
    /// Sets the pitch of the game music track.
    /// </summary>
    /// <param name="pitch">The pitch level to set.</param>
    /// <param name="audioSource">Optional AudioSource through which the music is being played. Defaults to the class's gameMusicSource1.</param>
    public void SetGameMusicPitch(float pitch, AudioSource audioSource = null)
    {
        AudioSource targetSource = (audioSource != null) ? audioSource : gameMusicSource1;
        targetSource.pitch = pitch;
    }

    /// <summary>
    /// Sets whether the ambience music should loop.
    /// </summary>
    /// <param name="shouldLoop">True if the music should loop, false otherwise.</param>
    /// <param name="audioSource">Optional AudioSource to set the looping for. Defaults to the class's gameMusicSource1.</param>
    public void SetAmbienceMusicLooping(bool shouldLoop, AudioSource audioSource = null)
    {
        AudioSource targetSource = (audioSource != null) ? audioSource : ambienceMusicSource1;
        targetSource.loop = shouldLoop;
    }

    /// <summary>
    /// Mutes the ambience music.
    /// </summary>
    /// <param name="audioSource">Optional AudioSource to mute. Defaults to the class's gameMusicSource1.</param>
    public void MuteAmbienceMusic(AudioSource audioSource = null)
    {
        AudioSource targetSource = (audioSource != null) ? audioSource : ambienceMusicSource1;
        targetSource.mute = true;
    }

    /// <summary>
    /// Unmutes the ambience music.
    /// </summary>
    /// <param name="audioSource">Optional AudioSource to unmute. Defaults to the class's gameMusicSource1.</param>
    public void UnmuteAmbienceMusic(AudioSource audioSource = null)
    {
        AudioSource targetSource = (audioSource != null) ? audioSource : ambienceMusicSource1;
        targetSource.mute = false;
    }

    /// <summary>
    /// Sets the pitch of the ambience music track.
    /// </summary>
    /// <param name="pitch">The pitch level to set.</param>
    /// <param name="audioSource">Optional AudioSource through which the music is being played. Defaults to the class's gameMusicSource1.</param>
    public void SetAmbienceMusicPitch(float pitch, AudioSource audioSource = null)
    {
        AudioSource targetSource = (audioSource != null) ? audioSource : ambienceMusicSource1;
        targetSource.pitch = pitch;
    }

    /// <summary>
    /// Sets the pitch of the general sound effects.
    /// </summary>
    /// <param name="pitch">The pitch level to set.</param>
    public void SetSFXPitch(float pitch)
    {
        sfxSource.pitch = pitch;
    }


    /// <summary>
    /// Sets the pitch of the sound effects from a specified AudioSource.
    /// </summary>
    /// <param name="pitch">The pitch level to set.</param>
    /// <param name="audioSource">The AudioSource component through which the sound effect is being played.</param>
    public void SetSFXPitchFromSource(float pitch, AudioSource audioSource)
    {
        audioSource.pitch = pitch;
    }

    /// <summary>
    /// Sets the pitch of the UI sound effects.
    /// </summary>
    /// <param name="pitch">The pitch level to set.</param>
    public void SetUISFXPitch(float pitch)
    {
        uiSource.pitch = pitch;
    }


    /// <summary>
    /// Sets the pitch of the UI sound effects from a specified AudioSource.
    /// </summary>
    /// <param name="pitch">The pitch level to set.</param>
    /// <param name="audioSource">The AudioSource component through which the sound effect is being played.</param>
    public void SetUISFXPitchFromSource(float pitch, AudioSource audioSource)
    {
        audioSource.pitch = pitch;
    }

    #endregion


    #region Fade Control

    /// <summary>
    /// Fades in game music over a specified duration.
    /// </summary>
    /// <param name="track">Track to fade in.</param>
    /// <param name="duration">Duration of the fade-in effect.</param>
    /// <param name="audioSource">Optional AudioSource to fade in. Defaults to the class's gameMusicSource1.</param>
    public void FadeInGameMusic(GameMusic track, float duration, AudioSource audioSource = null)
    {
        AudioSource targetSource = (audioSource != null) ? audioSource : gameMusicSource1;
        AudioClip clip = audioClipsSO.GetGameMusicClip(track);
        if (clip != null)
        {
            StartCoroutine(FadeInProcess(targetSource, clip, duration));
        }
    }

    /// <summary>
    /// Fades out game music over a specified duration.
    /// </summary>
    /// <param name="duration">Duration of the fade-out effect.</param>
    /// <param name="audioSource">Optional AudioSource to fade out. Defaults to the class's gameMusicSource1.</param>
    public void FadeOutGameMusic(float duration, AudioSource audioSource = null)
    {
        AudioSource targetSource = (audioSource != null) ? audioSource : gameMusicSource1;
        StartCoroutine(FadeOutProcess(targetSource, duration));
    }

    /// <summary>
    /// Fades in ambience music over a specified duration.
    /// </summary>
    /// <param name="track">Track to fade in.</param>
    /// <param name="duration">Duration of the fade-in effect.</param>
    /// <param name="audioSource">Optional AudioSource to fade in. Defaults to the class's gameMusicSource1.</param>
    public void FadeInAmbienceMusic(AmbienceMusic track, float duration, AudioSource audioSource = null)
    {
        AudioSource targetSource = (audioSource != null) ? audioSource : ambienceMusicSource1;
        AudioClip clip = audioClipsSO.GetAmbienceMusicClip(track);
        if (clip != null)
        {
            StartCoroutine(FadeInProcess(targetSource, clip, duration));
        }
    }

    /// <summary>
    /// Fades out ambience music over a specified duration.
    /// </summary>
    /// <param name="duration">Duration of the fade-out effect.</param>
    /// <param name="audioSource">Optional AudioSource to fade out. Defaults to the class's gameMusicSource1.</param>
    public void FadeOutAmbienceMusic(float duration, AudioSource audioSource = null)
    {
        AudioSource targetSource = (audioSource != null) ? audioSource : ambienceMusicSource1;
        StartCoroutine(FadeOutProcess(targetSource, duration));
    }

    /// <summary>
    /// Fades in general sound effects over a specified duration.
    /// </summary>
    /// <param name="track">Track to fade in.</param>
    /// <param name="duration">Duration of the fade-in effect.</param>
    public void FadeInSFX(SFX track, float duration)
    {
        AudioClip clip = audioClipsSO.GetSFXClip(track);
        if (clip != null)
        {
            StartCoroutine(FadeInProcess(sfxSource, clip, duration));
        }
    }

    /// <summary>
    /// Fades in a sound effect from a specified AudioSource over a given duration.
    /// </summary>
    /// <param name="track">The track to be played.</param>
    /// <param name="duration">The duration over which to fade in the sound effect.</param>
    /// <param name="audioSource">The AudioSource component through which the sound effect will be played.</param>
    public void FadeInSFXFromSource(SFX track, float duration, AudioSource audioSource)
    {
        AudioClip clip = audioClipsSO.GetSFXClip(track);
        if (clip != null)
        {
            StartCoroutine(FadeInProcess(audioSource, clip, duration));
        }
    }

    /// <summary>
    /// Fades out general sound effects over a specified duration.
    /// </summary>
    /// <param name="duration">Duration of the fade-out effect.</param>
    public void FadeOutSFX(float duration)
    {
        StartCoroutine(FadeOutProcess(sfxSource, duration));
    }

    /// <summary>
    /// Fades out a sound effect from a specified AudioSource over a given duration.
    /// </summary>
    /// <param name="duration">The duration over which to fade out the sound effect.</param>
    /// <param name="audioSource">The AudioSource component through which the sound effect is being played.</param>
    public void FadeOutSFXFromSource(float duration, AudioSource audioSource)
    {
        StartCoroutine(FadeOutProcess(audioSource, duration));
    }

    /// <summary>
    /// Fades in UI sound effects over a specified duration.
    /// </summary>
    /// <param name="track">Track to fade in.</param>
    /// <param name="duration">Duration of the fade-in effect.</param>
    public void FadeInUISFX(UISFX track, float duration)
    {
        AudioClip clip = audioClipsSO.GetUISFXClip(track);
        if (clip != null)
        {
            StartCoroutine(FadeInProcess(uiSource, clip, duration));
        }
    }

    /// <summary>
    /// Fades in a UI sound effect from a specified AudioSource over a given duration.
    /// </summary>
    /// <param name="track">The track to be played.</param>
    /// <param name="duration">The duration over which to fade in the sound effect.</param>
    /// <param name="audioSource">The AudioSource component through which the sound effect will be played.</param>
    public void FadeInUISFXFromSource(UISFX track, float duration, AudioSource audioSource)
    {
        AudioClip clip = audioClipsSO.GetUISFXClip(track);
        if (clip != null)
        {
            StartCoroutine(FadeInProcess(audioSource, clip, duration));
        }
    }

    /// <summary>
    /// Fades out UI sound effects over a specified duration.
    /// </summary>
    /// <param name="duration">Duration of the fade-out effect.</param>
    public void FadeOutUISFX(float duration)
    {
        StartCoroutine(FadeOutProcess(uiSource, duration));
    }

    /// <summary>
    /// Fades out a UI sound effect from a specified AudioSource over a given duration.
    /// </summary>
    /// <param name="duration">The duration over which to fade out the sound effect.</param>
    /// <param name="audioSource">The AudioSource component through which the sound effect is being played.</param>
    public void FadeOutUISFXFromSource(float duration, AudioSource audioSource)
    {
        StartCoroutine(FadeOutProcess(audioSource, duration));
    }

    /// <summary>
    /// Initiates a crossfade between the currently playing game music AudioClip and a new AudioClip.
    /// </summary>
    /// <param name="newClip">The AudioClip to crossfade to.</param>
    /// <param name="duration">The duration of the crossfade in seconds.</param>
    public void CrossFadeGameMusic(AudioClip newClip, float duration)
    {
        if (isFadingGameMusic) return; // Prevent overlapping fades

        StartCoroutine(CrossFadeGameMusicProcess(newClip, duration));
    }

    /// <summary>
    /// Initiates a crossfade between the currently playing ambience music AudioClip and a new AudioClip.
    /// </summary>
    /// <param name="newClip">The AudioClip to crossfade to.</param>
    /// <param name="duration">The duration of the crossfade in seconds.</param>
    public void CrossFadeAmbienceMusic(AudioClip newClip, float duration)
    {
        if (isFadingAmbienceMusic) return; // Prevent overlapping fades

        StartCoroutine(CrossFadeAmbienceMusicProcess(newClip, duration));
    }

    /// <summary>
    /// Transitions between two game music tracks with a fade-out and fade-in effect.
    /// </summary>
    /// <param name="fromTrack">Track to fade out.</param>
    /// <param name="toTrack">Track to fade in.</param>
    /// <param name="duration">Duration of the transition.</param>
    public void TransitionGameMusicTracks(GameMusic fromTrack, GameMusic toTrack, float duration)
    {
        StartCoroutine(FadeOutProcess(gameMusicSource1, duration, () =>
        {
            FadeInGameMusic(toTrack, duration);
        }));
    }

    /// <summary>
    /// Transitions between two ambience music tracks with a fade-out and fade-in effect.
    /// </summary>
    /// <param name="fromTrack">Track to fade out.</param>
    /// <param name="toTrack">Track to fade in.</param>
    /// <param name="duration">Duration of the transition.</param>
    public void TransitionAmbienceMusicTracks(AmbienceMusic fromTrack, AmbienceMusic toTrack, float duration)
    {
        StartCoroutine(FadeOutProcess(ambienceMusicSource1, duration, () =>
        {
            FadeInAmbienceMusic(toTrack, duration);
        }));
    }

    #endregion


    #region Coroutines

    private IEnumerator FadeInProcess(AudioSource audioSource, AudioClip newClip, float duration)
    {
        // Check if a new audio clip is provided
        if (newClip)
        {
            // Set the new audio clip and start playing
            audioSource.clip = newClip;
            audioSource.Play();
        }

        // Initialize volume to zero
        audioSource.volume = 0;
        float targetVolume = 1.0f;

        // Gradually increase volume to target volume
        while (audioSource.volume < targetVolume)
        {
            // Incrementally increase the volume of the audio source based on the target volume, frame time, and duration
            audioSource.volume += targetVolume * Time.deltaTime / duration;

            // Pause the coroutine and wait for the next frame before continuing
            yield return null;
        }
    }

    private IEnumerator FadeOutProcess(AudioSource audioSource, float duration, Action onFinished = null)
    {
        // Store the initial volume
        float startVolume = audioSource.volume;

        // Gradually reduce the volume to zero
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;

            yield return null;
        }

        audioSource.Stop();

        // Reset the volume to the initial value
        audioSource.volume = startVolume;

        // Invoke any callback actions
        onFinished?.Invoke();
    }

    private IEnumerator CrossFadeGameMusicProcess(AudioClip newClip, float duration)
    {
        isFadingGameMusic = true;

        AudioSource activeSource = (gameMusicSource1.isPlaying) ? gameMusicSource1 : gameMusicSource2;
        AudioSource inactiveSource = (activeSource == gameMusicSource1) ? gameMusicSource2 : gameMusicSource1;

        inactiveSource.clip = newClip;
        inactiveSource.Play();
        inactiveSource.volume = 0;

        float halfDuration = duration / 2.0f;

        // Fade out the active source and fade in the inactive source
        for (float t = 0; t < halfDuration; t += Time.deltaTime)
        {
            activeSource.volume = Mathf.Lerp(1, 0, t / halfDuration);
            inactiveSource.volume = Mathf.Lerp(0, 1, t / halfDuration);

            yield return null;
        }

        // Stop the active source and make the inactive source the new active source
        activeSource.Stop();
        activeSource.volume = 1; // Reset volume for future use

        isFadingGameMusic = false;
    }

    private IEnumerator CrossFadeAmbienceMusicProcess(AudioClip newClip, float duration)
    {
        isFadingAmbienceMusic = true;

        AudioSource activeSource = (ambienceMusicSource1.isPlaying) ? ambienceMusicSource1 : ambienceMusicSource2;
        AudioSource inactiveSource = (activeSource == ambienceMusicSource1) ? ambienceMusicSource2 : ambienceMusicSource1;

        inactiveSource.clip = newClip;
        inactiveSource.Play();
        inactiveSource.volume = 0;

        float halfDuration = duration / 2.0f;

        // Fade out the active source and fade in the inactive source
        for (float t = 0; t < halfDuration; t += Time.deltaTime)
        {
            activeSource.volume = Mathf.Lerp(1, 0, t / halfDuration);
            inactiveSource.volume = Mathf.Lerp(0, 1, t / halfDuration);

            yield return null;
        }

        // Stop the active source and make the inactive source the new active source
        activeSource.Stop();
        activeSource.volume = 1; // Reset volume for future use

        isFadingAmbienceMusic = false;
    }

    #endregion


    #region ENUMS

    public enum GameMusic
    {
        None,
        IntroTrack,
        MenuTrack1,
        MenuTrack2,
        PauseTrack1,
        PauseTrack2,
        Level1Track1,
        Level1Track2,
        Level2Track1,
        Level2Track2,
        CreditsTrack,
    }

    public enum AmbienceMusic
    {
        None,
        Dungeon1,
        Track2,
        Track3,

    }

    public enum SFX
    {
        None,
        DoorOpen,
        DoorClose,
        ChestOpen,
        ChestClose,
        Thud,
        Success,
        Failed,
        Positive,
        Negative,

    }

    public enum UISFX
    {
        None,
        ButtonClick1,
        ButtonClick2,
        ButtonHover1,
        ButtonHover2,
        Toggle1,
        Toggle2,
        DropdownOpened,
        Select,
        SliderSelect,
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
