using UnityEngine;

public class EnemysRoot : CompositeRoot
{
    private PlayerRoot _playerRoot;

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
        }
    }

    public Player TryGetPlayer()
    {
        if (_playerRoot == null)
            return null;

        return _playerRoot.Player;
    }
}
