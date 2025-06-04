using System;
using UnityEngine;

public class ChangeSelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private ClearCounter clearCounter;
    [SerializeField] private GameObject clearCounterVisual;
    private void Start() {
        Player.Instance.OnSelectedCounterChange += ChangeCounterVisual;
    }

    private void ChangeCounterVisual(object sender, Player.OnSelectedCounterChangeArgs e) {
        if (e.selectedCounter == clearCounter) {
            Show();
        }
        else {
            Hide();
        }
    }

    private void Show() {
        clearCounterVisual.SetActive(true);
    }

    private void Hide() {
        clearCounterVisual.SetActive(false);
    }
}
