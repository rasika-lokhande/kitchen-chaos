using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    private Animator animator;
    private const string OPEN_CLOSE = "OpenClose";
    [SerializeField] private ContainerCounter containerCounter;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        containerCounter.OnPlayerGraggebObject += ContainerCounter_OnPlayerGraggebObject
            ;
    }

    private void ContainerCounter_OnPlayerGraggebObject(object sender, System.EventArgs e) {
        animator.SetTrigger(OPEN_CLOSE);
    }
}
