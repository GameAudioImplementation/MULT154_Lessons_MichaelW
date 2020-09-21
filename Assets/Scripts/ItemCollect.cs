using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//item.typeOfVeggie item is called like a method? how connected to typeOfVeggie
//delegate
//foreach "typeof()"
//why don't we declare CollectItem variable/reference
//where is ItemInventory coming from
//static cannot be used in other classes but is persistent?
//what is event?
public class ItemCollect : NetworkBehaviour
{
    private Dictionary<Item.VegetableType, int> ItemInventory = new Dictionary<Item.VegetableType, int>();

    //delegate allows function to be used like variable
    public delegate void CollectItem(Item.VegetableType item); //Delares delegate type and parameters
    public static event CollectItem ItemCollected;

    Collider itemCollider = null;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Item.VegetableType item in System.Enum.GetValues(typeof(Item.VegetableType)))
        {
            ItemInventory.Add(item, 0);
        }
    }
    
    private void AddToInventory(Item apple)
    {
        ItemInventory[apple.typeOfVeggie]++;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (itemCollider && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space bar and item collected");
            Item item = itemCollider.gameObject.GetComponent<Item>();
            AddToInventory(item);
            PrintInventory();

            CmdItemCollected(item.typeOfVeggie);
          
        }
    }

    [Command]

    void CmdItemCollected(Item.VegetableType itemType)
    {
        Debug.Log("CommandItemCollect:" + itemType);
        RpcItemCollected(itemType);
    }

    [ClientRpc]

    void RpcItemCollected(Item.VegetableType itemType)
    {
        ItemCollected?.Invoke(itemType);
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (other.CompareTag("Item"))
        {
            itemCollider = other;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (other.CompareTag("Item"))
        {
            itemCollider = null;
        }
    }

   

    private void PrintInventory()
    {
        string output = "";

        foreach (KeyValuePair<Item.VegetableType, int> kvp in ItemInventory)
        {
            output += string.Format("{0}: {1} ", kvp.Key, kvp.Value);
        }
        Debug.Log(output);
    }

}
