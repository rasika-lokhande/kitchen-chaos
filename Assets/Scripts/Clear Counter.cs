using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent
{
    //[SerializeField] private InputManager inputManager;
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;
    //[SerializeField] private ClearCounter secondClearCounter;

    private KitchenObject kitchenObject;

    [SerializeField]private bool testing;


    private void Update() {
        //if (testing && Input.GetKeyDown(KeyCode.T)) {
        //    kitchenObject.SetKitchenObjectParent(secondClearCounter);

        //}
    }

    public void Interact(Player player) {

        if (kitchenObject == null) {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
           
        }

        else {
            //give object to player
            kitchenObject.SetKitchenObjectParent(player);
        }
      
    }

    public Transform GetKitchObjectFollowTransform() {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {

        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject() { return kitchenObject; }

    public void ClearKitchenObject() {
        kitchenObject = null;
    }

    public bool HasKitchenObject() {return kitchenObject != null;}

}
