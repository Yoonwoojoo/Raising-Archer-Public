using UnityEngine;

public class PlayerManager : GenericSingleton<PlayerManager>
{
    private Player _player;
    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }

    public Transform PlayerTransform
    {
        get { return _player != null ? _player.transform : null; }
    }

    public GameObject _playerPrefab;
}
