using UnityEngine;

public class Placeable : MonoBehaviour
{
    [Header("Save Data")]
    [SerializeField] private int ID;

    [Header("Has Collider")]
    public bool HasCollider = true;

    public int GetID()
    {
        return ID;
    }

    public void SetID(int id)
    {
        ID = id;
    }

}