using UnityEngine;
using System.Collections;

[System.Serializable]

public class Item
{
    public string itemName;
    public int itemID;
    public string itemDesc;
    public string itemHelp;
    public Texture2D itemIcon;
    public int itemPower;
    public int itemSpeed;
    public ItemType itemType;
    public bool itemConsume;


    public enum ItemType
    {
        Weapon, 
        Consumable,
        Quest,
        Tool
    }

    public Item(string name, int id, string desc, string help, int power, int speed, ItemType type, bool consume)
    {
        itemName = name;
        itemID = id;
        itemDesc = desc;
        itemHelp = help;
        itemIcon = Resources.Load<Texture2D>("Icons/" + name);
        itemPower = power;
        itemSpeed = speed;
        itemType = type;
        itemConsume = consume;

    }

    public Item()
    {

    }
}
