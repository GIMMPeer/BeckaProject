using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroceryItem : MonoBehaviour
{
    public enum ItemType
    { IceCream, Milk, Eggs, Pasta, Sauce, Fruit, LipGloss, Mascara }

    [SerializeField]
    private ItemType m_ItemType; //keeps this proctected from being set by other classes, only set in editor
    [SerializeField]
    private bool m_IsPaintedOver = true;

    public ItemType GetItemType()
    {
        return m_ItemType;
    }

    public bool IsItemPainted()
    {
        return m_IsPaintedOver;
    }
}
