using UnityEngine;


[CreateAssetMenu(fileName = "NewMenu", menuName = "Scriptable Objects/Game Data/Menu")]
public class MenuSceneSO : GameSceneSO
{

    // Choose which type of menu from the editor
    [Header("Menu specific")]
    [SerializeField] private MenuType menuType;


    public MenuType MenuType { get => menuType; }
}


public enum MenuType
{
    Main_Menu,
    Main_Menu_Settings,
    LoadGame_Menu,
    Pause_Menu,
    Pause_Menu_Settings
    
    // Add more menus here
}