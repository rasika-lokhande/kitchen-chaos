using System;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.LightTransport.InputExtraction;

public class Player : MonoBehaviour, IKitchenObjectParent {
    public static Player Instance { get; private set; }

    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float rotateSpeed = 10f;

    [SerializeField] private LayerMask countersLayerMask;

    [SerializeField] private bool isWalking;
    [SerializeField] private Vector3 lastInteractDir;


    [SerializeField] private InputManager inputManager;

    private Vector2 inputVector;

    public event EventHandler<OnSelectedCounterChangeArgs> OnSelectedCounterChange;
    public class OnSelectedCounterChangeArgs : EventArgs {
        public BaseCounter selectedCounter;
    }


    [SerializeField] private BaseCounter selectedCounter;

    [SerializeField] private Transform kitchenObjectHoldPoint;
    [SerializeField] private KitchenObject kitchenObject;


    private void Awake() {
        Instance = this;

    }

    private void Start() {
        inputManager.OnInteraction += InputManager_OnInteraction;
        inputManager.OnInteractAlternateAction += InputManager_OnInteractAlternateAction;
    }

    private void InputManager_OnInteractAlternateAction(object sender, EventArgs e) {
        if (selectedCounter != null) {

            selectedCounter.InteractAlternate(this);
        }
    }

    private void InputManager_OnInteraction(object sender, EventArgs e) {
        //subscirbed to event OnInteraction
   
        if (selectedCounter != null) {
           
            selectedCounter.Interact(this);
        }
    }

    private void Update() {
        MovePlayer();
        HandleInteractions();
    }

    public bool IsWalking() {
        return isWalking;
    }

    private void MovePlayer() {
        inputVector = inputManager.GetMovementVectorNormalized();



        Vector3 movDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        //bool canMove = !Physics.Raycast(transform.position, movDir, playerRadius);
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + (Vector3.up * playerHeight), playerRadius, movDir, moveDistance);

        if (canMove) {
            transform.position += movDir * Time.deltaTime * moveSpeed;
        }
        isWalking = movDir.magnitude != 0f;
        transform.forward = Vector3.Slerp(transform.forward, movDir, Time.deltaTime * rotateSpeed);

        // Debug.Log(inputVector);

    }



    private void HandleInteractions() {
        Vector2 inputVector = inputManager.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask)) {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {
                // Has ClearCounter
                if (baseCounter != selectedCounter) {
                    SetSelectedCounter(baseCounter);
                    //Debug.Log("selected couter set: " + baseCounter);
                }
            }
            else {
                SetSelectedCounter(null);

            }
        }
        else {
            SetSelectedCounter(null);
        }
    }

    private void SetSelectedCounter(BaseCounter selectedCounter) {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChange?.Invoke(this, new OnSelectedCounterChangeArgs {
            selectedCounter = selectedCounter
        });
    }


    public Transform GetKitchObjectFollowTransform() {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {

        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject() { return kitchenObject; }

    public void ClearKitchenObject() {
        kitchenObject = null;
    }

    public bool HasKitchenObject() { return kitchenObject != null; }



}