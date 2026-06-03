using System.Collections.Generic;
using UnityEngine;

public class EnemysRoot : CompositeRoot
{
    private PlayerRoot _playerRoot;
    private List<Enemy> _currentEnemys = new List<Enemy>();

    public override void Compose()
    {
        _playerRoot = FindAnyObjectByType<PlayerRoot>();

        InitializeLevelEnemys();
    }

    private void InitializeLevelEnemys()
    {
        Enemy[] enemys = FindObjectsByType<Enemy>();

        foreach (Enemy enemy in enemys)
        {
            enemy.Initialzie(this);
            _currentEnemys.Add(enemy);
        }
    }

    public Player TryGetPlayer()
    {
        if (_playerRoot == null)
            return null;

        return _playerRoot.Player;
    }

}
