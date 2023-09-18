using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    private GameControls gameControls;
    private Vector2 moveInput;
    private Vector2 aimInput;

    [SerializeField] private GameSettingsDataSO gameSettingsSO;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private GameState currentState;


    // Enum to keep track of the game's current state
    public enum GameState
    {
        Gameplay,
        Inventory,
        // Add more states as needed
    }


    void Awake()
    {
        gameSettingsSO.LoadSettings();
        DAM.One.SetAudioSettings(gameSettingsSO);


        // Initialize the PlayerControls class from the new Input System
        gameControls = new GameControls();

        InitializeControlDevices();

        // Bind the movement and aiming actions to methods
        gameControls.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        gameControls.Gameplay.Move.canceled += ctx => moveInput = Vector2.zero;

        gameControls.Gameplay.Aim.performed += ctx => aimInput = ctx.ReadValue<Vector2>();
        gameControls.Gameplay.Aim.canceled += ctx => aimInput = Vector2.zero;

        gameControls.Gameplay.Esc.performed += ctx => HandleEscKey();

    }

    // Public method to get move input
    public Vector2 GetMoveInput()
    {
        return moveInput;
    }

    // Public method to get aim input
    public Vector2 GetAimInput()
    {
        return aimInput;
    }

    // Method to handle the Esc key based on the game's current state
    private void HandleEscKey()
    {
        switch (currentState)
        {
            case GameState.Gameplay:
                TogglePause();
                break;
            //case GameState.Inventory:
            //    CloseInventory();
            //    break;
            // Add more cases as needed
            default:
                break;
        }
    }

    // Method to switch to Gamepad controls
    public void SwitchToGamepad()
    {
        gameControls.devices = new InputDevice[] { Gamepad.current };
        gameSettingsSO.isGamepadEnabled = true;
        gameSettingsSO.SaveSettings();
    }

    // Method to switch to Keyboard/Mouse controls
    public void SwitchToKeyboardMouse()
    {
        gameControls.devices = new InputDevice[] { Keyboard.current, Mouse.current };
        gameSettingsSO.isGamepadEnabled = false;
        gameSettingsSO.SaveSettings();
    }

    // Method to toggle pause
    private void TogglePause()
    {
        pauseMenu.TogglePause();
        
        //if (gameControls.Gameplay.enabled)
        //{
        //    gameControls.Gameplay.Disable();
        //}
        //else
        //{
        //    gameControls.Gameplay.Enable();
        //}

    }

    // Method to close the inventory
    private void CloseInventory()
    {
        // Your logic to close the inventory
        // ...
    }

    // Initialize control devices based on settings
    private void InitializeControlDevices()
    {
        gameControls.devices = new InputDevice[] { Keyboard.current, Mouse.current };

        //if (gameSettingsSO.isGamepadEnabled)
        //{
        //    gameControls.devices = new InputDevice[] { Gamepad.current };
        //}
        //else
        //{
        //    gameControls.devices = new InputDevice[] { Keyboard.current, Mouse.current };
        //}
    }

    void OnEnable()
    {
        gameControls.Gameplay.Enable();
        gameControls.UI.Enable();
    }

    void OnDisable()
    {
        gameControls.Gameplay.Disable();
        gameControls.UI.Disable();
    }
}