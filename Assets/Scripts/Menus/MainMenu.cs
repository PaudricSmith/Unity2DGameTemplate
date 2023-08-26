using UnityEngine;


public class MainMenu : MonoBehaviour
{

    private void Awake()
    {
        // Set the resolution to 1920x1080 and enable fullscreen
        Screen.SetResolution(1920, 1080, true);
    }


    private void Start()
    {
        // DAM.Instance.FadeInMusic(DAM.GameMusic.Level1Track1, 2.0f);
    }
}