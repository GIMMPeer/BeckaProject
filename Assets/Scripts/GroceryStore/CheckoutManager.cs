using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckoutManager : MonoBehaviour {

    //Sorry, GG

    public Basket m_Basket;
    [Space]
    public int m_IceCreamAmount;
    public int m_MilkAmount;
    public int m_EggsAmount;
    public int m_PastaAmount;
    public int m_SauceAmount;
    public int m_FruitAmount;
    public int m_LipGlossAmount;
    public int m_MascaraAmount;

    

    private int itemsFulfilled;



    void Start () {

        itemsFulfilled = 0;

        //testing, meant to be called when basket touches scannerBox.
        CheckBasket();
    }


    public void CheckBasket()
    {
        int m_IceCreamAmountCurrent = 0;
        int m_MilkAmountCurrent = 0;
        int m_EggsAmountCurrent = 0;
        int m_PastaAmountCurrent = 0;
        int m_SauceAmountCurrent = 0;
        int m_FruitAmountCurrent = 0;
        int m_LipGlossAmountCurrent = 0;
        int m_MascaraAmountCurrent = 0;
    
        foreach (GroceryItem i in m_Basket.GetItemsFromBasket())
        {
            switch (i.GetItemType())
            {
                case GroceryItem.ItemType.IceCream:
                    m_IceCreamAmountCurrent++;
                    if (m_IceCreamAmountCurrent >= m_IceCreamAmount)
                    {
                        itemsFulfilled++;
                    }
                    break;
                case GroceryItem.ItemType.Milk:
                    m_MilkAmountCurrent++;
                    if (m_MilkAmountCurrent >= m_MilkAmount)
                    {
                        itemsFulfilled++;
                    }
                    break;
                case GroceryItem.ItemType.Eggs:
                    m_EggsAmountCurrent++;
                    if (m_EggsAmountCurrent >= m_EggsAmount)
                    {
                        itemsFulfilled++;
                    }
                    break;
                case GroceryItem.ItemType.Fruit:
                    m_FruitAmountCurrent++;
                    if (m_FruitAmountCurrent >= m_FruitAmount)
                    {
                        itemsFulfilled++;
                    }
                    break;
                case GroceryItem.ItemType.LipGloss:
                    m_LipGlossAmountCurrent++;
                    if (m_LipGlossAmountCurrent >= m_LipGlossAmount)
                    {
                        itemsFulfilled++;
                    }
                    break;
                case GroceryItem.ItemType.Mascara:
                    m_MascaraAmountCurrent++;
                    if (m_MascaraAmountCurrent >= m_MascaraAmount)
                    {
                        itemsFulfilled++;
                    }
                    break;
                case GroceryItem.ItemType.Pasta:
                    m_PastaAmountCurrent++;
                    if (m_PastaAmountCurrent >= m_PastaAmount)
                    {
                        itemsFulfilled++;
                    }
                    break;
                case GroceryItem.ItemType.Sauce:
                    m_SauceAmountCurrent++;
                    if (m_SauceAmountCurrent >= m_SauceAmount)
                    {
                        itemsFulfilled++;
                    }
                    break;
            }
        }

        int totalNeeded = System.Enum.GetNames(typeof(GroceryItem.ItemType)).Length;

        Debug.Log("Needed Items fulfilled: " + itemsFulfilled + "/" + totalNeeded);


        if(itemsFulfilled >= totalNeeded)
        {
            Debug.Log("You win!");
            //User has put all required items into basket

            GetComponent<RoomTask>().FinishTask();
        }
    }
}
