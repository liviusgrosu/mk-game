using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    void Awake()
    {
        items.Add(new Item("flat-head screwdriver", 1, "A screwdriver used to break open a lock", "X to use and X again to interact with something", 0, 1, Item.ItemType.Tool, false));
        items.Add(new Item("cross-head screwdriver", 2, "A screwdriver used to (screw/unscrew) cross-head screws", "X to use and X again to interact with something", 0, 1, Item.ItemType.Tool, false));
        items.Add(new Item("lock", 3, "A padlock used to (lock/unlock) doors", "X to use and X again to interact with something", 0, 1, Item.ItemType.Tool, true));
        items.Add(new Item("plank", 4, "A plank used for barricading", "X to use and X again to interact with something", 0, 1, Item.ItemType.Tool, true));
        items.Add(new Item("bronze key", 5, "Keys to open the guards door", "X to use and X again to interact with something", 0, 1, Item.ItemType.Tool, true));
        items.Add(new Item("batteries", 6, "Used to power up a flashlight", "X to use", 0, 1, Item.ItemType.Consumable, true));
        items.Add(new Item("knife", 7, "Used to cut rope", "X to use and X again to interact with something", 0, 1, Item.ItemType.Tool, false));
        items.Add(new Item("map", 8, "A map of the forest", "X to use and X again to interact with something", 0, 1, Item.ItemType.Quest, false));
    }
}
