using UnityEngine;
using UnityEngine.UI;


public class SavedGameItem : MonoBehaviour
{
    [SerializeField] private Text saveNameText;
    [SerializeField] private Text saveDateText;
    [SerializeField] private Text saveTimeText;

    public void Initialize(SavedGame savedGame)
    {
        saveNameText.text = savedGame.saveName;
        saveDateText.text = savedGame.saveDate;
        saveTimeText.text = savedGame.saveTime;
    }

    public void PlayButtonHover()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonHover1);
    }
}