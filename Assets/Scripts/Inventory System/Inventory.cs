using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    private bool leftDPadActive = false;
    private bool rightDPadActive = false;
    private bool upDPadActive = false;
    private bool downDPadActive = false;

    public int slotsX, slotsY;
    public Rect handSlotRect;
    public GUISkin skin;
    public List<Item> inventory = new List<Item>();
    public List<Item> slots = new List<Item>();
    private bool showInventory;
    private ItemDatabase database;
    private bool showToolTip;
    private string toolTip;

    private int selectedItemX = 0;
    private int selectedItemY = 0;

    private bool keyIsDown;

    private AudioSource audio;
    private bool mapAudioPlayed;

    public Texture batteryText;
    private Rect batteryRect;
    public GameObject flashLightObject;
    public Texture[] batteryIter;
    private Rect batteryLifeRect;

    public Texture mapText;
    private Rect mapRect;

    private float mapNodeX, mapNodeY;

    public Texture playerLocatorText;
    private Rect playerLocatorRect;

    public int dev;
    private float playerX, playerY;
    public GameObject locationTool;

    public float nodeRadius;
    float nodeDiameter;

    private bool draggingItem;
    private Item draggedItem;
    //Any item that the player wants to use to interact in the world
    private bool usingItem;
    private int currentIndex;
    private int prevIndex;

    private bool showMap;
    private bool _isLocked;
    private bool _drawInventory;

    GameObject hitTemp;

    public int[] startItems;

    public Texture itemButton;
    private Rect itemRect;

    public bool isLocked
    {
        get
        {
            return _isLocked;
        }
        set
        {
            _isLocked = value;
        }
    }

    // Use this for initialization
    void Start() {


        nodeDiameter = nodeRadius * 2;

        mapNodeX = mapText.width / locationTool.GetComponent<Grid>().getGridX();
        mapNodeY = mapText.height / locationTool.GetComponent<Grid>().getGridY();

        //print("mapNodeX" + mapNodeX + " mapNodeY" + mapNodeY);

        playerX = 0;
        playerY = 0;

        mapAudioPlayed = false;
        audio = gameObject.GetComponent<AudioSource>();


        setCursorLock(true);
        for (int i = 0; i < (slotsX * slotsY) + 1; i++)
        {
            slots.Add(new Item());
            inventory.Add(new Item());
        }

        //Last slot is the hand slot 
        //slots.Add(new Item());

        handSlotRect = new Rect(0, Screen.height - 128, 128, 128);
        batteryRect = new Rect(Screen.width - batteryText.width, 0, batteryText.width, batteryText.height);
        batteryLifeRect = new Rect(Screen.width - 259, 35, batteryIter[0].width * 2, batteryIter[0].height * 2);

        itemRect = new Rect(Screen.width / 2 - itemButton.width * 1.3f / 2, Screen.height / 2, itemButton.width * 1.3f, itemButton.height);



        database = GameObject.FindGameObjectWithTag("Item Database").GetComponent<ItemDatabase>();

        foreach(int item in startItems)
        {
            AddItem(item);
        }
    }
	
    void Update()
    {
        //print("lock cursor: " + Cursor.lockState);
        if (Input.GetButtonDown("Inventory"))
        {
            if (!showMap)
            {
                if (draggingItem)
                {
                    inventory[prevIndex] = draggedItem;
                    draggedItem = null;
                    draggingItem = false;
                    setCursorLock(true);
                }
                setCursorLock(!isLocked);

                showInventory = !showInventory;
                //if(!showInventory)
                //   setDev();
            }
        }

    }

    void setCursorLock(bool isLocked)
    {
        this.isLocked = isLocked;
        if(isLocked)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;

        Cursor.visible = !isLocked;

    }

    void OnGUI()
    {
        Event eve = Event.current;
        toolTip = "";
        GUI.skin = skin;

        if (showInventory)
        {
            //GUI.Box(handSlotRect, "", skin.GetStyle("Hand_slot_skin"));
                DrawInventory();
                if (showToolTip)
                {
                    GUI.Box(new Rect(selectedItemX * 128 + 64, selectedItemY * 128 + 64, 200, 200), toolTip, skin.GetStyle("Tool_tip_skin"));
                }
        }
        else
        {

            GUI.DrawTexture(batteryRect, batteryText);

            int currBattLvl = (int)Mathf.Round(flashLightObject.GetComponent<Flash_Light>().getBatteryLife() / 80.0f);
            if (currBattLvl >= 5)
                currBattLvl = 5;

            GUI.DrawTexture(batteryLifeRect, batteryIter[currBattLvl]); 
        }
        if (showMap)
        {
            //print("showing the map");
            if(!mapAudioPlayed)
            {
                audio.Play();
                mapAudioPlayed = true;
            }

            //print("showMap in OnGUI");
            playerX = mapNodeX * (locationTool.GetComponent<Grid>().getPlayerNodeX());
            playerY = mapNodeY * (locationTool.GetComponent<Grid>().getPlayerNodeY());

            //print("playerX: " + playerX + " playerY: " + playerY);

            mapRect = new Rect((Screen.width / 2) - 550, (Screen.height / 2) - 509, mapText.width, mapText.height);
            playerLocatorRect = new Rect((((Screen.width / 2) - 550) + playerX) - playerLocatorText.width/2 , ((((Screen.height / 2) + 509) - playerY - 60) ), playerLocatorText.width, playerLocatorText.height);

            GUI.DrawTexture(mapRect, mapText);
            GUI.DrawTexture(playerLocatorRect, playerLocatorText);
            //if (eve.button == 1 && eve.type == EventType.mouseUp)
            if (!Input.GetButton("Interact"))
            { 
                keyIsDown = false;
            }

            //if (eve.button == 1 && eve.type == EventType.mouseDown && keyIsDown == false)
            if (Input.GetButton("Interact") && !keyIsDown)
            {
                showInventory = true;
                showMap = false;
                keyIsDown = true;
            }
        }
        if (draggingItem)
        {
            GUI.DrawTexture(new Rect(Screen.width / 2 - 25, Screen.height / 2 + 60f, 50, 50), draggedItem.itemIcon);

            GUI.DrawTexture(itemRect, itemButton);
            //If the player tries to interact with anything with the enviroment
            //if (!showInventory && eve.button == 0 && eve.type == EventType.mouseDown)
            if (!showInventory && Input.GetButton("Interact"))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                //if (Physics.Raycast(ray, out hit, 6f))
                if(Physics.Raycast(ray, out hit, 6f, 9))
                {
                    hitTemp = hit.collider.gameObject;
                    print(hitTemp.name);
                    if (hitTemp.name == "padlock_body" && draggedItem.itemID == 1)
                    {
                        if(hitTemp.GetComponent<Key_lock>().isLocked())
                        {
                            hitTemp.GetComponent<Key_lock>().unlock();
                        }
                        putAway();
                    }
                    if(hit.collider.name == "key_lock" && draggedItem.itemID == 5)
                    {
                        hitTemp.GetComponent<Key_lock>().unlock();
                        putAway();
                        //RemoveItem(draggedItem.itemID);
                    }
                    else if(hit.collider.name == "door_latch" && draggedItem.itemID == 3)
                    {
                        //If the door is not open and doesnt have a lock
                        if (!hit.collider.transform.parent.Find("door").GetComponent<Door_V2>().openStatus() && !hit.collider.GetComponent<Door_latch_script>().hasLock())
                        {
                            hitTemp.transform.parent.Find("padlock").transform.Find("padlock_body").GetComponent<Key_lock>()._lock();
                            hitTemp.GetComponent<Door_latch_script>().assignLock();

                            //BUG vvvvvvvvv
                            //BUG NOTE:Deletes the one in the inventory but not in the one in your hand
                            RemoveItem(draggedItem.itemID);
                            //BUG ^^^^^^^^^
                        }
                    }

                    else if(hit.collider.name == "perma_key_lock" && draggedItem.itemID == 5)
                    {
                        if (!hit.collider.transform.parent.Find("door").GetComponent<Door_V2>().openStatus())
                        {
                            if(hit.collider.GetComponent<Perma_lock_script>().isLocked())
                                hit.collider.GetComponent<Perma_lock_script>().unlock();
                            else
                                hit.collider.GetComponent<Perma_lock_script>()._lock();

                            putAway();
                        }
                    }

                    else if(hit.collider.name == "non_key_door_latch" && draggedItem.itemID == 4)
                    {
                        if (!hit.collider.transform.parent.Find("door").GetComponent<Door_V2>().openStatus() && !hit.collider.GetComponent<Non_key_latch_script>().hasLock())
                        {
                            hit.collider.GetComponent<Non_key_latch_script>().assignLock();
                            hit.collider.transform.parent.Find("non_key_plank").GetComponent<Non_key_lock>()._lock();
                            RemoveItem(draggedItem.itemID);
                        }
                    }
                    else if(hit.collider.name == "Rope" && draggedItem.itemID == 7)
                    {
                        //print("salut");
                        //hit.collider.gameObject.transform.parent.Find("rope_result").gameObject.SetActive(true);
                        hit.collider.gameObject.SetActive(false);
                        hit.collider.transform.parent.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                        hit.collider.transform.parent.gameObject.GetComponent<Rigidbody>().useGravity = true;
                        putAway();

                    }
                }
            }
        }
    }

    void DrawInventory()
    {
        usingItem = false;
        Event e = Event.current;
        currentIndex = 0;

        if (Input.GetAxis("Select X") < 0.0f && !leftDPadActive && selectedItemX != 0)
        {
            selectedItemX--;
            leftDPadActive = true;
        }
        if (Input.GetAxis("Select X") > 0.0f && !rightDPadActive && selectedItemX != slotsX - 1)
        {
            selectedItemX++;
            rightDPadActive = true;
        }

        if (Input.GetAxis("Select Y") < 0.0f && !downDPadActive && selectedItemY != slotsY - 1)
        {
            selectedItemY++;
            downDPadActive = true;
        }
        if (Input.GetAxis("Select Y") > 0.0f && !upDPadActive && selectedItemY != 0)
        {
            selectedItemY--;
            upDPadActive = true;
        }

        if (Input.GetAxis("Select X") == 0.0f)
        {
            leftDPadActive = false;
            rightDPadActive = false;
        }

        if (Input.GetAxis("Select Y") == 0.0f)
        {
            upDPadActive = false;
            downDPadActive = false;
        }

        if (currentIndex < (slotsX * slotsY))
        {
            /*GUI.DrawTexture(batteryRect, batteryText);

            int currBattLvl = (int)Mathf.Round(flashLightObject.GetComponent<Flash_Light>().getBatteryLife() / 80.0f);
            if (currBattLvl >= 5)
                currBattLvl = 5;

            GUI.DrawTexture(batteryLifeRect, batteryIter[currBattLvl]);*/



            for (int y = 0; y < slotsY; y++)
            {
                for (int x = 0; x < slotsX; x++)
                {
                    Rect slotRect = new Rect(x * 128, y * 128, 128, 128);
                    if(x == selectedItemX && y == selectedItemY)
                        GUI.Box(slotRect, "", skin.GetStyle("Selected_item_skin"));
                    else
                        GUI.Box(slotRect, "", skin.GetStyle("Slot_panel_skin"));

                    slots[currentIndex] = inventory[currentIndex];


                    if (slots[currentIndex].itemName != null)
                    {
                        GUI.DrawTexture(slotRect, slots[currentIndex].itemIcon);

                        //if (slotRect.Contains(e.mousePosition))
                        if((slotRect.x / slotRect.width) == selectedItemX && (slotRect.y / slotRect.height) == selectedItemY)
                        {
                            //print("slot x: " + slotRect.x);
                            toolTip = CreateToolTip(slots[currentIndex]);
                            showToolTip = true;

                            //Theres no point of dragging items anymore
                            /*if (e.button == 0 && e.type == EventType.mouseDrag && !draggingItem)
                            {

                                draggingItem = true;
                                prevIndex = currentIndex;
                                draggedItem = slots[currentIndex];
                                inventory[currentIndex] = new Item();

                            }*/

                            /*if (e.type == EventType.mouseUp && draggingItem)
                            {
                                inventory[prevIndex] = draggedItem;
                                draggingItem = false;
                                draggedItem = null;
                            }*/

                            //if(e.button == 1 && e.type == EventType.mouseDown /*&& !draggingItem*/)
                            if (!Input.GetButton("Interact"))
                            {
                                //print("Stopped pressing up");
                                keyIsDown = false;
                            }

                            //if (eve.button == 1 && eve.type == EventType.mouseDown && keyIsDown == false)
                            if (Input.GetButton("Interact") && !keyIsDown)
                            {
                                if(slots[currentIndex].itemName == "map")
                                {
                                    keyIsDown = true;
                                    showMap = true;
                                }
                                else
                                {
                                    setCursorLock(true);
                                    usingItem = true;
                                    draggingItem = true;
                                    prevIndex = currentIndex;
                                    draggedItem = slots[currentIndex];
                                    inventory[currentIndex] = new Item();
                                }
                            }
                        }
                        //If(slots[].itemName == "some other shit")
                        //Then combine the 2 to make a new item
                    }

                    else
                    {
                        if (slotRect.Contains(e.mousePosition))
                        {
                            if (e.type == EventType.MouseUp && draggingItem)
                            {
                                //print(i);
                                inventory[currentIndex] = draggedItem;
                                draggingItem = false;
                                draggedItem = null;
                            }
                        }
                    }

                    if (toolTip == "")
                    {
                        showToolTip = false;
                    }

                    currentIndex++;
                }
            }

            //If the player tries inserting a battery into the flashlight
            //if(draggingItem && draggedItem.itemID == 6 && e.type == EventType.mouseUp && batteryRect.Contains(e.mousePosition))
            if (draggingItem && draggedItem.itemID == 6)
            {
                print("inserted");
                /*if (flashLightObject.GetComponent<Flash_Light>().getBatteryLife() == 100.0f)
                {
                    flashLightObject.GetComponent<Flash_Light>().addbatteries(20.0f);
                    print(flashLightObject.GetComponent<Flash_Light>().getBatteryLife());
                }
                else if (100.0f < flashLightObject.GetComponent<Flash_Light>().getBatteryLife() &&
                                flashLightObject.GetComponent<Flash_Light>().getBatteryLife() < 120.0f)
                {
                    float difference = 60f - flashLightObject.GetComponent<Flash_Light>().getBatteryLife();
                    flashLightObject.GetComponent<Flash_Light>().addbatteries(difference);
                    print(flashLightObject.GetComponent<Flash_Light>().getBatteryLife());
                }
                else if(flashLightObject.GetComponent<Flash_Light>().getBatteryLife() < 100.0f)
                    flashLightObject.GetComponent<Flash_Light>().addbatteries(20.0f);
                    */

                if (flashLightObject.GetComponent<Flash_Light>().getBatteryLife() < 400.0f)
                    flashLightObject.GetComponent<Flash_Light>().setBattery(400.0f);

                draggingItem = false;
                draggedItem = null;
            }

            //print("state of showMap: " + showMap);

            //if((draggingItem && (e.mousePosition.x > slotsX * 128 || e.mousePosition.y > slotsY * 128)) || showMap)
            if(showMap || usingItem)
            {
                showInventory = false;
            }
            else
            {
                showInventory = true;
            }
        }
    }

    string CreateToolTip(Item item)
    {
        toolTip = "<color=#ffffff>" + item.itemName + "</color>\n\n" + "<color=#359B00>" + item.itemDesc + "</color>\n\n" + "<color=#3552F4>" + item.itemHelp + "</color>\n";
        return toolTip;
    }

    void RemoveItem(int id)
    {
        for(int i = 0; i < inventory.Count; i++)
        {
            if(inventory[i].itemID == id)
            {
                print("removing in inventory");
                inventory[i] = new Item();
                return;
            }
        }
        print("test");
        //If you have something dragging
        if(draggedItem.itemID == id)
        {
            //If the item is being consumed
            print("itemConsume: " + draggedItem.itemConsume); 
            if(!draggedItem.itemConsume)
            {
                print("Before: inventory[" + prevIndex +"] = " + inventory[prevIndex].itemName);
                inventory[prevIndex] = draggedItem;
                print("Before: inventory[" + prevIndex + "] = " + inventory[prevIndex].itemName);
            }

            draggedItem = null;
            draggingItem = false;

            if (showInventory)
            {
                //print("showinventory = true");
                //Something
            }
            else
            {
                //print("showinventory = false");
                setCursorLock(true);
            }

        }

    }

    public void AddItem(int id)
    {
        for(int i = 0; i < inventory.Count; i++)
        {
            if(inventory[i].itemName == null)
            {
                for(int j = 0; j < database.items.Count; j++)
                {
                    if(database.items[j].itemID == id)
                    {
                        inventory[i] = database.items[j];
                    }
                }
                break;
            }
        }
    }

    public bool InventoryContains(int id)
    {
        foreach(Item item in inventory)
        {
            if (item.itemID == id)
                return true;
        }
        return false;
    }

    void putAway()
    {
        inventory[prevIndex] = draggedItem;
        draggedItem = null;
        draggingItem = false;
        setCursorLock(true);
       
        //setCursorLock(!isLocked);
        //showInventory = !showInventory;
    }

    void setDev()
    {
        float centreX, centreY;
        if(Input.GetAxis("Mouse X") < Screen.width / 2)
            centreX = -(Screen.width / 2 - Input.GetAxis("Mouse X"));
        else
            centreX = Input.GetAxis("Mouse X") - Screen.width / 2;

        if (Input.GetAxis("Mouse Y") < Screen.height / 2)
            centreY = -(Screen.height / 2 - Input.GetAxis("Mouse Y"));
        else
            centreY = Input.GetAxis("Mouse Y") - Screen.height / 2;

  

        GameObject.Find("Player").GetComponent<Player>().assignDev(centreX, centreY);
    }

    public bool inventoryOpen()
    {
        return showInventory;
    }

}
