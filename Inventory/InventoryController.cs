using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    // Used for returning all player placed objects back into their original place
    public struct SpawnedObject
    {
        private GameObject m_actualObject;
        private string m_originInvSlot;
        private Vector3 m_position;
        private GameObject m_prefab;
        private Quaternion m_rotation;
        private int m_connectionID;

        public SpawnedObject(GameObject newObject, string newOriginSlot, Vector3 newPosition, GameObject newPrefab, Quaternion newRotation, int newConnectionID)
        {
            m_actualObject = newObject;
            m_originInvSlot = newOriginSlot;
            m_position = newPosition;
            m_prefab = newPrefab;
            m_rotation = newRotation;
            m_connectionID = newConnectionID;
        }

        public GameObject ActualObject
        {
            get
            {
                return m_actualObject;
            }
            set
            {
                m_actualObject = value;
            }
        }
        public string OriginInvSlot
        {
            get
            {
                return m_originInvSlot;
            }
            set
            {
                m_originInvSlot = value;
            }
        }
        public Vector3 V3Position
        {
            get
            {
                return m_position;
            }
            set
            {
                m_position = value;
            }
        }
        public GameObject Prefab
        {
            get
            {
                return m_prefab;
            }
            set
            {
                m_prefab = value;
            }
        }
        public Quaternion Rotation
        {
            get
            {
                return m_rotation;
            }
            set
            {
                m_rotation = value;
            }
        }
        public int connectionID
        {
            get
            {
                return m_connectionID;
            }
            set
            {
                m_connectionID = value;
            }
        }
    }

    // Used for returning wires back to their build mode state
    public struct SpawnedWire
    {
        // These refer to the connectionID of the ConnectionPoint they are attached to
        private int m_con1;
        private int m_con2;

        public SpawnedWire(int newCon1, int newCon2)
        {
            m_con1 = newCon1;
            m_con2 = newCon2;
        }

        public int connectionOne
        {
            get
            {
                return m_con1;
            }
            set
            {
                m_con1 = value;
            }
        }
        public int connectionTwo
        {
            get
            {
                return m_con2;
            }
            set
            {
                m_con2 = value;
            }
        }
    }

    public List<SpawnedObject> spawnedObjects = new List<SpawnedObject>();
    public List<SpawnedWire> spawnedWires = new List<SpawnedWire>();
    public InvItem[] itemSlots;

    Drag dragScript;
    GameObject restrictedInvPanel;

    void Start()
    {
        dragScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Drag>();
        restrictedInvPanel = GameObject.Find("Canvas").transform.Find("InventoryPanel").Find("invRestrictedInvPanel").gameObject;

        // Find all InvItems in the scene, assuming they are where they are supposed to be
        itemSlots = GameObject.Find("Canvas").transform.Find("InventoryPanel").Find("invSlotPanel").GetComponentsInChildren<InvItem>();
    }

    public void RestrictInventory(bool restrict)
    {
        restrictedInvPanel.SetActive(restrict);
    }

    public void SpawnPlayerObject(GameObject givenPrefab, InvItem originInvSlot)
    {
        // Don't spawn in an object if the player is already dragging one
        if (!dragScript.draggedObject)
        {
            originInvSlot.startingAmount--;
            originInvSlot.UpdateAmountCounter();

            // Spawn the object, and capture a reference to it in order to add it to spawnedObjects
            GameObject gameObjectInstance = (GameObject)Instantiate(givenPrefab);
			
			InteractiveObject intObj = gameObjectInstance.GetComponent<InteractiveObject>();
			if (intObj != null) {
				intObj.draggable = true;
			}
			else {
				Debug.LogError("Spawned object has no interactive object script");
			}

            // Add it to the list of spawnedObjects | m_position is set 0,0,0 since it doesnt 
            // really matter as we'll grab the objects final position when the game actually starts
            SpawnedObject newSpawnedObject = new SpawnedObject(gameObjectInstance, originInvSlot.uniqueName, new Vector3(0, 0, 0), givenPrefab, Quaternion.identity, -1);
            spawnedObjects.Add(newSpawnedObject);

            dragScript.forceDraggedObject = true;
            dragScript.SetNewDraggedObject(gameObjectInstance, new Vector2(0, 0));
        }
    }

    public void ReturnItemToInventory(GameObject objToReturn)
    {
        //int i = 0;

        Wire objsWire = null;
        // Destroy wires as well, if they exist
        if (objToReturn.transform.Find("Electrical") != null)
        {
            ConnectionPoint conPoint = objToReturn.transform.Find("Electrical").Find("ConnectionPoint").GetComponent<ConnectionPoint>();
            if (conPoint.connectedWire != null)
            {
                objsWire = conPoint.connectedWire;
            }
        }

        Destroy(objToReturn);

        if (objsWire != null)
        {
            // This will check if both connection points are valid, they won't be, therefore
            // the wire will be deleted
            objsWire.DeleteNextFrame();
        }

        // Look through spawnedObjects for objToReturn, and then break;
        foreach (SpawnedObject spawnedObject in spawnedObjects)
        {
            //i++;
            //Debug.Log(Time.frameCount + " | " + i.ToString() + " Looping through another spawned object...");

            if (spawnedObject.ActualObject == objToReturn)
            {
                //Debug.Log(Time.frameCount + " | " + "Found the object to return on loop " + i.ToString());

                foreach (InvItem invItem in itemSlots)
                {
                    // Find the item slot of objToReturn and +1 to it
                    if (invItem.uniqueName == spawnedObject.OriginInvSlot)
                    {
                        invItem.startingAmount++;
                        invItem.UpdateAmountCounter();
                        break;
                    }
                }

                // Remove objToReturns relevant SpawnedObject as otherwise errors will occur later
                spawnedObjects.Remove(spawnedObject);
                break;
            }
        }
    }

    public void GetFinalValues()
    {
        // Saving spawned objects
        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            // Because C# is smart and doesnt let me edit spawnedObjects[i] directly
            SpawnedObject thisSpawnedObject;
            thisSpawnedObject = spawnedObjects[i];

            thisSpawnedObject.V3Position = thisSpawnedObject.ActualObject.transform.position;
            thisSpawnedObject.Rotation = thisSpawnedObject.ActualObject.transform.rotation;

            if (thisSpawnedObject.ActualObject.GetComponentInChildren<ConnectionPoint>() != null)
            {
                thisSpawnedObject.connectionID = thisSpawnedObject.ActualObject.GetComponentInChildren<ConnectionPoint>().connectionID;
            }

            spawnedObjects[i] = thisSpawnedObject;
        }

        // Saving wire connections
        Wire[] allWires = GameObject.FindObjectsOfType<Wire>();
        for (int i = 0; i < allWires.Length; i++)
        {
            SpawnedWire newSpawnedWire = new SpawnedWire(allWires[i].connectionOne.connectionID, allWires[i].connectionTwo.connectionID);
            spawnedWires.Add(newSpawnedWire);
        }
    }
}
