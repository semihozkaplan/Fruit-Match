using UnityEngine;

public class GoalManager : MonoBehaviour
{
    private ItemLevelData[] _itemDataGoals;

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

            break;
        }
    }

    private void CompleteGoal(int goalIndex)
    {
        Debug.Log($"Goal Completed: {_itemDataGoals[goalIndex].itemPrefab.ItemType}");
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
