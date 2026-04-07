using TMPro;
using UnityEngine;

public class GoalCard : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI _goalAmountTxt;

    public void InitializeText(int firstAmount)
    {
        _goalAmountTxt.text = firstAmount.ToString();
    }

    public void UpdateGoalText(int amount)
    {
        _goalAmountTxt.text = amount.ToString();
    }

    public void CompleteGoal()
    {
        gameObject.SetActive(false);
    }
}
