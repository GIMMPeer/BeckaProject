using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroceryItem : MonoBehaviour
{
    public enum ItemType {Love, Sphere, Can}

    [SerializeField]
    private ItemType m_ItemType; //keeps this proctected from being set by other classes, only set in editor

    public ItemType GetItemType()
    {
        return m_ItemType;
    }
}
