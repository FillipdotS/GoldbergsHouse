using UnityEngine;

public class Drag : MonoBehaviour {

	Vector3 offset;
    Color32 previousColor;
    string previousLayer;
    float previousZCord;
    float dustParticleSystemLifetime; // total emission duration + lifetime of a particle

    AdjustObjects adjustObjectsScript;

    [HideInInspector]
    public GameObject draggedObject;
    [HideInInspector]
    public bool forceDraggedObject = false; // hacky way to get around how buttons work

    public AudioClip placedSound;
    public AudioClip takeSound;

    public GameObject dustParticles;
    public Color32 draggingColor = new Color32(160, 255, 140, 140);
    public Color32 deletionColor; // Not used in this script, but InvItem and GarbageArea use this

    // Every left click does a raycast, if an object with the "InteractiveObject" tag
	// is hit with and its 'draggable' set to true, draggedObject is set to that object and until the mouse is released
    // draggedObject will follow the mouse position

    void Start()
    {
        ParticleSystem particleSystem = dustParticles.GetComponent<ParticleSystem>(); // easier to read
        dustParticleSystemLifetime = particleSystem.main.duration + particleSystem.main.startLifetimeMultiplier;

        adjustObjectsScript = transform.GetComponent<AdjustObjects>();
    }

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0) && !draggedObject) // Left click down
        {
            CastRayOnInteractiveObjects();
        }
        // Left click up or right click up
        if ( (Input.GetMouseButtonUp(0) && draggedObject && !forceDraggedObject) || (Input.GetMouseButtonDown(1) && draggedObject) )
        {
            AudioController.Instance.audioSource.PlayOneShot(placedSound);
            // Creates a dust effect when placing the object. objPos is just there for easy reading.
            Vector3 objPos = draggedObject.transform.position;
            GameObject instantiatedParticles = Instantiate(dustParticles, new Vector3(objPos.x, objPos.y, dustParticles.transform.position.z), Quaternion.identity);
            Destroy(instantiatedParticles, dustParticleSystemLifetime);

            ResetDraggedObjectValues();
            draggedObject = null;
        }
        if (draggedObject)
        {
            // Mouse position
            Vector2 camToMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // easier reading
            draggedObject.transform.position = new Vector3(camToMousePos.x, camToMousePos.y, -6f) + offset;

            forceDraggedObject = false;
        }

    }

    // Called by this script and also inventory items when clicked (whose click position will just be 0)
    public void SetNewDraggedObject(GameObject newDraggedObject, Vector2 clickPosition)
    {
        AudioController.Instance.audioSource.PlayOneShot(takeSound);

        Vector3 newClickPos = new Vector3(clickPosition.x, clickPosition.y, 0);

        // When dragging hide the modifyButtons
        adjustObjectsScript.HideButtons();


        draggedObject = newDraggedObject;

        // Remember some variables since we are going to change them
        previousColor = draggedObject.transform.GetComponent<SpriteRenderer>().material.color;
        previousLayer = draggedObject.GetComponent<Renderer>().sortingLayerName;
        previousZCord = draggedObject.transform.position.z;

        draggedObject.GetComponent<Renderer>().sortingLayerName = "AboveAll";
        draggedObject.transform.GetComponent<SpriteRenderer>().material.color = draggingColor;

        // Used when setting draggedObject's transform.position so that where the player clicked
        // is where the object is being dragged
        offset = new Vector3(draggedObject.transform.position.x, draggedObject.transform.position.y, 0) - newClickPos;
    }

    // Called when object is released and also when the game begins, as otherwise objects could remain green etc
    public void ResetDraggedObjectValues()
    {
        if (draggedObject)
        {
            draggedObject.transform.GetComponent<SpriteRenderer>().material.color = previousColor;
            draggedObject.GetComponent<Renderer>().sortingLayerName = previousLayer;
            draggedObject.transform.position = new Vector3(draggedObject.transform.position.x, draggedObject.transform.position.y, previousZCord);
        }
    }

    void CastRayOnInteractiveObjects()
    {
        Vector2 rayPosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D hit = Physics2D.Raycast(rayPosition, Vector2.zero, 0f);

        Debug.DrawLine(rayPosition, hit.point, Color.cyan, 5f);

        if (hit)
        {
            if (hit.transform.tag == "InteractiveObject")
            {
				if (hit.transform.gameObject.GetComponent<InteractiveObject>().draggable) {
                    SetNewDraggedObject(hit.transform.gameObject, rayPosition);
				}
            }
        }
    }
}
