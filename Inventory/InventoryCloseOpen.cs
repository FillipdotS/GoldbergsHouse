using UnityEngine;

public class InventoryCloseOpen : MonoBehaviour
{
    GameObject invPanel;

    void Start()
    {
        invPanel = GameObject.FindGameObjectWithTag("InventoryPanel");
    }

    public void CloseInv()
    {
        invPanel.SetActive(false);
    }

    public void OpenInv()
    {
        invPanel.SetActive(true);
    }
}
