using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// DynamicAudioManager (DAM) is responsible for managing all audio-related functionalities.
/// </summary>
public class DAM : MonoBehaviour
{
    private Coroutine fadeInCoroutine;
    private Coroutine fadeOutCoroutine;

    private float masterVolume;
    private float gameMusicVolume;
    private float ambienceMusicVolume;
    private float sfxVolume;
    private float uiSfxVolume;

    private bool isCrossFadingGameMusic = false;
    private bool isCrossFadingAmbienceMusic = false;

    [SerializeField] private GameSettingsDataSO gameSettingsSO;
    [SerializeField] private AudioClipsSO audioClipsSO;

    [SerializeField] private AudioSource[] gameMusicSources;
    [SerializeField] private AudioSource[] ambienceMusicSources;
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

    public void SetAllVolumes()
    {
        masterVolume = gameSettingsSO.masterVolume;
        gameMusicVolume = gameSettingsSO.gameMusicVolume;
        ambienceMusicVolume = gameSettingsSO.ambienceMusicVolume;
        sfxVolume = gameSettingsSO.sfxVolume;
        uiSfxVolume = gameSettingsSO.uiSfxVolume;

        AudioListener.volume = masterVolume;
        SetGameMusicVolume(gameMusicVolume);
        SetAmbienceMusicVolume(ambienceMusicVolume);
        SetSFXVolume(sfxVolume);
        SetUISFXVolume(uiSfxVolume);
    }



    #region Playback Control

    /// <summary>
    /// Plays a specified game music track.
    /// The AudioSource to be used is specified by its index in the gameMusicSources array.
    /// </summary>
    /// <param name="track">The track to be played.</param>
    /// <param name="sourceIndex">The AudioSource index through which the game music is being played.</param>
    public void PlayGameMusic(GameMusic track, int audioSourceIndex)
    {
        AudioClip clip = audioClipsSO.GetGameMusicClip(track);
        if (clip)
        {
            gameMusicSources[audioSourceIndex].clip = clip;
            gameMusicSources[audioSourceIndex].Play();
        }
    }

    /// <summary>
    /// Plays a game music track with a specified delay.
    /// </summary>
    /// <param name="track">The track to be played.</param>
    /// <param name="audioSourceIndex">The AudioSource index through which the game music is being played.</param>
    /// <param name="delay">The delay time in seconds before the track starts playing.</param>
    public void PlayGameMusicDelayed(GameMusic track, int audioSourceIndex, float delay)
    {
        AudioClip clip = audioClipsSO.GetGameMusicClip(track);
        if (clip)
        {
            gameMusicSources[audioSourceIndex].clip = clip;
            gameMusicSources[audioSourceIndex].PlayDelayed(delay);
        }
    }

    /// <summary>
    /// Plays an ambience track based on the provided track enumeration.
    /// </summary>
    /// <param name="track">The track to be played.</param>
    /// <param name="audioSourceIndex">The AudioSource index through which the ambience music is being played.</param>
    public void PlayAmbienceMusic(AmbienceMusic track, int audioSourceIndex)
    {
        AudioClip clip = audioClipsSO.GetAmbienceMusicClip(track);
        if (clip)
        {
            ambienceMusicSources[audioSourceIndex].clip = clip;
            ambienceMusicSources[audioSourceIndex].Play();
        }
    }

