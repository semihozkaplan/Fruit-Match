using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using System;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class ItemPlaceController : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private List<ItemLevelData> _itemData;

    [Header("Settings")]
    [SerializeField] BoxCollider _spawnZone;

    [SerializeField] private int _seed;

    public ItemLevelData[] GetGoals()
    {
        List<ItemLevelData> goals = new List<ItemLevelData>();
        foreach (ItemLevelData data in _itemData)
        {
            if (data.isGoal)
                goals.Add(data);
        }
        return goals.ToArray();
    }

#if UNITY_EDITOR

    [Button]
    private void GenerateItems()
    {
        while (transform.childCount > 0)
        {
            Transform t = transform.GetChild(0);
            t.SetParent(null);
            DestroyImmediate(t.gameObject);
        }

        UnityEngine.Random.InitState(_seed);

        for (int i = 0; i < _itemData.Count; i++)
        {
            ItemLevelData data = _itemData[i];
            for (int j = 0; j < data.itemAmount; j++)
            {
                Vector3 spawnPos = GetSpawnPosition();

                Item item = PrefabUtility.InstantiatePrefab(data.itemPrefab, transform) as Item;
                item.transform.position = spawnPos;
                item.transform.rotation = Quaternion.Euler(UnityEngine.Random.onUnitSphere * 360);
            }
        }
    }

    private Vector3 GetSpawnPosition()
    {
        float xPos = UnityEngine.Random.Range(-_spawnZone.size.x / 2, _spawnZone.size.x / 2);
        float yPos = UnityEngine.Random.Range(-_spawnZone.size.y / 2, _spawnZone.size.y / 2);
        float zPos = UnityEngine.Random.Range(-_spawnZone.size.z / 2, _spawnZone.size.z / 2);

        Vector3 localPos = _spawnZone.center + new Vector3(xPos, yPos, zPos);
        Vector3 spawnPos = transform.TransformPoint(localPos);

        return spawnPos;
    }

#endif

}
