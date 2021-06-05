using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects.Items.Scripts;
using UnityEngine;

public class PlayerInventoryScript : MonoBehaviour
{
    public MouseItem MouseItem = new MouseItem();
    public InventoryObject inventory;

    public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<GroundItem>();

        if (item)
        {
            inventory.AddItem(new Item(item.item), 1);
            Destroy(other.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventory.Save();
        }
        else if(Input.GetKeyDown(KeyCode.Return))
        {
            inventory.Load();
        }
    }

    private void OnApplicationQuit()
    {
        inventory.container.items = new InventorySlot[12];
    }
}
