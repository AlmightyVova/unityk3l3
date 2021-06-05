using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Scriptable_Objects.Items.Scripts
{
    public enum ItemType
    {
        Default,
        Food,
        Helmet,
        Weapon,
        Shield,
        Boots,
        Chest
    }

    public abstract class ItemObject : ScriptableObject
    {
        public int id;
        public Sprite uiDisplay;
        public ItemType type;
        [TextArea(15, 20)] public string description;
    }

    [Serializable]
    public class Item
    {
        public string name;
        public int id;

        public Item(ItemObject item)
        {
            name = item.name;
            id = item.id;
        }
    }
}