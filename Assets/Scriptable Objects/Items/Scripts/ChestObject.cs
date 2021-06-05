
using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects.Items.Scripts;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chest Object", menuName = "Inventory System/Items/Chest")]
public class ChestObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Chest;
    }
}
