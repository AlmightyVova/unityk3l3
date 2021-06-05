
using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects.Items.Scripts;
using UnityEngine;

[CreateAssetMenu(fileName = "New Helmet Object", menuName = "Inventory System/Items/Helmet")]
public class HelmetObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Helmet;
    }
}
