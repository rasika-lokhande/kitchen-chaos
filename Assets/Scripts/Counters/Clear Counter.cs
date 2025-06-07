using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

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
        
    }

}
