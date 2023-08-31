using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Scriptable object that manages audio clips for different categories of music and sound effects.
/// </summary>
[CreateAssetMenu(fileName = "AudioManager", menuName = "Scriptable Objects/Audio/Audio Clips")]
public class AudioClipsSO : ScriptableObject
{
    /// <summary>
    /// Represents an audio track with a type and associated AudioClip.
    /// </summary>
    /// <typeparam name="T">The type of the audio track.</typeparam>
    [System.Serializable]
    public class AudioTrack<T>
    {
        public T track;
        public AudioClip clip;
    }

    [System.Serializable]
    public class GameMusicCategory
    {
        public AudioTrack<DAM.GameMusic>[] tracks;
    }

    [System.Serializable]
    public class AmbienceMusicCategory
    {
        public AudioTrack<DAM.AmbienceMusic>[] tracks;
    }

    [System.Serializable]
    public class SfxCategory
    {
        public AudioTrack<DAM.SFX>[] tracks;
    }

    [System.Serializable]
    public class UiSfxCategory
    {
        public AudioTrack<DAM.UISFX>[] tracks;
    }

    [System.Serializable]
    public class PlayerSfxCategory
    {
        public AudioTrack<DAM.PlayerSFX>[] tracks;
    }

    [System.Serializable]
    public class EnemySfxCategory
    {
        public AudioTrack<DAM.EnemySFX>[] tracks;
    }


    [Header("Music Tracks")]
    public GameMusicCategory gameTracks;
    public AmbienceMusicCategory ambienceTracks;

    [Header("SFX Tracks")]
    public SfxCategory sfxTracks;
    public UiSfxCategory uiTracks;
    public PlayerSfxCategory playerTracks;
    public EnemySfxCategory enemyTracks;


    /// <summary>
    /// Dictionary to map audio types to their audio clips.
    /// </summary>
    private Dictionary<DAM.GameMusic, AudioClip> gameMusicDict;
    private Dictionary<DAM.AmbienceMusic, AudioClip> ambienceMusicDict;
    private Dictionary<DAM.SFX, AudioClip> sfxDict;
    private Dictionary<DAM.UISFX, AudioClip> uiSfxDict;
    private Dictionary<DAM.PlayerSFX, AudioClip> playerSfxDict;
    private Dictionary<DAM.EnemySFX, AudioClip> enemySfxDict;


    /// <summary>
    /// Initializes dictionaries for each category of audio tracks.
    /// </summary>
    private void OnEnable()
    {
        gameMusicDict = CreateDictionary(gameTracks.tracks);
        ambienceMusicDict = CreateDictionary(ambienceTracks.tracks);
        sfxDict = CreateDictionary(sfxTracks.tracks);
        uiSfxDict = CreateDictionary(uiTracks.tracks);
        playerSfxDict = CreateDictionary(playerTracks.tracks);
        enemySfxDict = CreateDictionary(enemyTracks.tracks);
    }


    /// <summary>
    /// Creates a dictionary to map audio track types to their clips.
    /// </summary>
    /// <typeparam name="T">The type of the audio track.</typeparam>
    /// <param name="tracks">Array of audio tracks.</param>
    /// <returns>A dictionary mapping audio track types to clips.</returns>
    private Dictionary<T, AudioClip> CreateDictionary<T>(AudioTrack<T>[] tracks)
    {
        Dictionary<T, AudioClip> dict = new Dictionary<T, AudioClip>();
        foreach (var track in tracks)
        {
            dict[track.track] = track.clip;
        }
        return dict;
    }


    public AudioClip GetGameMusicClip(DAM.GameMusic track)
    {
        if (!gameMusicDict[(DAM.GameMusic)(object)track])
        {
            Debug.LogWarning($"No audio clip found for the given track: {track}");
            return null;
        }

        return gameMusicDict[(DAM.GameMusic)(object)track];
    }
    
    public AudioClip GetAmbienceMusicClip(DAM.AmbienceMusic track)
    {
        if (!ambienceMusicDict[(DAM.AmbienceMusic)(object)track])
        {
            Debug.LogWarning($"No audio clip found for the given track: {track}");
            return null;
        }

        return ambienceMusicDict[(DAM.AmbienceMusic)(object)track];
    }
    
    public AudioClip GetSFXClip(DAM.SFX track)
    {
        if (!sfxDict[(DAM.SFX)(object)track])
        {
            Debug.LogWarning($"No audio clip found for the given track: {track}");
            return null;
        }

        return sfxDict[(DAM.SFX)(object)track];
    }

    public AudioClip GetUISFXClip(DAM.UISFX track)
    {
        if (!uiSfxDict[(DAM.UISFX)(object)track])
        {
            Debug.LogWarning($"No audio clip found for the given track: {track}");
            return null;
        }

        return uiSfxDict[(DAM.UISFX)(object)track];
    }


    /// <summary>
    /// Retrieves the AudioClip associated with a given audio track type.
    /// </summary>
    /// <typeparam name="T">The type of the audio track.</typeparam>
    /// <param name="track">The audio track type.</param>
    /// <returns>The associated AudioClip, or null if not found.</returns>
    public AudioClip GetClip<T>(T track)
    {
        switch (track)
        {
            case DAM.GameMusic:
                return gameMusicDict[(DAM.GameMusic)(object)track];

            case DAM.AmbienceMusic:
                return ambienceMusicDict[(DAM.AmbienceMusic)(object)track];

            case DAM.UISFX:
                return uiSfxDict[(DAM.UISFX)(object)track];

            case DAM.SFX:
                return sfxDict[(DAM.SFX)(object)track];

            case DAM.PlayerSFX:
                return playerSfxDict[(DAM.PlayerSFX)(object)track];

            case DAM.EnemySFX:
                return enemySfxDict[(DAM.EnemySFX)(object)track];

            default:
                Debug.LogWarning($"No audio clip found for the given track: {track}");
                return null;
        }
    }
}
