using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckoutManager : MonoBehaviour {


    public Basket m_Basket;

    public int m_NeededLoveAmount;
    public int m_NeededCanAmount;
    public int m_NeededSphereAmount;



    private List<string> m_AllItemTypes;
    private List<string> m_NeededItemTypes;

    private int itemsRemoved = 0;

    //Need to have functionality to use multiple items. Not just one of each

    void Start () {

        m_AllItemTypes = new List<string>();
        m_NeededItemTypes = new List<string>();

        string[] list = System.Enum.GetNames(typeof(GroceryItem.ItemType));
        for(var i = 0; i < list.Length; i++)
        {
            m_AllItemTypes.Add(list[i]);
        }
        m_NeededItemTypes = m_AllItemTypes;

        //testing
        CheckBasket();
    }

    public void CheckBasket()
    {
        int amtOfLove = 0;
        int amtOfCan = 0;
        int amtOfSphere = 0;

        foreach (GroceryItem item in m_Basket.GetItemsFromBasket())
        {
            GroceryItem.ItemType curItemType = item.GetItemType();

            Debug.Log("Current Item Type: " + curItemType);

            switch (curItemType)
            {
                case GroceryItem.ItemType.Can:
                    if(amtOfCan < m_NeededCanAmount)
                    {
                        Debug.Log("Adding Can");
                        amtOfCan++;
                    }
                    break;
                case GroceryItem.ItemType.Love:
                    if(amtOfLove < m_NeededLoveAmount)
                    {
                        Debug.Log("Adding Love");
                        amtOfLove++;
                    }
                    break;
                case GroceryItem.ItemType.Sphere:
                    if(amtOfSphere < m_NeededSphereAmount)
                    {
                        Debug.Log("Adding Spheres");
                        amtOfSphere++;
                    }
                    break;
            }         
        }

        int totalAmt = amtOfCan + amtOfLove + amtOfSphere;
        int totalNeeded = m_NeededCanAmount + m_NeededLoveAmount + m_NeededSphereAmount;
        Debug.Log("Needed Items fulfilled: " + totalAmt + "/" + totalNeeded);

        if (amtOfCan == m_NeededCanAmount 
            && amtOfLove == m_NeededLoveAmount 
            && amtOfSphere == m_NeededSphereAmount)
        {
            Debug.Log("Nice, you win");
        }
        






        //RIP below 


        /*
         * 
        foreach (GroceryItem item in m_Basket.GetItemsFromBasket())
        {
            GroceryItem.ItemType curItemType = item.GetItemType();
            
            Debug.Log("Current Item Type: " + curItemType);

            for (int i = 0; i < m_NeededItemTypes.Count; i++)
            {
                Debug.Log("Itterating through Needed Items: " + m_NeededItemTypes[i]);
                if (curItemType.ToString() == m_NeededItemTypes[i])
                {
                    Debug.Log("Current Item = Current Needed Item");
                    m_NeededItemTypes.Remove(curItemType.ToString());
                    itemsRemoved++;
                }
            }
        }

        Debug.Log("Needed Items fulfilled: " + itemsRemoved + "/" + m_AllItemTypes.Count);

        if(itemsRemoved == m_AllItemTypes.Count)
        {
            //Basket is on checkout counter, basket is full of necessary items, ready to progress
            Debug.Log("Nice Job! Finished!");
        }
        
         */
    }
}
