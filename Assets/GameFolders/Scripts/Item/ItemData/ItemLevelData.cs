using UnityEngine;

[System.Serializable]
public struct ItemLevelData 
{
    public Item itemPrefab;

    [NaughtyAttributes.ValidateInput("CheckItemAmount", "Amount must be multiple of three!")]
    [NaughtyAttributes.AllowNesting]
    [Range(0,100)]
    public int itemAmount;

    private bool CheckItemAmount() => itemAmount % 3 == 0;
}
