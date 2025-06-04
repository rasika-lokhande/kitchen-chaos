using System;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Text;
using UnityEngine;
using UnityEngine.EventSystems;

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
        public ClearCounter selectedCounter;
    }


    [SerializeField] private ClearCounter selectedCounter;

    [SerializeField] private Transform kitchenObjectHoldPoint;
    [SerializeField] private KitchenObject kitchenObject;


    private void Awake() {
        Instance = this;

    }

    private void Start() {
        inputManager.OnInteraction += InputManager_OnInteraction;
    }

    private void InputManager_OnInteraction(object sender, EventArgs e) {
        //subscirbed to event OnInteraction

        inputVector = inputManager.GetMovementVectorNormalized();
        Vector3 movDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (movDir.magnitude != 0f) {
            lastInteractDir = movDir;
        }

        float interactDistance = 2f;

        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask)) {
            //Debug.Log(raycastHit.transform);
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter)) {

                clearCounter.Interact(this);
                setSelectedCounter(clearCounter);

            }
            else {
                //selectedCounter = null;
                setSelectedCounter(null);
            }
        }
        else {
            setSelectedCounter(null);
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
        inputVector = inputManager.GetMovementVectorNormalized();
        Vector3 movDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (movDir.magnitude != 0f) {
            lastInteractDir = movDir;
        }

        float interactDistance = 2f;

        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask)) {
            //Debug.Log(raycastHit.transform);
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter)) {


                setSelectedCounter(clearCounter);

            }
            else {

                setSelectedCounter(null);
            }
        }
        else {
            setSelectedCounter(null);
        }
    }

    private void setSelectedCounter(ClearCounter selectedCounter) {

        OnSelectedCounterChange?.Invoke(this, new OnSelectedCounterChangeArgs { selectedCounter = selectedCounter });

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