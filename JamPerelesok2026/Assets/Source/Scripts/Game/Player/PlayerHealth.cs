using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _maxHealth;

    private Player _player;
    private int _currentHealth;
    private int _damageCount;

    public int CurrentHealth => _currentHealth;   

    public void Initialize(Player player)
    {
        _player = player;
        _currentHealth = _maxHealth;
        _damageCount = 0;
    }

    public bool IsHealthFull()
    {
        if (_currentHealth == _maxHealth)
            return true;
        else
            return false;
    }

    public void OnTakeDamage()
    {
        if (!_player.IsAlive)
            return;

        _damageCount++;

        if(_damageCount >= _maxHealth)
        {
            _damageCount = _maxHealth;
            Dead();
        }

        UpdateCurrentHealth();
    }

    public void OnHealthAdded()
    {
        if (!_player.IsAlive)
            return;

        _damageCount--;

        if(_damageCount <= 0)
        {
            _damageCount = 0;
        }

        UpdateCurrentHealth();
    }

    private void UpdateCurrentHealth()
    {
        _currentHealth = _maxHealth - _damageCount;       
    }

    private void Dead()
    {
        _player.OnPlayerDead();
    }
}
