using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [Header("Player Health")]
    [SerializeField] private Transform _heartsContainer;
    [SerializeField] private GameObject _heartPrefab;

    private PlayerRoot _root;
    private List<GameObject> _currentHearts;

    public void Initialize(PlayerRoot root)
    {
        _root = root;
        _currentHearts = new List<GameObject>();

        UpdateHeartCount();
    }

    public void UpdateHeartCount()
    {
        int heartCount = _root.Player.CurrentHealth;
        UpdateHeartsCount(heartCount);
    }

    private void UpdateHeartsCount(int count)
    {
        RemoveAllHearts();

        for (int i = 0; i < count; i++)
        {
            var heart = Instantiate(_heartPrefab, _heartsContainer);
            _currentHearts.Add(heart);
        }
    }

    private void RemoveAllHearts()
    {
        for (int i = 0; i < _currentHearts.Count; i++)
        {
            Destroy(_currentHearts[i].gameObject);
        }

        _currentHearts.Clear();
    }
}
