using System;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress
{
    [SerializeField] FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] BurningRecipeSO[] burningRecipeSOArray;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs: EventArgs {
        public State state;
    }

    public enum State {
        Idle,
        Frying,
        Fried,
        Burnt
    }
    private State state;

    public event EventHandler<IHasProgress.OnProgressChangedArgs> OnProgressChanged;


    private void Start() {
        state = State.Idle;
    }



    private void Update() {
        StoveWork();
        
    }

    private void StoveWork() {
        if (HasKitchenObject()) {
            switch (state) {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimeMax
                    });
                    if (fryingTimer >= fryingRecipeSO.fryingTimeMax) {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        state = State.Fried;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                            state = this.state
                        });
                        
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                        Debug.Log("Fried");
                        burningTimer = 0f;
                    }
                    
                    //Debug.Log(burningRecipeSO);
                    break;

                case State.Fried:
                    //Debug.Log(burningRecipeSO);
                    burningTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs {
                        progressNormalized = burningTimer / burningRecipeSO.burningTimeMax
                    });
                    if (burningTimer >= burningRecipeSO.burningTimeMax) {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        state = State.Burnt;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                            state = this.state
                        });
                        
                        Debug.Log("Burnt");
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs {
                            progressNormalized = 0f
                        });
                    }
                   
                    break;
                case State.Burnt:
                    break;
            }
            Debug.Log(state);
        }
        }

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {

            if (player.HasKitchenObject()) {

                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {

                    //drop kitchen object
                    state = State.Frying;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                        state = this.state
                    });
                    fryingTimer = 0f;
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    


                }
            }
        }
        else {
            if (!player.HasKitchenObject()) {
                //pick up
                this.GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                    state = this.state
                });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs {
                    progressNormalized = 0f
                });

            }
        }
    }


    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);

        if (fryingRecipeSO != null) {
            return true;
        }
        return false;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {

        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);

        if (fryingRecipeSO != null) {
            return fryingRecipeSO.output;
        }

        return null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray) {
            if (inputKitchenObjectSO == fryingRecipeSO.input) {


                return fryingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray) {
            //Debug.Log(burningRecipeSO);
            //Debug.Log(burningRecipeSO.input);
            //Debug.Log(inputKitchenObjectSO);
            if (inputKitchenObjectSO == burningRecipeSO.input) {

                return burningRecipeSO;
            }
        }
        return null;
    }


    public override void InteractAlternate(Player player) {
        base.InteractAlternate(player);
    }
}
