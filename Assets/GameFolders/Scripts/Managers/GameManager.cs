using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    private EGameState _gameState;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }
}
