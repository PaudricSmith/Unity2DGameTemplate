using UnityEngine;
using UnityEngine.UI;


public class SavedGameItem : MonoBehaviour
{
    [SerializeField] private Text saveNameText;
    [SerializeField] private Text saveDateTimeText;

    public void Initialize(SavedGame savedGame)
    {
        saveNameText.text = savedGame.saveName;
        saveDateTimeText.text = savedGame.saveDateTime;
    }

    public void PlayButtonHover()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonHover1);
    }
}