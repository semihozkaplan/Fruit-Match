using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private const string LevelKey = "Level";

    [Header("Data")]
    [SerializeField] private LevelController[] _levels;
    private int _levelIndex;

    private LevelController _currentLevel;

    public static Action<LevelController> OnLevelSpawned;

    private void Awake()
    {
        LoadData();
    }

    private void Start()
    {
        SpawnLevel();
    }

    private void SpawnLevel()
    {
        transform.Clear();

        int validateLevel = _levelIndex % _levels.Length;
        _currentLevel = Instantiate(_levels[validateLevel], transform);

        OnLevelSpawned?.Invoke(_currentLevel);
    }

    private void LoadData()
    {
        _levelIndex = PlayerPrefs.GetInt(LevelKey);
    }

    private void SavaData()
    {
        PlayerPrefs.SetInt(LevelKey, _levelIndex);
    }
}
