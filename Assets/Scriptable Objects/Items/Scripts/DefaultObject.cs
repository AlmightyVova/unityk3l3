﻿
using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects.Items.Scripts;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Object", menuName = "Inventory System/Items/Default")]
public class DefaultObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Default;
    }
}
