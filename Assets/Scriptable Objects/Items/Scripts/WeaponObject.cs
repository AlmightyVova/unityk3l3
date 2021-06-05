
using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects.Items.Scripts;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Object", menuName = "Inventory System/Items/Weapon")]
public class WeaponObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Weapon;
    }
}
