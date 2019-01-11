using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Hold/carry physics objects
 * Keep track of number of objects inside
 * Specify how many of each item is required
 * Slows player down if objects are not painted over
*/

public class Basket : MonoBehaviour
{
    //public UnityStandardAssets.Characters.FirstPerson.FirstPersonController m_PlayerController;

    public int m_NumItem1;
    public int m_NumItem2;
    public int m_NumItem3;
    
    private List<GroceryItem> m_BasketItems;

    public List<GroceryItem> testingItems;

    //Used as Awake and not Start ONLY AS TESTING -GG
    private void Awake()
    {
        m_BasketItems = new List<GroceryItem>();
    }

    private void Update()
    {
        UpdatePlayerSpeed(); //TODO move this to an event system so it doesn't have to check every frame
    }

    private void OnTriggerEnter(Collider other)
    {
        GroceryItem item = other.gameObject.GetComponent<GroceryItem>();

        //if triggered object is a grocery item and is already not in basket list
        if (item && !m_BasketItems.Contains(item))
        {
            //add item to basket list
            item.gameObject.transform.parent = transform;
            item.gameObject.layer = gameObject.layer;
            m_BasketItems.Add(item);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GroceryItem item = other.gameObject.GetComponent<GroceryItem>();

        //if triggered object is a grocery item and is in basket list
        if (item && m_BasketItems.Contains(item))
        {
            //remove item from basket list
            item.gameObject.transform.parent = null;
            item.gameObject.layer = 0;
            m_BasketItems.Remove(item);
        }
    }

    private void UpdatePlayerSpeed()
    {
        int maxItems = m_BasketItems.Count;
        int numNonPaintedItems = GetNumItemsNotPainted();

        float walkScalar = (float)numNonPaintedItems / (float)maxItems; //get percentage of items non painted
        walkScalar = 1.0f - walkScalar; //inverse percentage to make more non painted items cause a slower walk speed

        //m_PlayerController.SetWalkScalar(walkScalar);
    }

    //goes through current basket items and gets number of items not painted over
    private int GetNumItemsNotPainted()
    {
        int count = 0;
        foreach (GroceryItem item in m_BasketItems)
        {
            if (!item.IsItemPainted())
            {
                count++;
            }
        }
        return count;
    }

    public List<GroceryItem> GetItemsFromBasket()
    {
        //testing --> Adding Items into basket manually, no vr for testing
        for(var i = 0; i < testingItems.Count; i++)
        {
            m_BasketItems.Add(testingItems[i]);
        }//<--

        return m_BasketItems;
    }

}
