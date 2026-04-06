using UnityEngine;

public class LevelController : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private ItemPlaceController _itemPlaceController;

    public ItemLevelData[] GetLevelGoals()
    {
        return _itemPlaceController.GetGoals();
    }
}
