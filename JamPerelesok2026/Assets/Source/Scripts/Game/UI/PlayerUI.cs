using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Player Health")]
    [SerializeField] private Transform _heartsContainer;
    [SerializeField] private GameObject _heartPrefab;
    [SerializeField] private Sprite _activeHeart;
    [SerializeField] private Sprite _notActiveHeart;
    [SerializeField] private Image _heart_1_image;
    [SerializeField] private Image _heart_2_image;
    [SerializeField] private Image _heart_3_image;
    [Header("Key")]
    [SerializeField] private Image _keyImage;

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
        if (count == 3)
        {
            _heart_1_image.sprite = _activeHeart;
            _heart_2_image.sprite = _activeHeart;
            _heart_3_image.sprite = _activeHeart;
        }
        else if (count == 2)
        {
            _heart_1_image.sprite = _activeHeart;
            _heart_2_image.sprite = _activeHeart;
            _heart_3_image.sprite = _notActiveHeart;
        }
        else if(count == 1)
        {
            _heart_1_image.sprite = _activeHeart;
            _heart_2_image.sprite = _notActiveHeart;
            _heart_3_image.sprite = _notActiveHeart;
        }
        else if(count == 0)
        {
            _heart_1_image.sprite = _notActiveHeart;
            _heart_2_image.sprite = _notActiveHeart;
            _heart_3_image.sprite = _notActiveHeart;
        }
    }

    public void ShowKeyUi()
    {
        _keyImage.gameObject.SetActive(true);
    }

    public void HideKeyUi()
    {
        _keyImage.gameObject.SetActive(false);
    }
}
