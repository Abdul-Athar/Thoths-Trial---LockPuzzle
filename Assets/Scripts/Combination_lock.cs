using UnityEngine;
using UnityEngine.Events;

public class Combination_lock : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Dial[] dials;

    [Header(" Settings ")]
    [SerializeField] private string combination;

    [Header(" Events ")]
    [SerializeField] private UnityEvent onCorrectCombinationFound;

    public void CheckCombination(Dial dial)
    {
        // Loop through all the dials
        for (int i = 0; i < dials.Length; i++)
        {
            // Store the number the dial shoud have
            int combinationNumber = int.Parse(combination[i].ToString());

            // Check if the current dial we are checking actually has the correct digit
            if (combinationNumber != dials[i].GetNumber())
            {
                // If thats not the case, unlock the dial to allow it to rotate and return 
                dial.Unlock();
                return;
            }
        }

        // At this point, all the dials have the correct symbol
        // The combination is correct!! Yay!!
        CorrectCombination();
    }

    private void CorrectCombination()
    {
        // Lock all the dials to prevent thier rotation
        for (int i = 0; i < dials.Length; i++)
        {
            dials[i].Lock();
        }

        // Trigger the "onCorrectCombinationFound" event to OPEN the Shackle or ANYTHING ELSE HERE
        onCorrectCombinationFound?.Invoke();
    }
}