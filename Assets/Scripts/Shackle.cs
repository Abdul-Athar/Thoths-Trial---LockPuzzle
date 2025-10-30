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
                .setOnComplete(() =>
                {
                    // --- FIRST DELAY: after shackle animation, before camera exit ---
                    LeanTween.delayedCall(0.5f, () =>   // Adjust delay time here (seconds)
                    {
                        // Exit lock camera view
                        InteractionManagement.Instance.ExitLockView();

                        // --- SECOND DELAY: after camera switch, before chest lid opens ---
                        LeanTween.delayedCall(0.5f, () =>  // Adjust delay time here
                        {
                            // Trigger chest opening event
                            onShackleOpened?.Invoke();
                        });
                    });
                })
        );
    }
}