using UnityEngine;


[CreateAssetMenu(fileName = "NewMenu", menuName = "Scriptable Objects/Game Data/Menu")]
public class MenuSceneSO : GameSceneSO
{
    // Choose which type of menu from the editor
    [Header("Menu specific")]
    [SerializeField] private MenuType type;
}


public enum MenuType
{
    Main_Menu,
    Settings_Menu,
    LoadGame_Menu,
    Pause_Menu
    
    // Add more menus here
}