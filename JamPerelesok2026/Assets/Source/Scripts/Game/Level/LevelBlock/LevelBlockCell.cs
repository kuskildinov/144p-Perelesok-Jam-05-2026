using UnityEngine;

public class LevelBlockCell : MonoBehaviour
{
    [SerializeField] private GameObject _pointedOutline;
    [SerializeField] private GameObject _selectedOutline;
    [SerializeField] protected MeshRenderer _meshRenderer;
    [SerializeField] private Material[] _materials;

    public virtual void Start()
    {
        RandomizeMaterial();
    }

    public void ToggleOutline(bool isPointed,bool value)
    {
        if(isPointed)
        {
            _pointedOutline.gameObject.SetActive(value);
        }
        else
        {
            _selectedOutline.gameObject.SetActive(value);
        }
       
    }

    private void RandomizeMaterial()
    {
        int count = _materials.Length;
        int random = Random.Range(0, count);
        _meshRenderer.material = _materials[random];
    }
}
