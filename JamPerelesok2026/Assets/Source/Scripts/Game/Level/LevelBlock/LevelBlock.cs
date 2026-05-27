using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelBlock : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private bool _canMove = true;
    [SerializeField] private Transform _itemsContainer;
    [Header("Animation Settings")]
    [SerializeField] private float _rotateDuration = 0.5f;
    [SerializeField] private float _scaleMultiplier = 1.2f;
    [SerializeField] private float _scaleDuration = 0.15f;

    [SerializeField] private List<Item> _items = new List<Item>();
       
    private Vector3 _originalScale;

    private void Start()
    {      
        _originalScale = transform.localScale;        
    }

    public void Initialize()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_canMove)
            return;

        SetActivePosition();
        RotateObject(gameObject,true,
            () =>
            {
                ResetPosition();
            });
        RotateAllItems();
    }
    #region >>> POSITION

    private void SetActivePosition()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, GlobalVars.MovableBlockActivePositionZ);
    }

    private void ResetPosition()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, GlobalVars.MovableBlockPositionZ);
    }

    #endregion
    #region >>> ITEMS
    public void AddItem(Item item)
    {
        if (_items == null)
            return;

        _items.Add(item);
        item.transform.SetParent(_itemsContainer);
    }

    public void RemoveItem(Item item)
    {
        if (_items == null || !_items.Contains(item))
            return;

        _items.Remove(item);
    }

    private void RotateAllItems()
    {
        foreach (Item item in _items)
        {
            RotateObject(item.gameObject, false, null);
        }
    }

    #endregion
    #region >>> ROTATION

    private void RotateObject(GameObject obj, bool dir, Action OnComplete)
    {
        float angle = -90;
        if (dir)
            angle = -90;
        else
            angle = 0;

        Vector3 targetScale = _originalScale * _scaleMultiplier;
        Vector3 targetRotation = obj.transform.eulerAngles + new Vector3(0, 0, angle);

        Sequence sequence = DOTween.Sequence();

        // 1. Увеличение (подъём)
        sequence.Append(obj.transform.DOScale(targetScale, _scaleDuration).SetEase(Ease.OutQuad));

        // 2. Поворот
        sequence.Append(obj.transform.DORotate(targetRotation, _rotateDuration, RotateMode.Fast)
            .SetEase(Ease.InOutQuad));

        // 3. Возврат размера
        sequence.Append(obj.transform.DOScale(_originalScale, _scaleDuration).SetEase(Ease.InQuad));

        sequence.Play();

        sequence.OnComplete(() =>
        {
            OnComplete?.Invoke();
        }
        );
    }

    #endregion
}
