using UnityEngine;
using System.Collections;

// Allows us to take JSON data and turn it into a C# object!
using LitJson;

// We need access to the file system
using System.IO;

// We need a generic list of items
using System.Collections.Generic;

// Class to define our Stats
[System.Serializable]
public class Stat
{
    public int Power { get; set; }
    public int Defence { get; set; }
    public int Vitality { get; set; }
}

// Class to define our Item
[System.Serializable]
public class ItemData
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Value { get; set; }
    public Stat Stats { get; set; }
    public bool Stackable { get; set; }
    public string Slug { get; set; }
    public Sprite sprite { get; set; }
    public GameObject gameObject { get; set; }

    // Custom constructor for Item that takes in JsonData
    public ItemData(JsonData data)
    {
        Id = (int)data["id"];
        Title = data["title"].ToString();
        Description = data["description"].ToString();
        Value = (int)data["value"];
        Stats = new Stat(); Stats.Power = (int)data["stats"]["power"];
        Stats.Defence = (int)data["stats"]["defence"];
        Stats.Vitality = (int)data["stats"]["vitality"];
        Stackable = (bool)data["stackable"];
        Slug = data["slug"].ToString();
        sprite = Resources.Load<Sprite>("Sprites/Items/" + Slug);
    }
}

public class ItemDatabase : MonoBehaviour
{

    // Our list of database 
    public List<ItemData> database = new List<ItemData>();

    // Holds the JSON data we pull in from the scene
    private JsonData itemData;

    // Use this for initialization
    void Start()
    {
        // Read in from JSON file
        string jsonFilePath = Application.dataPath + "/StreamingAssets/items.json";

        // Load JSON from file
        string jsonData = File.ReadAllText(jsonFilePath);

        // Convert JSON data to object
        itemData = JsonMapper.ToObject(jsonData);

        // Construct item database
        ConstructItemDatabase();
    }

    void ConstructItemDatabase()
    {
        // Loop through all items from Json data
        for (int i = 0; i < itemData.Count; i++)
        {
            // Add each to database list
            database.Add(new ItemData(itemData[i]));
        }
    }

    public ItemData GetItemById(int id)
    {
        // Loop through database
        for (int i = 0; i < database.Count; i++)
        {
            // Check if current item has the same ID
            if (database[i].Id == id)
            {
                return database[i];
            }
        }     // Return null otherwise
        return null;
    }
}
