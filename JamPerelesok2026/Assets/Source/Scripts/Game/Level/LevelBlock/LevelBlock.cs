using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelBlock : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Transform _itemsContainer;
    [SerializeField] private GameObject _outline;
    [Header("Animation Settings")]
    [SerializeField] private float _rotateDuration = 0.5f;
    [SerializeField] private float _scaleMultiplier = -1.2f;
    [SerializeField] private float _scaleDuration = 0.15f;

    [SerializeField] private List<Item> _items = new List<Item>();
       
    private bool _canMove = true;
    private bool _isMoved = false;
    private Vector3 _originalScale;

    public bool CanMove { get => _canMove; set => _canMove = value; }

    private void Start()
    {      
        _originalScale = transform.localScale;        
    }

    public void Initialize()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_canMove || _isMoved)
            return;
               
        RotateObject(gameObject,true,
            () =>
            {
                _isMoved = false;
            });
        RotateAllItems();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_canMove || _isMoved)
            return;

        _outline.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_canMove || _isMoved)
            return;

        _outline.gameObject.SetActive(false);
    }

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
        _isMoved = true;
        _outline.gameObject.SetActive(false);

        float angle = -90;
        if (dir)
            angle = -90;
        else
            angle = 0;

        Vector3 targetScale = _originalScale * _scaleMultiplier;
        Vector3 targetRotation = obj.transform.eulerAngles + new Vector3(0f, angle, 0f);

        Sequence sequence = DOTween.Sequence();
       
        sequence.Append(obj.transform.DORotate(targetRotation, _rotateDuration, RotateMode.Fast)
            .SetEase(Ease.InOutQuad));
      
        sequence.Play();

        sequence.OnComplete(() =>
        {
            OnComplete?.Invoke();
        }
        );
    }

   

    #endregion
}
