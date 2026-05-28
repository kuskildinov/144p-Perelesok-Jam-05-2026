using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public event Action<Vector2> MoveInput;
    public event Action AttackInput;
    public event Action InteractInput;

   public void OnMove(InputValue value)
    {       
        MoveInput?.Invoke(value.Get<Vector2>());      
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
        { 
            AttackInput?.Invoke();
        }
    }

    public void OnInteract(InputValue value)
    {
        if (value.isPressed)
        {
            InteractInput?.Invoke();
        }
    }
}
