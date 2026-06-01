using UnityEngine;

public class SpriteDepthSorter : MonoBehaviour
{
    [SerializeField] private int _sortingOffset;
    [SerializeField] private SpriteRenderer _renderer;
   
    private void LateUpdate()
    {
        _renderer.sortingOrder =
            -(int)(transform.position.z * 100)
            + _sortingOffset;
    }
}
