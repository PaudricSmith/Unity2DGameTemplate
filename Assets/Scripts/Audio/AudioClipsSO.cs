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
    public class MenuMusicCategory
    {
        public AudioTrack<DAM.MenuMusic>[] tracks;
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
    public MenuMusicCategory menuTracks;
    public GameMusicCategory gameTracks;
    public AmbienceMusicCategory ambienceTracks;

    [Header("SFX Tracks")]
    public UiSfxCategory uiTracks;
    public PlayerSfxCategory playerTracks;
    public EnemySfxCategory enemyTracks;


    /// <summary>
    /// Dictionary to map MenuMusic types to their audio clips.
    /// </summary>
    private Dictionary<DAM.MenuMusic, AudioClip> menuMusicDict;
    private Dictionary<DAM.GameMusic, AudioClip> gameMusicDict;
    private Dictionary<DAM.AmbienceMusic, AudioClip> ambienceMusicDict;
    private Dictionary<DAM.UISFX, AudioClip> uiSfxDict;
    private Dictionary<DAM.PlayerSFX, AudioClip> playerSfxDict;
    private Dictionary<DAM.EnemySFX, AudioClip> enemySfxDict;


    /// <summary>
    /// Initializes dictionaries for each category of audio tracks.
    /// </summary>
    private void OnEnable()
    {
        menuMusicDict = CreateDictionary(menuTracks.tracks);
        gameMusicDict = CreateDictionary(gameTracks.tracks);
        ambienceMusicDict = CreateDictionary(ambienceTracks.tracks);
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


    /// <summary>
    /// Retrieves the AudioClip associated with a given audio track type.
    /// </summary>
    /// <typeparam name="T">The type of the audio track.</typeparam>
    /// <param name="track">The audio track type.</param>
    /// <returns>The associated AudioClip, or null if not found.</returns>
    public AudioClip GetClip<T>(T track)
    {
        if (track is DAM.MenuMusic)
        {
            return menuMusicDict[(DAM.MenuMusic)(object)track];
        }
        else if (track is DAM.GameMusic)
        {
            return gameMusicDict[(DAM.GameMusic)(object)track];
        }
        else if (track is DAM.AmbienceMusic)
        {
            return ambienceMusicDict[(DAM.AmbienceMusic)(object)track];
        }
        else if (track is DAM.UISFX)
        {
            return uiSfxDict[(DAM.UISFX)(object)track];
        }
        else if (track is DAM.PlayerSFX)
        {
            return playerSfxDict[(DAM.PlayerSFX)(object)track];
        }
        else if (track is DAM.EnemySFX)
        {
            return enemySfxDict[(DAM.EnemySFX)(object)track];
        }
        else
        {
            Debug.LogWarning($"No audio clip found for the given track: {track}");
            return null;
        }
    }
}
