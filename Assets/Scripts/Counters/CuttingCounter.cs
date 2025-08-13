using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    private int cuttingProgress = 0;
    private float cuttingProgressNormalized = 1;

    public event EventHandler<IHasProgress.OnProgressChangedArgs> OnProgressChanged;
    

    public event EventHandler OnCut;


    public override void Interact(Player player) {

        if (!HasKitchenObject()) {

            if (player.HasKitchenObject()) {

                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {

                    //drop kitchen object
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    cuttingProgress = 0;
                    cuttingProgressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs() {
                        progressNormalized = cuttingProgressNormalized
                    });
                   
                    
                }
            }

        }
        else {
            if (!player.HasKitchenObject()) {
                //pick up
                this.GetKitchenObject().SetKitchenObjectParent(player);
              
            }
        }

    }
    public override void InteractAlternate(Player player) {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {
            cuttingProgress += 1;
            OnCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            cuttingProgressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs() {
                progressNormalized = cuttingProgressNormalized
            });
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
               

            }
           

        }

    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);

        if (cuttingRecipeSO != null) {
        return true;
        }
        return false;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {

        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);

        if (cuttingRecipeSO != null) { 
        return cuttingRecipeSO.output;
        }
       
        return null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (inputKitchenObjectSO == cuttingRecipeSO.input) {


                return cuttingRecipeSO;
            }
        }
        return null;
    }


   
}
