using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button saveButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private GameManagerSO gameManager;

    private void Start()
    {
        //saveButton.onClick.AddListener(gameManager.SaveGame);  // Implement SaveGame in GameManagerSO
        resumeButton.onClick.AddListener(gameManager.ResumeGame);
        exitButton.onClick.AddListener(gameManager.QuitGame);

        gameObject.SetActive(false);
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TogglePauseMenu();
        }
    }

    private void TogglePauseMenu()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        if (gameObject.activeSelf)
        {
            gameManager.PauseGame();
        }
        else
        {
            gameManager.ResumeGame();
        }
    }
}