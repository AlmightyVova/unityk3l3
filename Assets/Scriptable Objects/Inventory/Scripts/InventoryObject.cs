using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public ItemDatabaseObject database;
    public Inventory container;

    private void OnEnable()
    {
#if UNITY_EDITOR
        database = (ItemDatabaseObject) AssetDatabase.LoadAssetAtPath("Assets/Resources/Database.asset", typeof(ItemDatabaseObject));
#else
        database = Resources.Load<ItemDatabaseObject>("Database.asset");
#endif
    }

    public void AddItem(Item item, int amount)
    {
        for (int i = 0; i < container.items.Length; i++)
        {
            if (container.items[i].id == item.id)
            {
                container.items[i].AddAmount(amount);
                return;
            }
        }

        SetEmptySlot(item, amount);
    }

    public InventorySlot SetEmptySlot(Item item, int amount)
    {
        for (int i = 0; i < container.items.Length; i++)
        {
            if (container.items[i].id <= -1)
            {
                container.items[i].UpdateSlot(item.id, item, amount);
                return container.items[i];
            }
        }

        return null;
    }

    public void MoveItem(InventorySlot item1, InventorySlot item2)
    {
        var temp = new InventorySlot(item2.id, item2.item, item2.amount);
        item2.UpdateSlot(item1.id, item1.item, item1.amount);
        item1.UpdateSlot(temp.id, temp.item, temp.amount);
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < container.items.Length; i++)
        {
            if (container.items[i].item == item)
            {
                container.items[i].UpdateSlot(-1, null, 0);
            }
        }
    }

    [ContextMenu("Save")]
    public void Save()
    {
        /*
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();
    */
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, container);
        stream.Close();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        /*
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
    */
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory) formatter.Deserialize(stream);
            for (int i = 0; i < container.items.Length; i++)
            {
                var newSlot = newContainer.items[i];
                container.items[i].UpdateSlot(newSlot.id, newSlot.item, newSlot.amount);
            }
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        container = new Inventory();
    }
}

[Serializable]
public class Inventory
{
    public InventorySlot[] items = new InventorySlot[12];
}

[Serializable]
public class InventorySlot
{
    public int id = -1;
    public Item item;
    public int amount;

    public InventorySlot()
    {
        id = -1;
        item = null;
        amount = 0;
    }

    public InventorySlot(int id, Item item, int amount)
    {
        this.id = id;
        this.item = item;
        this.amount = amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }

    public void UpdateSlot(int id, Item item, int amount)
    {
        this.id = id;
        this.item = item;
        this.amount = amount;
    }
}