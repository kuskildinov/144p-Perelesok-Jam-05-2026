using System.Collections.Generic;
using UnityEngine;

public class TipsHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> _playerTips;
    [SerializeField] private List<GameObject> _characterTips;
    [SerializeField] private PlayerRoot _playerRoot;

    public void OnPlayerModeChanged(PlayerMode mode)
    {
        if (mode == PlayerMode.Character)
        {
            TogglePlayerTips(false);
            ToggleCharacterTips(true);
        }
        else if(mode == PlayerMode.Player)
        {
            TogglePlayerTips(true);
            ToggleCharacterTips(false);
        }
    }

    private void TogglePlayerTips(bool value)
    {
        foreach (GameObject obj in _playerTips)
        {
            obj.SetActive(value);
        }
    }

    private void ToggleCharacterTips(bool value)
    {
        foreach (GameObject obj in _characterTips)
        {
            obj.SetActive(value);
        }
    }

    private void OnEnable()
    {
        _playerRoot.PlayerModeChanged += OnPlayerModeChanged;
    }

    private void OnDisable()
    {
        _playerRoot.PlayerModeChanged -= OnPlayerModeChanged;
    }
}
