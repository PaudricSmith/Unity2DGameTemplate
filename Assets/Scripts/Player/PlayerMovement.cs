using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private PlayerDataSO playerDataSO;
    [SerializeField] private float speed = 1.0f;


    private void Start()
    {
        //transform.position = playerDataSO.PlayerPosition;
    }


    void Update()
    {
        // Get the move input from the InputManager
        Vector2 moveInput = inputManager.GetMoveInput();

        // Update the player's position
        transform.position += new Vector3(moveInput.x, moveInput.y, 0) * Time.deltaTime * speed;

        // Get the aim input from the InputManager
        Vector2 aimInput = inputManager.GetAimInput();
        // Implement your aiming logic here
    }
}