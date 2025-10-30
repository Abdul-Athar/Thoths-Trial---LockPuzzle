using UnityEngine;

public class LockInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public Camera lockCamera;
    public MonoBehaviour playerMovementScript;

    void Update()
    {
        if (lockCamera.enabled && Input.GetKeyDown(KeyCode.Escape))
            ExitLock();
    }

    public void InteractWithLock()
    {
        // Switch to lock view
        playerCamera.enabled = false;
        lockCamera.enabled = true;

        // Disable player control
        playerMovementScript.enabled = false;
    }

    public void ExitLock()
    {
        // Switch back to FPS view
        lockCamera.enabled = false;
        playerCamera.enabled = true;

        // Enable player control again
        playerMovementScript.enabled = true;
    }
}
