using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    private PlayerInputActions playerInputActions;

    public event EventHandler OnInteraction;
    public event EventHandler OnInteractAlternateAction;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Interact.performed += Interact_performed; // E Key is pressed
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed; // F is pressed


        
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        // Following Event is triggered if E key is pressed
        
        OnInteraction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        // Following Event is triggered if E key is pressed

        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }
    


    public Vector2 GetMovementVectorNormalized() {

        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
     // Debug.Log(inputVector);
        return inputVector;

    }

   

  

    


}
