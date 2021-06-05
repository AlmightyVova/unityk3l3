
using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects.Items.Scripts;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shield Object", menuName = "Inventory System/Items/Shield")]
public class ShieldObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Shield;
    }
}
