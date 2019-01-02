using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Hold/carry physics objects
 * Keep track of number of objects inside
*/

public class Basket : MonoBehaviour {

    private List<GroceryItem> m_BasketItems;
	// Use this for initialization

    private void OnTriggerEnter(Collider other)
    {
        GroceryItem item = other.gameObject.GetComponent<GroceryItem>();

        //if triggered object is a grocery item and is already not in basket list
        if (item && !m_BasketItems.Contains(item))
        {
            //add item to basket list
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
            m_BasketItems.Remove(item);
        }
    }
}
