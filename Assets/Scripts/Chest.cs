using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenChest()
    {
        animator.SetTrigger("OpenChest");
    }
}
