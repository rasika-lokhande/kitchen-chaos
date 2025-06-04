using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private const string IS_WALKING = "IsWalking";

    [SerializeField] private Player player;

    private void Awake() {
        animator = GetComponent<Animator>(); // get Animator component attached to this game object that this script is attached to
        
    }

    private void Update() {
        animator.SetBool(IS_WALKING, player.IsWalking());
    }
}