    /// <summary>
    /// Plays an ambience music track with a specified delay.
    /// </summary>
    /// <param name="track">The track to be played.</param>
    /// <param name="audioSourceIndex">The AudioSource index through which the ambience music is being played.</param>
    /// <param name="delay">The delay time in seconds before the track starts playing.</param>
    public void PlayAmbienceMusicDelayed(AmbienceMusic track, int audioSourceIndex, float delay)
    {
        AudioClip clip = audioClipsSO.GetAmbienceMusicClip(track);
        if (clip)
        {
            ambienceMusicSources[audioSourceIndex].clip = clip;
            ambienceMusicSources[audioSourceIndex].PlayDelayed(delay);
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
            sfxSource.PlayOneShot(clip);
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
    /// Plays a UI sound effect (UISFX) based on the provided track enumeration.
    /// </summary>
    /// <param name="track">The track to be played.</param>
    public void PlayUISFX(UISFX track)
    {
        AudioClip clip = audioClipsSO.GetUISFXClip(track);
        if (clip)
            uiSource.PlayOneShot(clip);
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
    /// Pauses the currently playing game music track.
    /// </summary>
    /// <param name="audioSourceIndex">The AudioSource index through which the game music is being played.</param>
    public void PauseGameMusic(int audioSourceIndex)
    {
        if (gameMusicSources[audioSourceIndex].isPlaying) 
            gameMusicSources[audioSourceIndex].Pause();
    }

    /// <summary>
    /// Pauses the currently playing ambience music track.
    /// </summary>
    /// <param name="audioSourceIndex">The AudioSource index through which the ambience music is being played.</param>
    public void PauseAmbienceMusic(int audioSourceIndex)
    {
        if (ambienceMusicSources[audioSourceIndex].isPlaying) 
            ambienceMusicSources[audioSourceIndex].Pause();
    }

    /// <summary>
    /// Resumes the currently paused game music track.
    /// </summary>
    /// <param name="audioSourceIndex">The AudioSource index through which the game music is being played.</param>
    public void ResumeGameMusic(int audioSourceIndex)
    {
        if (!gameMusicSources[audioSourceIndex].isPlaying)
            gameMusicSources[audioSourceIndex].Play();
    }

    /// <summary>
    /// Resumes the currently paused ambience music track.
    /// </summary>
    /// <param name="audioSourceIndex">The AudioSource index through which the ambience music is being played.</param>
    public void ResumeAmbienceMusic(int audioSourceIndex)
    {
        if (!ambienceMusicSources[audioSourceIndex].isPlaying)
            ambienceMusicSources[audioSourceIndex].Play();
    }

    /// <summary>
    /// Stops the currently playing game music track.
    /// </summary>
    /// <param name="audioSourceIndex">The AudioSource index through which the game music is being played.</param>
    public void StopGameMusic(int audioSourceIndex) => gameMusicSources[audioSourceIndex].Stop();

    /// <summary>
    /// Stops the currently playing ambience music track.
    /// </summary>
    /// <param name="audioSourceIndex">The AudioSource index through which the ambience music is being played.</param>
    public void StopAmbienceMusic(int audioSourceIndex) => ambienceMusicSources[audioSourceIndex].Stop();

    #endregion


    #region Volume Control

    /// <summary>
    /// Sets the volume for game music playback.
    /// </summary>
    /// <param name="volume">The volume level between 0 and 1.</param>
    public void SetGameMusicVolume(float volume)
    {
        // Clamp the volume to be within the range [0, 1]
        volume = Mathf.Clamp(volume, 0, 1);

        foreach (AudioSource source in gameMusicSources)
            source.volume = volume;
    }

    /// <summary>
    /// Sets the volume for ambience music playback.
    /// </summary>
    /// <param name="volume">The volume level between 0 and 1.</param>
    public void SetAmbienceMusicVolume(float volume)
    {
        // Clamp the volume to be within the range [0, 1]
        volume = Mathf.Clamp(volume, 0, 1);

        foreach (AudioSource source in ambienceMusicSources)
            source.volume = volume;
    }

    /// <summary>
    /// Sets the volume for general sound effects.
    /// </summary>
    /// <param name="volume">The volume level between 0 and 1.</param>
    public void SetSFXVolume(float volume)
    {
        // Clamp the volume to be within the range [0, 1]
        sfxSource.volume = Mathf.Clamp(volume, 0, 1);
    }

    /// <summary>
    /// Sets the volume for UI sound effects.
    /// </summary>
    /// <param name="volume">The volume level between 0 and 1.</param>
    public void SetUISFXVolume(float volume)
    {
        // Clamp the volume to be within the range [0, 1]
        uiSource.volume = Mathf.Clamp(volume, 0, 1);
    }

    /// <summary>
    /// Gets the current volume level of the game music track.
    /// </summary>
    /// <param name="audioSourceIndex">The AudioSource index through which the game music is being played.</param>
    /// <returns>Current volume level.</returns>
    public float GetGameMusicVolume(int audioSourceIndex) => gameMusicSources[audioSourceIndex].volume;

    /// <summary>
    /// Gets the current volume level of the ambience music track.
    /// </summary>
    /// <param name="audioSourceIndex">The AudioSource index through which the ambience music is being played.</param>
    /// <returns>Current volume level.</returns>
    public float GetAmbienceMusicVolume(int audioSourceIndex) => ambienceMusicSources[audioSourceIndex].volume;

    /// <summary>
    /// Gets the current volume level of the general sound effects.
    /// </summary>
    /// <returns>Current volume level.</returns>
    public float GetSFXVolume() => sfxSource.volume;

    /// <summary>
    /// Gets the current volume level of the UI sound effects.
    /// </summary>
    /// <returns>Current volume level.</returns>
    public float GetUISFXVolume() => uiSource.volume;

    #endregion


    #region AudioControlMethods

    /// <summary>
    /// Sets whether the game music should loop.
    /// </summary>
    /// <param name="audioSourceIndex">The AudioSource index through which the game music is being played.</param>
    /// <param name="shouldLoop">True if the music should loop, false otherwise.</param>
    public void SetGameMusicLooping(int audioSourceIndex, bool shouldLoop) => gameMusicSources[audioSourceIndex].loop = shouldLoop;

    /// <summary>
    /// Mutes the game music.
    /// </summary>
    /// <param name="audioSourceIndex">The AudioSource index through which the game music is being played.</param>
    public void MuteGameMusic(int audioSourceIndex) => gameMusicSources[audioSourceIndex].mute = true;

    /// <summary>
    /// Unmutes the game music.
    /// </summary>
    /// <param name="audioSourceIndex">The AudioSource index through which the game music is being played.</param>
    public void UnmuteGameMusic(int audioSourceIndex) => gameMusicSources[audioSourceIndex].mute = false;

    /// <summary>
    /// Sets the pitch of the game music track.
    /// </summary>
    /// <param name="audioSourceIndex">The AudioSource index through which the game music is being played.</param>
    /// <param name="pitch">The pitch level to set.</param>
    public void SetGameMusicPitch(int audioSourceIndex, float pitch) => gameMusicSources[audioSourceIndex].pitch = pitch;

    /// <summary>
    /// Sets whether the ambience music should loop.
    /// </summary>
    /// <param name="audioSourceIndex">The AudioSource index through which the ambience music is being played.</param>
    /// <param name="shouldLoop">True if the music should loop, false otherwise.</param>
    public void SetAmbienceMusicLooping(int audioSourceIndex, bool shouldLoop) => ambienceMusicSources[audioSourceIndex].loop = shouldLoop;

    /// <summary>
    /// Mutes the ambience music.
    /// </summary>
    /// <param name="audioSourceIndex">The AudioSource index through which the ambience music is being played.</param>
    public void MuteAmbienceMusic(int audioSourceIndex) => ambienceMusicSources[audioSourceIndex].mute = true;

    /// <summary>
    /// Unmutes the ambience music.
    /// </summary>
    /// <param name="audioSourceIndex">The AudioSource index through which the ambience music is being played.</param>
    public void UnmuteAmbienceMusic(int audioSourceIndex) => ambienceMusicSources[audioSourceIndex].mute = false;

    /// <summary>
    /// Sets the pitch of the ambience music track.
    /// </summary>
    /// <param name="audioSourceIndex">The AudioSource index through which the ambience music is being played.</param>
    /// <param name="pitch">The pitch level to set.</param>
    public void SetAmbienceMusicPitch(int audioSourceIndex, float pitch) => ambienceMusicSources[audioSourceIndex].pitch = pitch;

    /// <summary>
    /// Sets the pitch of the general sound effects.
    /// </summary>
    /// <param name="pitch">The pitch level to set.</param>
    public void SetSFXPitch(float pitch) => sfxSource.pitch = pitch;

    /// <summary>
    /// Sets the pitch of the UI sound effects.
    /// </summary>
    /// <param name="pitch">The pitch level to set.</param>
    public void SetUISFXPitch(float pitch) => uiSource.pitch = pitch;

    #endregion


    #region Fade Control

    /// <summary>
    /// Fades in game music over a specified duration.
    /// </summary>
    /// <param name="track">Track to fade in.</param>
    /// <param name="audioSourceIndex">The AudioSource index through which the game music is being played.</param>
    /// <param name="duration">Duration of the fade-in effect.</param>
    public void FadeInGameMusic(GameMusic track, int audioSourceIndex, float duration)
    {
        AudioClip clip = audioClipsSO.GetGameMusicClip(track);
        if (clip) fadeInCoroutine = StartCoroutine(FadeInProcess(gameMusicSources[audioSourceIndex], clip, duration));
    }

    /// <summary>
    /// Fades out game music over a specified duration.
    /// </summary>
    /// <param name="audioSourceIndex">The AudioSource index through which the game music is being played.</param>
    /// <param name="duration">Duration of the fade-out effect.</param>
    public void FadeOutGameMusic(int audioSourceIndex, float duration)
    {
        AudioSource targetSource = gameMusicSources[audioSourceIndex];
        fadeOutCoroutine = StartCoroutine(FadeOutProcess(targetSource, duration));
    }

    /// <summary>
    /// Fades in ambience music over a specified duration.
    /// </summary>
    /// <param name="track">Track to fade in.</param>
    /// <param name="audioSourceIndex">The AudioSource index through which the ambience music is being played.</param>
    /// <param name="duration">Duration of the fade-in effect.</param>
    public void FadeInAmbienceMusic(AmbienceMusic track, int audioSourceIndex, float duration)
    {
        AudioClip clip = audioClipsSO.GetAmbienceMusicClip(track);
        if (clip) StartCoroutine(FadeInProcess(ambienceMusicSources[audioSourceIndex], clip, duration));
    }

    /// <summary>
    /// Fades out ambience music over a specified duration.
    /// </summary>
    /// <param name="audioSourceIndex">The AudioSource index through which the ambience music is being played.</param>
    /// <param name="duration">Duration of the fade-out effect.</param>
    public void FadeOutAmbienceMusic(int audioSourceIndex, float duration)
    {
        AudioSource targetSource = ambienceMusicSources[audioSourceIndex];
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
        if (clip) StartCoroutine(FadeInProcess(sfxSource, clip, duration));
    }

    /// <summary>
    /// Fades out general sound effects over a specified duration.
    /// </summary>
    /// <param name="duration">Duration of the fade-out effect.</param>
    public void FadeOutSFX(float duration) => StartCoroutine(FadeOutProcess(sfxSource, duration));

    /// <summary>
    /// Fades in UI sound effects over a specified duration.
    /// </summary>
    /// <param name="track">Track to fade in.</param>
    /// <param name="duration">Duration of the fade-in effect.</param>
    public void FadeInUISFX(UISFX track, float duration)
    {
        AudioClip clip = audioClipsSO.GetUISFXClip(track);
        if (clip) StartCoroutine(FadeInProcess(uiSource, clip, duration));
    }

    /// <summary>
    /// Fades out UI sound effects over a specified duration.
    /// </summary>
    /// <param name="duration">Duration of the fade-out effect.</param>
    public void FadeOutUISFX(float duration) => StartCoroutine(FadeOutProcess(uiSource, duration));

    /// <summary>
    /// Initiates a crossfade between the currently playing game music and a new music track.
    /// </summary>
    /// <param name="gameTrack">The game music track to crossfade to.</param>
    /// <param name="duration">The duration of the crossfade in seconds.</param>
    public void CrossFadeGameMusic(GameMusic gameTrack, float duration)
    {
        if (isCrossFadingGameMusic) return; // Prevent overlapping fades

        AudioClip newClip = audioClipsSO.GetGameMusicClip(gameTrack);
        StartCoroutine(CrossFadeGameMusicProcess(newClip, duration));
    }

    /// <summary>
    /// Initiates a crossfade between the currently playing ambience music and a new ambience track.
    /// </summary>
    /// <param name="ambienceTrack">The ambience music track to crossfade to.</param>
    /// <param name="duration">The duration of the crossfade in seconds.</param>
    public void CrossFadeAmbienceMusic(AmbienceMusic ambienceTrack, float duration)
    {
        if (isCrossFadingAmbienceMusic) return; // Prevent overlapping fades

        AudioClip newClip = audioClipsSO.GetAmbienceMusicClip(ambienceTrack);
        StartCoroutine(CrossFadeAmbienceMusicProcess(newClip, duration));
    }

    /// <summary>
    /// Transitions between two game music tracks by first fading out the current track and then fading in the new track.
    /// </summary>
    /// <param name="toTrack">The GameMusic enum representing the track to fade in to.</param>
    /// <param name="fromSourceIndex">The index of the AudioSource playing the track to fade out from.</param>
    /// <param name="toSourceIndex">The index of the AudioSource that will play the track to fade in to.</param>
    /// <param name="duration">The duration in seconds over which the fade-out and fade-in will occur.</param>
    public void TransitionGameMusicTracks(GameMusic toTrack, int fromSourceIndex, int toSourceIndex, float duration)
    {
        StartCoroutine(FadeOutProcess(gameMusicSources[fromSourceIndex], duration, () =>
        {
            StartCoroutine(FadeInProcess(gameMusicSources[toSourceIndex], audioClipsSO.GetGameMusicClip(toTrack), duration));
        }));
    }

    /// <summary>
    /// Transitions between two ambience music tracks with a fade-out and fade-in effect.
    /// </summary>
    /// <param name="toTrack">Track to fade in.</param>
    /// <param name="fromSourceIndex">The AudioSource index for the track to fade out from.</param>
    /// <param name="toSourceIndex">The AudioSource index for the track to fade in to.</param>
    /// <param name="duration">Duration of the transition.</param>
    public void TransitionAmbienceMusicTracks(AmbienceMusic toTrack, int fromSourceIndex, int toSourceIndex, float duration)
    {
        StartCoroutine(FadeOutProcess(ambienceMusicSources[fromSourceIndex], duration, () =>
        {
            StartCoroutine(FadeInProcess(ambienceMusicSources[toSourceIndex], audioClipsSO.GetAmbienceMusicClip(toTrack), duration));
        }));
    }

    #endregion


    #region Coroutines

    private IEnumerator FadeInProcess(AudioSource audioSource, AudioClip newClip, float duration)
    {
        if (fadeInCoroutine != null)
        {
            StopCoroutine(fadeInCoroutine);
            fadeInCoroutine = null;
        }

        // Check if a new audio clip is provided
        if (newClip)
        {
            // Explicitly stop any existing audio
            audioSource.Stop();  

            // Set the new audio clip and start playing
            audioSource.clip = newClip;
            audioSource.Play();
        }

        // Store the initial volume
        float startVolume = audioSource.volume;

        // Initialize volume to zero
        audioSource.volume = 0;

        // Gradually increase volume to target volume
        while (audioSource.volume < startVolume)
        {
            // Incrementally increase the volume of the audio source based on the target volume, frame time, and duration
            audioSource.volume = Mathf.MoveTowards(audioSource.volume, startVolume, (startVolume / duration) * Time.deltaTime);

            // Pause the coroutine and wait for the end of frame before continuing
            yield return null;
        }

        // Reset the volume to the initial value
        audioSource.volume = startVolume;
    }

    private IEnumerator FadeOutProcess(AudioSource audioSource, float duration, Action onFinished = null)
    {
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
            fadeOutCoroutine = null;
        }

        // Store the initial volume
        float startVolume = audioSource.volume;

        // Gradually reduce the volume to zero
        while (audioSource.volume > 0)
        {
            audioSource.volume = Mathf.MoveTowards(audioSource.volume, 0, (startVolume / duration) * Time.deltaTime);

            yield return null;
        }

        // Stop and reset the volume to the initial value
        audioSource.Stop();
        audioSource.volume = startVolume;

        // Invoke any callback actions
        onFinished?.Invoke();
    }

    private IEnumerator CrossFadeGameMusicProcess(AudioClip newClip, float duration)
    {
        isCrossFadingGameMusic = true;

        AudioSource activeSource = (gameMusicSources[0].isPlaying) ? gameMusicSources[0] : gameMusicSources[1];
        AudioSource inactiveSource = (activeSource == gameMusicSources[0]) ? gameMusicSources[1] : gameMusicSources[0];

        // Capture the current volume of the active source
        float currentVolume = activeSource.volume;

        inactiveSource.clip = newClip;
        inactiveSource.Play();
        inactiveSource.volume = 0;

        float halfDuration = duration / 2.0f;

        // Fade out the active source and fade in the inactive source
        for (float t = 0; t < halfDuration; t += Time.deltaTime)
        {
            activeSource.volume = Mathf.Lerp(currentVolume, 0, t / halfDuration);
            inactiveSource.volume = Mathf.Lerp(0, currentVolume, t / halfDuration);

            yield return null;
        }

        // Stop the active source and make the inactive source the new active source
        activeSource.Stop();
        activeSource.volume = currentVolume; 
        inactiveSource.volume = currentVolume;

        isCrossFadingGameMusic = false;
    }

    private IEnumerator CrossFadeAmbienceMusicProcess(AudioClip newClip, float duration)
    {
        isCrossFadingAmbienceMusic = true;

        AudioSource activeSource = (ambienceMusicSources[0].isPlaying) ? ambienceMusicSources[0] : ambienceMusicSources[1];
        AudioSource inactiveSource = (activeSource == ambienceMusicSources[0]) ? ambienceMusicSources[1] : ambienceMusicSources[0];

        // Capture the current volume of the active source
        float currentVolume = activeSource.volume;

        inactiveSource.clip = newClip;
        inactiveSource.Play();
        inactiveSource.volume = 0;

        float halfDuration = duration / 2.0f;

        // Fade out the active source and fade in the inactive source
        for (float t = 0; t < halfDuration; t += Time.deltaTime)
        {
            activeSource.volume = Mathf.Lerp(currentVolume, 0, t / halfDuration);
            inactiveSource.volume = Mathf.Lerp(0, currentVolume, t / halfDuration);

            yield return null;
        }

        // Stop the active source and make the inactive source the new active source
        activeSource.Stop();
        activeSource.volume = currentVolume; 
        inactiveSource.volume = currentVolume;

        isCrossFadingAmbienceMusic = false;
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
