using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{

    private PlayerInputActions playerInputActions;

    public event EventHandler OnInteraction;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Interact.performed += Interact_performed; // E Key is pressed
        
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        // Following Event is triggered if E key is pressed
        OnInteraction?.Invoke(this, EventArgs.Empty);
    }

    private void Update() {
        

    }


    public Vector2 GetMovementVectorNormalized() {

        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
     // Debug.Log(inputVector);
        return inputVector;

    }

   

  

    


}
