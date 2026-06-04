using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelBlock : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Transform _itemsContainer;
    [SerializeField] private Transform _enemysContainer;
    [SerializeField] private List<LevelBlockCell> _cells;
    [Header("Animation Settings")]
    [SerializeField] private float _rotateDuration = 0.5f;
    [SerializeField] private float _raiseHeight = 1f;
    [SerializeField] private float _raiseDuration = 0.2f;
    [SerializeField] private float _lowerDuration = 0.2f;
    [Header("Sounds")]
    [SerializeField] private AudioSource _mainSource;
    [SerializeField] private AudioClip _turnSound;

    [SerializeField] private List<Item> _items = new List<Item>();
    [SerializeField] private List<Enemy> _enemys = new List<Enemy>();

    private LevelBlocksHandler _blocksHandler;
    private bool _canMove = true;
    private bool _isMoved = false;
    private Vector3 _originalScale;

    public bool CanMove { get => _canMove; set => _canMove = value; }
   
    public void Initialize(LevelBlocksHandler blocksHandler)
    {
        _blocksHandler = blocksHandler;
        _originalScale = transform.localScale;
    }
  
    public void OnPointerClick(PointerEventData eventData)
    {      
        if (!_canMove || _isMoved)
            return;

        _blocksHandler.SetCurrentBlock(this);      
    }

    public void OnPointerEnter(PointerEventData eventData)
    {       
        if (!_canMove || _isMoved)
            return;

        ToggleOutline(true,true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_canMove)
            return;

        ToggleOutline(true,false);
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

    #endregion
    #region >>> ENEMYS
    public void AddEnemy(Enemy enemy)
    {
        if (_enemys == null || _enemys.Contains(enemy))
            return;

        _enemys.Add(enemy);
        enemy.transform.SetParent(_enemysContainer);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        if (_enemys == null || !_enemys.Contains(enemy))
            return;

        _enemys.Remove(enemy);
    }

    private void ActivateAllEnemys()
    {
        foreach (Enemy enemy in _enemys)
        {
            if (enemy != null || enemy.IsAlive)
            {
                enemy.Activate(null);
                enemy.Agent.enabled = true;
            }                
        }
    }

    private void DeactivateAllEnemys()
    {
        foreach (Enemy enemy in _enemys)
        {
            if(enemy != null || enemy.IsAlive)
            {
                enemy.Deactivate();
                enemy.Agent.enabled = false;
            }           
        }
    }

    #endregion
    #region >>> ROTATION

    public void RotateBlock(bool isLeft, Action onCompete)
    {
        DeactivateAllEnemys();
        PlayTurnSound();
        RotateObject(gameObject, isLeft,
           () =>
           {
               _isMoved = false;
               ActivateAllEnemys();

               onCompete?.Invoke();
           });
    }

    private void RotateObject(GameObject obj, bool dir, Action OnComplete)
    {
        _isMoved = true;
        float angle = -90;
        if (dir)
            angle = -90;       
        else
            angle = 90;
              
        Vector3 targetRotation = obj.transform.eulerAngles + new Vector3(0f, angle, 0f);
               
        float originalY = obj.transform.position.y;
        float raisedY = originalY + _raiseHeight;

        Sequence sequence = DOTween.Sequence();
               
        sequence.Append(obj.transform.DOMoveY(raisedY, _raiseDuration)
            .SetEase(Ease.OutQuad));
                
        sequence.Append(obj.transform.DORotate(targetRotation, _rotateDuration, RotateMode.Fast)
            .SetEase(Ease.InOutQuad));
               
        sequence.Append(obj.transform.DOMoveY(originalY, _lowerDuration)
            .SetEase(Ease.InQuad));

        sequence.Play();

        sequence.OnComplete(() =>
        {
            OnComplete?.Invoke();
        });
    }

    #endregion
    #region >>> OUTLINE

    public void ToggleOutline(bool isPointed, bool value)
    {      
        foreach (LevelBlockCell cell in _cells)
        {
            cell.ToggleOutline(isPointed, value);
        }
    }

    #endregion
    #region >>> SOUNDS

    public void PlayTurnSound()
    {
        _mainSource.PlayOneShot(_turnSound);
    }

    #endregion
}
