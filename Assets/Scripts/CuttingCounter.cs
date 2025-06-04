using UnityEngine;

public class CuttingCounter : BaseCounter 
{
    [SerializeField] private KitchenObjectSO cutkitchenObjectSO;


    public override void Interact(Player player) {

        if (!HasKitchenObject()) {

            if (player.HasKitchenObject()) {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }

        }
        else {
            if (!player.HasKitchenObject()) {
                this.GetKitchenObject().SetKitchenObjectParent(player);
            }
        }

    }
    public override void InteractAlternate(Player player) {
        if (HasKitchenObject()) {
            GetKitchenObject().DestroySelf();
           
           KitchenObject.SpawnKitchenObject(cutkitchenObjectSO, this);
        }

    }

}
