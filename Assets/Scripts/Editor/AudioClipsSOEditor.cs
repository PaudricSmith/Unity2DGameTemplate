using UnityEditor;
using UnityEngine;


/// <summary>
/// Custom editor for the AudioClipsSO ScriptableObject.
/// </summary>
[CustomEditor(typeof(AudioClipsSO))]
public class AudioClipsSOEditor : Editor
{
    // Serialized properties for music and SFX categories
    private SerializedProperty[] musicCategories;
    private SerializedProperty[] sfxCategories;

    // Foldout flags for each category
    private bool[] musicFoldouts;
    private bool[] sfxFoldouts;


    /// <summary>
    /// Called when the editor is enabled.
    /// Initializes serialized properties and foldout flags.
    /// </summary>
    private void OnEnable()
    {
        // Initialize serialized properties
        musicCategories = new SerializedProperty[]
        {
            serializedObject.FindProperty("gameTracks").FindPropertyRelative("tracks"),
            serializedObject.FindProperty("ambienceTracks").FindPropertyRelative("tracks")
        };

        sfxCategories = new SerializedProperty[]
        {
            serializedObject.FindProperty("sfxTracks").FindPropertyRelative("tracks"),
            serializedObject.FindProperty("uiTracks").FindPropertyRelative("tracks"),
            serializedObject.FindProperty("playerTracks").FindPropertyRelative("tracks"),
            serializedObject.FindProperty("enemyTracks").FindPropertyRelative("tracks")
        };

        // Initialize foldout flags
        musicFoldouts = new bool[musicCategories.Length];
        sfxFoldouts = new bool[sfxCategories.Length];
    }


    /// <summary>
    /// Overrides the Inspector GUI.
    /// </summary>
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawCategories("Music Tracks", musicCategories, musicFoldouts, 
            new System.Type[] { typeof(DAM.GameMusic), typeof(DAM.AmbienceMusic) });
        DrawCategories("SFX Tracks", sfxCategories, sfxFoldouts, 
            new System.Type[] { typeof(DAM.SFX), typeof(DAM.UISFX), typeof(DAM.PlayerSFX), typeof(DAM.EnemySFX) });

        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// Draws categories of audio tracks.
    /// </summary>
    /// <param name="label">The label for the category section.</param>
    /// <param name="categories">The serialized properties for the categories.</param>
    /// <param name="foldouts">The foldout flags for the categories.</param>
    /// <param name="enumTypes">The enum types for the categories.</param>

    private void DrawCategories(string label, SerializedProperty[] categories, bool[] foldouts, System.Type[] enumTypes)
    {
        GUIStyle centeredLabelStyle = new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter };
        EditorGUILayout.LabelField(label, centeredLabelStyle);

        for (int i = 0; i < categories.Length; i++)
        {
            ShowCategory(ref foldouts[i], categories[i], enumTypes[i]);
        }
    }

    /// <summary>
    /// Shows a single category of audio tracks.
    /// </summary>
    /// <param name="foldout">The foldout flag for the category.</param>
    /// <param name="tracksProperty">The serialized property for the category.</param>
    /// <param name="enumType">The enum type for the category.</param>

    private void ShowCategory(ref bool foldout, SerializedProperty tracksProperty, System.Type enumType)
    {
        foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, enumType.Name + " (" + tracksProperty.arraySize + ")");
        if (foldout)
        {
            DrawTracks(tracksProperty, enumType);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    /// <summary>
    /// Draws the tracks for a single category.
    /// </summary>
    /// <param name="tracksProperty">The serialized property for the tracks.</param>
    /// <param name="enumType">The enum type for the tracks.</param>

    private void DrawTracks(SerializedProperty tracksProperty, System.Type enumType)
    {
        EditorGUILayout.BeginHorizontal();

        int oldSize = tracksProperty.arraySize;
        string newSizeStr = EditorGUILayout.TextField("Size", oldSize.ToString());

        // Check if the user enters a positive number
        if (int.TryParse(newSizeStr, out int parsedSize) && parsedSize >= 0)
        {
            if (GUILayout.Button("-", GUILayout.Width(20)) && parsedSize > 0)
            {
                parsedSize--;
            }
            else if (GUILayout.Button("+", GUILayout.Width(20)))
            {
                parsedSize++;
            }

            if (parsedSize != oldSize)
            {
                if (parsedSize < tracksProperty.arraySize)
                {
                    if (EditorUtility.DisplayDialog("Confirm Reduction",
                        "Are you sure you want to reduce the array size? This will result in data loss.", "Yes", "No"))
                    {
                        tracksProperty.arraySize = parsedSize;
                    }
                }
                else
                {
                    int prevSize = tracksProperty.arraySize;
                    tracksProperty.arraySize = parsedSize;
                    for (int i = prevSize; i < parsedSize; i++)
                    {
                        SerializedProperty newTrackProperty = tracksProperty.GetArrayElementAtIndex(i);
                        SerializedProperty enumProperty = newTrackProperty.FindPropertyRelative("track");
                        enumProperty.intValue = 0; // Assuming "None" corresponds to 0 in your enum
                    }
                }
            }
        }
        else
        {
            if (GUILayout.Button("-", GUILayout.Width(20)) && oldSize > 0)
            {
                oldSize--;
                tracksProperty.arraySize = oldSize;
            }
            else if (GUILayout.Button("+", GUILayout.Width(20)))
            {
                oldSize++;
                tracksProperty.arraySize = oldSize;
            }
        }


        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        for (int i = 0; i < tracksProperty.arraySize; i++)
        {
            SerializedProperty trackProperty = tracksProperty.GetArrayElementAtIndex(i);
            SerializedProperty enumProperty = trackProperty.FindPropertyRelative("track");
            SerializedProperty clipProperty = trackProperty.FindPropertyRelative("clip");

            enumProperty.intValue = EditorGUILayout.Popup("Track", enumProperty.intValue, System.Enum.GetNames(enumType));
            EditorGUILayout.PropertyField(clipProperty);

            EditorGUILayout.Space();
        }
    }
}
