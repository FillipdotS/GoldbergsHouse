using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    // For other objects
    // gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

    // When the game begins, the game manager will go through and SetActive(true)
    // everything in the array. Game objects are expected to place themselves inside.
    // This is also makes it easier to prefab the GameManager and these types of game objects
    public List<GameObject> objectsToEnable;

	public bool gameBegan = false;
	
    // Used when placing back wires
    public GameObject wirePrefab;

    private static bool loadedFromAnotherScene = false;

    // The 'score' of the current level
    int collectedCollecibles = 0;

    // All controllers, TODO: Make Player class that keeps track of these
    InventoryController invController;
    Drag dragScript;
    AdjustObjects adjustScript;
    WireEditor wireEditor;

    List<InventoryController.SpawnedObject> cached_spawnedObjects;
    List<InventoryController.SpawnedWire> cached_spawnedWires;

    void Awake()
    {
        MusicController.Instance.PlayMainMusic();

        // Check the static variable and if another GameManager exists destroy self
        if (loadedFromAnotherScene)
        {
            Destroy(gameObject);
        }
        else
        {
            loadedFromAnotherScene = true; // for future game managers
            DontDestroyOnLoad(gameObject);

            if (GameObject.Find("Canvas").transform.Find("TitleScreen"))
            {
                GameObject.Find("Canvas").transform.Find("TitleScreen").gameObject.SetActive(true);
            }
        }
    }

    // Called when going into another scene, and should only be
    // called in that situation
    public void PrepareForNewLevel()
    {
        loadedFromAnotherScene = false;
        Destroy(gameObject);
    }

    void Start()
    {
        Init();
    }

    // Since the GameManager doesnt delete itself between scene reloads, we need a function to cache what
    // is needed again, since those objects will be deleted. Basically this is a Start() function more or less
    void Init()
    {
		gameBegan = false;
		
        collectedCollecibles = 0; // reset score
        invController = GameObject.FindGameObjectWithTag("InventoryController").GetComponent<InventoryController>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        dragScript = p.GetComponent<Drag>();
        adjustScript = p.GetComponent<AdjustObjects>();
        wireEditor = p.GetComponent<WireEditor>();
    }

    // Called when the build phase is over. Activates physics for objects that need it, etc.
    public void StartGame()
    {
		gameBegan = true;
		
        foreach (GameObject gameObj in objectsToEnable)
        {
            if (gameObj != null)
            {
                gameObj.SetActive(true);
            }
        }

        // Find all InteractiveObjects in the game and BeginGame on each one
        GameObject[] allInteractiveObjects = GameObject.FindGameObjectsWithTag("InteractiveObject");
        foreach (GameObject gameObj in allInteractiveObjects)
        {
            InteractiveObject interObj = gameObj.GetComponent<InteractiveObject>();
            interObj.BeginGame();
        }

        // Get the final positions of all player placed objects
        invController.GetFinalValues();
        invController.RestrictInventory(true);

        // No player actions after beginning
        dragScript.ResetDraggedObjectValues();
        dragScript.enabled = false;

        adjustScript.HideButtons();
        adjustScript.enabled = false;

        wireEditor.enabled = false;
    }

    public void AddCollectible()
    {
        if (collectedCollecibles < 3)
        {
            collectedCollecibles++;
        }
        else
        {
            Debug.LogError("3 or more collectibles have already been collected!");
        }
    }

    // Can be called by anything in the scene which completes the level
    public void LevelWin()
    {
        // Save the score if its bigger than the previous one
        int oldScore = PlayerPrefs.GetInt("Score_" + SceneManager.GetActiveScene().name);
        if (collectedCollecibles > oldScore)
        {
            PlayerPrefs.SetInt("Score_" + SceneManager.GetActiveScene().name, collectedCollecibles);

            // And also add it to the collected total
            int diff = collectedCollecibles - oldScore;

            int total = PlayerPrefs.GetInt("TotalGearsCollected");
            PlayerPrefs.SetInt("TotalGearsCollected", total + diff);
        }

        // Since the level could be restarted many times, we don't need to grab
        // the reference to the level complete panel until we actually need to show it
        GameObject levelCompletePanel;
        levelCompletePanel = GameObject.Find("Canvas").transform.Find("LevelCompletePanel").gameObject;
        levelCompletePanel.SetActive(true); // The level panel has its own methods which continue from here

        LevelComplete lvlComplete = levelCompletePanel.transform.Find("InnerPanel").GetComponent<LevelComplete>();
        lvlComplete.ShowAcquiredCollectibles(collectedCollecibles, oldScore);
    }

    // Reloads the scene and uses InventoryController's (cached as invController here) 
    // spawnedObjects to return player placed objects back to where they were
    public void BackToBuildMode()
    {
        SceneLoader.Instance.ShowLoading();

        // Cache spawnedObjects since they'll be deleted in the scene reset
        cached_spawnedObjects = invController.spawnedObjects;
        cached_spawnedWires = invController.spawnedWires;

        // Before the new scene loads, we empty this list to remove references that will be gone
        // after the scene reset. Has to happen now as some objects add themeselves to objectsToEnable
        // on their Start() method, which happens before our Init() most likely
        objectsToEnable.Clear();

        SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().buildIndex, AfterReturnToBuild);
    }

    void AfterReturnToBuild()
    {
        StartCoroutine(WaitForFramesThenExecute(1, Init, PlaceBackSpawnedObjects));
    }

    // Returns all objects placed by the player in the previous build mode
    void PlaceBackSpawnedObjects()
    {
        for (int i = 0; i < cached_spawnedObjects.Count; i++)
        {
            // Because C# is smart and doesnt let me edit spawnedObjects[i] directly
            InventoryController.SpawnedObject thisSpawnedObject;
            thisSpawnedObject = cached_spawnedObjects[i];


            GameObject newActualObject = Instantiate(thisSpawnedObject.Prefab, thisSpawnedObject.V3Position, thisSpawnedObject.Rotation) as GameObject;
			
			newActualObject.GetComponent<InteractiveObject>().draggable = true;
			
            thisSpawnedObject.ActualObject = newActualObject;

            if (newActualObject.GetComponentInChildren<ConnectionPoint>() != null)
            {
                newActualObject.GetComponentInChildren<ConnectionPoint>().connectionID = thisSpawnedObject.connectionID;
            }

            // Go through all inventory slots in the scene, find the ones with a matching id,
            // and decrease their startingAmount
            foreach (InvItem invItem in invController.itemSlots)
            {
                if (invItem.uniqueName == thisSpawnedObject.OriginInvSlot)
                {
                    invItem.startingAmount--;
                    invItem.UpdateAmountCounter();

                    // Small safety measure
                    if (invItem.startingAmount < 0)
                    {
                        Debug.LogError("PlaceBackSpawnedObjects has made an item slots amount below 0. Something went wrong.");
                    }

                    break; // stop the loop since we've found what we were looking for
                }
            }


            cached_spawnedObjects[i] = thisSpawnedObject;
        }

        ConnectionPoint[] conPoints = GameObject.FindObjectsOfType<ConnectionPoint>();

        for (int i = 0; i < cached_spawnedWires.Count; i++)
        {
            InventoryController.SpawnedWire thisSpawnedWire;
            thisSpawnedWire = cached_spawnedWires[i];

            GameObject newWire = Instantiate(wirePrefab);
            Wire wireComp = newWire.GetComponent<Wire>();

            // Finding connection one
            for (int j = 0; j < conPoints.Length; j++)
            {
                if (conPoints[j].connectionID == thisSpawnedWire.connectionOne)
                {
                    wireComp.connectionOne = conPoints[j];
                    conPoints[j].connectedWire = wireComp;
                }
            }
            // Finding connection two
            for (int j = 0; j < conPoints.Length; j++)
            {
                if (conPoints[j].connectionID == thisSpawnedWire.connectionTwo)
                {
                    wireComp.connectionTwo = conPoints[j];
                    conPoints[j].connectedWire = wireComp;
                }
            }
        }

        // Init is called just before this, so we have a new invController that need a list of spawnedObjects
        invController.spawnedObjects = cached_spawnedObjects;

        SceneLoader.Instance.HideLoading();
    }

    // Take an amount of frames to wait, then execute all given methods/functions
    // USE WITH StartCoroutine (look in BackToBuildMode as an example)
    IEnumerator WaitForFramesThenExecute(int frameCount, params Action[] methodsToCall)
    {
        while (frameCount > 0)
        {
            frameCount--;
            yield return null;
        }
        foreach (Action method in methodsToCall)
        {
            method();
        }
    }
}
