using UnityEngine;

public class InteractionManagement : MonoBehaviour
{
    public static InteractionManagement Instance;

    private void Awake()
    {
        Instance = this;
    }


    [Header("Cameras & Player")]
    public Camera playerCamera;                 // FPS camera (child of player)
    public Camera lockCamera;                   // close-up lock camera (disabled at start)
    public MonoBehaviour playerController;      // your FirstPersonController component

    [Header("Interaction")]
    public float interactDistance = 3f;
    public LayerMask interactMask = ~0;         // adjust if you want to filter layers

    // runtime state
    private bool inLockView = false;
    private LockInteraction currentLock = null;


    void Start()
    {
        if (playerCamera != null) playerCamera.enabled = true;
        if (lockCamera != null) lockCamera.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!inLockView)
        {
            if (Input.GetMouseButtonDown(0))
                HandlePlayerClick();
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
                HandleLockViewClick();

            if (Input.GetKeyDown(KeyCode.Escape))
                ExitLockView();
        }
    }

    // Player mode clicks: only enter lock view if we clicked a LockInteraction (any child or parent)
    private void HandlePlayerClick()
    {
        if (playerCamera == null) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactMask))
        {
            // PRIORITISE LockInteraction: if this collider is part of a lock, open lock view.
            LockInteraction lockScript = hit.collider.GetComponentInParent<LockInteraction>();
            if (lockScript != null)
            {
                EnterLockView(lockScript);
                return; // important: do NOT rotate dials in player mode
            }

            // If it's not part of a lock, you can still rotate standalone dials here if desired:
            // Dial dial = hit.collider.GetComponentInParent<Dial>();
            // if (dial != null) dial.Rotate();
        }
    }

    private void HandleLockViewClick()
    {
        if (lockCamera == null) return;

        // Cast a ray from the mouse position through the lock camera
        Ray ray = lockCamera.ScreenPointToRay(Input.mousePosition);
        float maxDistance = interactDistance * 10f;

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactMask))
        {
            Dial dial = hit.collider.GetComponentInParent<Dial>();
            if (dial != null)
            {
                Debug.Log($"LockView: clicked dial '{dial.name}' (collider: {hit.collider.name})");
                dial.Rotate();
            }
            else
            {
                Debug.Log($"LockView: hit '{hit.collider.name}' but no Dial component found on parent.");
            }
        }
        else
        {
            Debug.Log("LockView: click hit nothing.");
        }
    }

    public void EnterLockView(LockInteraction lockScript)
    {
        Debug.Log($"EnterLockView called. playerCamera={playerCamera?.name}, lockCamera={lockCamera?.name}");

        if (playerCamera == null)
        {
            Debug.LogError("EnterLockView aborted: playerCamera not assigned!");
            return;
        }

        if (lockCamera == null)
        {
            Debug.LogError("EnterLockView aborted: lockCamera not assigned! Keeping player camera.");
            return;
        }

        // Safety: print some useful camera state info
        Debug.Log($"Before switch -> player.activeInHierarchy={playerCamera.gameObject.activeInHierarchy}, player.enabled={playerCamera.enabled}, lock.activeInHierarchy={lockCamera.gameObject.activeInHierarchy}, lock.enabled={lockCamera.enabled}, lock.targetDisplay={lockCamera.targetDisplay}");

        // Ensure lock camera GameObject is active (in case it was accidentally deactivated)
        if (!lockCamera.gameObject.activeInHierarchy)
            lockCamera.gameObject.SetActive(true);

        // Disable player camera component (not entire player GameObject)
        playerCamera.enabled = false;

        // Enable lock camera component
        lockCamera.enabled = true;

        // Extra safety: if after enabling the lock camera it still isn't active, revert to player camera
        if (!lockCamera.enabled)
        {
            Debug.LogWarning("lockCamera.enabled is still false after attempt to enable it. Re-enabling player camera to avoid no-camera state.");
            playerCamera.enabled = true;
            return;
        }

        // show cursor & disable player controller
        if (playerController != null) playerController.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        inLockView = true;
        currentLock = lockScript;

        Debug.Log($"After switch -> player.enabled={playerCamera.enabled}, lock.enabled={lockCamera.enabled}");
    }

    public void ExitLockView()
    {
        Debug.Log("ExitLockView called");

        // Make sure we always leave at least one camera enabled
        if (playerCamera != null) playerCamera.enabled = true;
        if (lockCamera != null) lockCamera.enabled = false;

        if (playerController != null) playerController.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inLockView = false;
        currentLock = null;

        Debug.Log($"Exit done -> player.enabled={playerCamera?.enabled}, lock.enabled={lockCamera?.enabled}");
    }
}
