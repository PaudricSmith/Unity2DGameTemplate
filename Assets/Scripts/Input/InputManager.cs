using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    private GameControls gameControls;
    private Vector2 moveInput;
    private Vector2 aimInput;

    [SerializeField] private GameSettingsDataSO gameSettingsSO;
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

    void Update()
    {
        // Use the moveInput and aimInput vectors to control your character
        // For example:
        //transform.position += new Vector3(moveInput.x, moveInput.y, 0) * Time.deltaTime;

        // Aiming logic
        Vector3 aimDirection = new Vector3(aimInput.x, aimInput.y, 0);
        // Implement your aiming logic here, using aimDirection
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
            case GameState.Inventory:
                CloseInventory();
                break;
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
        // Your pause logic here
        // For example, you could set Time.timeScale to 0 to pause the game
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
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
        if (gameSettingsSO.isGamepadEnabled)
        {
            gameControls.devices = new InputDevice[] { Gamepad.current };
        }
        else
        {
            gameControls.devices = new InputDevice[] { Keyboard.current, Mouse.current };
        }
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