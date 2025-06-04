using UnityEngine;

public interface IKitchenObjectParent
{
    public Transform GetKitchObjectFollowTransform();
    public void SetKitchenObject(KitchenObject kitchenObject);

    public KitchenObject GetKitchenObject();

    public void ClearKitchenObject();

    public bool HasKitchenObject();
}
