using System;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    [SerializeField]
    private PickableType _pickableType;

    public Action<PickableType, Pickable> OnPicked;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPicked?.Invoke(_pickableType, this);
        }
    }

    public PickableType GetPickableType()
    {
        return _pickableType;
    }

}
