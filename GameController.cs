using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public class GameController : MonoBehaviour
{
    public PlayerController Player;
    public UI_Inventory inventoryUI;

    public Vector3 lastCheckpoint;
    public int currentRoom;
    public int currentCurrency;
    public int currentEndurance;
    public int currentSaveFile;

    // NPC variables
    public bool WinecellarWardenMet;

    public static GameController Instance;

    // Inventory
    [SerializeField] public UI_Inventory uiInventory;
    public Inventory inventory;
    public string inventoryFilePath;
    private static int HashItem(Item item) => Animator.StringToHash(item.itemName);
    const char SPLIT_CHAR = '_';

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        Time.timeScale = 1;

        inventory = new Inventory();
        uiInventory = GameController.Instance.inventoryUI;
        uiInventory.SetInventory(inventory);
        inventoryFilePath = Application.persistentDataPath + "/Inventory.txt";
    }

    void Update()
    {
        if (Player == null)
            if (FindObjectOfType<PlayerController>() != null)
                Player = FindObjectOfType<PlayerController>();

        // Inventory tests
        if (Input.GetKeyDown(KeyCode.F))
        {
            inventory.FindItem(100, 1);
            Debug.Log("Item name: " + inventory.GetItemInfo(100).itemName + ". " +
                "Description: " + inventory.GetItemInfo(100).desc + ". " +
                "Grade: " + inventory.GetItemInfo(100).itemGrade + ".");
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            inventory.FindItem(101, 1);
            Debug.Log("Item name: " + inventory.GetItemInfo(101).itemName + ". " +
                "Description: " + inventory.GetItemInfo(101).desc + ". " +
                "Grade: " + inventory.GetItemInfo(101).itemGrade + ".");
        }
    }

    public void ChooseSaveFile(int savefile)
    {
        currentSaveFile = savefile;

        // Check if the save file needs to be initialized
        if (PlayerPrefs.GetInt("level" + currentSaveFile).Equals(null) || 
                PlayerPrefs.GetFloat("Xcord" + currentSaveFile) == 0 || PlayerPrefs.GetFloat("Ycord" + currentSaveFile) == 0)
        {
            // Initialize save file
            PlayerPrefs.SetInt("level" + currentSaveFile, 1);
            
            lastCheckpoint = new Vector2(18, 9);
            PlayerPrefs.SetFloat("Xcord" + currentSaveFile, lastCheckpoint.x);
            PlayerPrefs.SetFloat("Ycord" + currentSaveFile, lastCheckpoint.y);

        }
        Load(currentSaveFile);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SaveData(Vector3 cp)
    {
        currentRoom = SceneAndUIController.Instance.CurrentScene;
        // Save current level and location of the current checkpoint
        PlayerPrefs.SetInt("level" + currentSaveFile, currentRoom);
        lastCheckpoint = cp;
        PlayerPrefs.SetFloat("Xcord" + currentSaveFile, lastCheckpoint.x);
        PlayerPrefs.SetFloat("Ycord" + currentSaveFile, lastCheckpoint.y);

        // Save inventory
        using (StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/Inventory.txt"))
        {
            foreach(Item kvp in inventory.GetItemList())
            {
                Item item = kvp;
                int count = kvp.amount;
                string itemID = item.itemID.ToString();
                sw.WriteLine(itemID + SPLIT_CHAR + count);
            }
        }
        Debug.Log("Data saved");
    }


    public void Load(int currentSaveFile)
    {
        StartCoroutine(LoadData(currentSaveFile));
    }
    public IEnumerator LoadData(int gameFileToLoad)
    {
        Debug.Log("LOADING PLAYER");
        SceneAndUIController.Instance.sceneLoaded = false;

        // Load scene
        currentRoom = PlayerPrefs.GetInt("level" + currentSaveFile);
        SceneAndUIController.Instance.LoadScene(currentRoom);


        while (!SceneAndUIController.Instance.sceneLoaded)
        {
            yield return null;
        }
        if (SceneAndUIController.Instance.sceneLoaded)
        {
            // Get max health and set players hp to it
            Debug.Log("Add max hp to save/load");
            Player.PH.SetHP(Player.PH.MaxEndurance);

            // Move player to it's last checkpoint
            Player.gameObject.transform.position = new Vector2(PlayerPrefs.GetFloat("Xcord" + currentSaveFile), PlayerPrefs.GetFloat("Ycord" + currentSaveFile));

            // Get ability status'
            // 


            // Get currency
            // 

            if (!File.Exists(inventoryFilePath))
            {
                Debug.Log("File path doesn't exist. Try saving the game first");
                yield break;
            }

            string line = "";

            using (StreamReader sr = new StreamReader(inventoryFilePath))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    int id = int.Parse(line.Split(SPLIT_CHAR)[0]);
                    int count = int.Parse(line.Split(SPLIT_CHAR)[1]);

                    inventory.FindItem(id, count);
                }
            }
            Debug.Log("Data loaded");
        }
    }
}

