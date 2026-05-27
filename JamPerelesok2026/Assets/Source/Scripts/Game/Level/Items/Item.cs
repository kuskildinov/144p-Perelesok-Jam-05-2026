using UnityEngine;

public class Item : MonoBehaviour
{

    public void Initialize()
    {

    }

    private void Start()
    {
        SetStartPosition();
    }

    #region >>> POSITION

    private void SetStartPosition()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, GlobalVars.ItemPositionZ);
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<LevelBlock>(out LevelBlock block))
        {
            block.AddItem(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<LevelBlock>(out LevelBlock block))
        {
            block.RemoveItem(this);
        }
    }
}
