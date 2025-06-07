using UnityEngine;
using System;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    public event EventHandler OnPlayerGraggebObject;

    public override void Interact(Player player) {


        if (!player.HasKitchenObject()) { 
        KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
        OnPlayerGraggebObject?.Invoke(this, EventArgs.Empty);

        }
        else {
            //player is carrying a kitchen object
        }
    }

    public override void InteractAlternate(Player player) {


    }

}
