using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustObjects : MonoBehaviour {

    GameObject modifyButtons;
    ModifyMethods modifyMethods;

    void Start()
    {
        modifyButtons = GameObject.Find("Canvas").transform.Find("ModifyButtons").gameObject;
        modifyMethods = modifyButtons.GetComponent<ModifyMethods>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right click
        {
            if (modifyButtons.activeInHierarchy) // active = already selected, therefore hide
            {
                modifyButtons.SetActive(false);
            }
            else
            {
                CastRayOnInteractiveObjects();
            }
        }
    }

    public void HideButtons()
    {
        modifyButtons.SetActive(false);
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
                if (hit.transform.gameObject.GetComponent<InteractiveObject>().draggable)
                {
                    Vector3 hitPos = hit.transform.position; // makes it easier to see whats happening below
                    // Sets modifyButtons to be on the object BUT keeps its previous z position
                    modifyButtons.transform.position = new Vector3(hitPos.x, hitPos.y, modifyButtons.transform.position.z);
                    modifyButtons.SetActive(true);
                    modifyMethods.selectedObject = hit.transform.gameObject;
                }
            }
        }
    }
}
