using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform _goalCardParent;
    [SerializeField] private GoalCard _goalCardPrefab;

    private ItemLevelData[] _itemDataGoals;
    private List<GoalCard> _goalCards = new List<GoalCard>();

    private void OnEnable()
    {
        LevelManager.OnLevelSpawned += HandleLevelSpawned;
        ItemSpotManager.OnItemSelected += HandleItemSelected;
    }

    private void OnDisable()
    {
        LevelManager.OnLevelSpawned -= HandleLevelSpawned;
        ItemSpotManager.OnItemSelected -= HandleItemSelected;
    }

    private void HandleLevelSpawned(LevelController level)
    {
        _itemDataGoals = level.GetLevelGoals();

        GenerateGoalCards();
    }

    private void GenerateGoalCards()
    {
        for (int i = 0; i < _itemDataGoals.Length; i++)
        {
            GenerateGoalCard(_itemDataGoals[i]);
        }
    }

    private void GenerateGoalCard(ItemLevelData goalData)
    {
        GoalCard goalCard = Instantiate(_goalCardPrefab, _goalCardParent);
        goalCard.InitializeCard(goalData.itemAmount, goalData.itemPrefab.ItemIcon);
        _goalCards.Add(goalCard);
    }

    private void HandleItemSelected(Item item)
    {
        for (int i = 0; i < _itemDataGoals.Length; i++)
        {
            if (!_itemDataGoals[i].itemPrefab.ItemType.Equals(item.ItemType))
                continue;

            _itemDataGoals[i].itemAmount--;

            if (_itemDataGoals[i].itemAmount <= 0)
                CompleteGoal(i);
            else
                _goalCards[i].UpdateGoalText(_itemDataGoals[i].itemAmount);

            break;
        }
    }

    private void CompleteGoal(int goalIndex)
    {
        Debug.Log($"Goal Completed: {_itemDataGoals[goalIndex].itemPrefab.ItemType}");
        _goalCards[goalIndex].CompleteGoal();
        CheckForLevelCompleted();
    }

    private void CheckForLevelCompleted()
    {
        for (int i = 0; i < _itemDataGoals.Length; i++)
        {
            if (_itemDataGoals[i].itemAmount > 0)
                return;
        }

        Debug.Log("Level Completed");
    }
}
