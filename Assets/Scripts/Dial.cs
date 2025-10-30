using UnityEngine;
using UnityEngine.Events;

public class Dial : MonoBehaviour
{
    [Header(" Settings ")]
    [SerializeField] private float animationDuration;
    private bool isRotating = false;
    private int currentIndex;

    [Header(" Events ")]
    [SerializeField] private UnityEvent<Dial> onDialRotated;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Determine currentIndex based on the dial's rotation in the scene
        float angle = transform.localRotation.eulerAngles.x;

        // Convert angle back into index (round to nearest)
        currentIndex = Mathf.RoundToInt(-angle / 60f) % 6;
    }

    public void Rotate()
    {
        // If the dial is already rotating, do nothing and escape current method
        if (isRotating)
            return;
        
        // Otherwise, set the dial as rotating to prevent rotating twice
        isRotating = true;

        // Increase the current dial index / number / symbol
        currentIndex++;
        
        //If the number is greater than 6, reset it
        if (currentIndex >= 6)
            currentIndex = 0;

        // Cancel the previous tween on this object, and rotate
        // 360° / 6 numbers = 60°, rotate 60° around the local right axis
        // After the rotation, call the " RotationCompleteCallback " method
        LeanTween.cancel(gameObject);
        LeanTween.rotateAroundLocal(gameObject, Vector3.right, -60, animationDuration).setOnComplete(RotationCompleteCallback);
    }
    private void RotationCompleteCallback()
    {
        // Trigger the " onDialRotated " event to let the combination lock know that this specific dial has rotated
        onDialRotated?.Invoke(this);
    }

    public int GetNumber()
    {
        // Return the current number of the dial
        return currentIndex;
    }

    public void Lock()
    {
        // Prevents the dial from rotating
        isRotating = true;
    }

    public void Unlock()
    {
        // Allows the dial to rotate
        isRotating = false;
    }


}
