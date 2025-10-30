using UnityEngine;
using UnityEngine.Events;

public class Shackle : MonoBehaviour
{
    [Header(" Animation Settings ")]
    [SerializeField] private float yMovement;
    [SerializeField] private float yMovementDuration;
    [SerializeField] private float rotationAngle;
    [SerializeField] private float rotationDuration;

    [Header(" Events ")]
    [SerializeField] private UnityEvent onShackleOpened;

    public void Open()
    {
        // Open the Shackle
        LeanTween.moveLocalY(gameObject, yMovement, yMovementDuration).setEase(LeanTweenType.easeOutBack).setOnComplete(
            () => LeanTween.rotateAroundLocal(gameObject, Vector3.up, rotationAngle, rotationDuration)
                .setEase(LeanTweenType.easeOutBack)
                .setOnComplete(() => onShackleOpened?.Invoke())   // <-- TRIGGER EVENT AFTER FULL ANIMATION
        );
    }
}