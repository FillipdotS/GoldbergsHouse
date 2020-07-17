using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    GameObject loadingOverlay;
    GameObject loadingOverlayInstance;

    // Static singleton instance
    private static SceneLoader instance;

    // Static singleton property
    public static SceneLoader Instance
    {
        // Here we use the ?? operator, to return 'instance' if 'instance' does not equal null
        // otherwise we assign instance to a new component and return that
        get { return instance ?? ( instance = CreateSingleton() ); }
    }

    private static SceneLoader CreateSingleton()
    {
        SceneLoader newInstance;
        newInstance = new GameObject("SceneLoader").AddComponent<SceneLoader>();
        newInstance.loadingOverlay = Resources.Load("LoadingOverlay") as GameObject;
        DontDestroyOnLoad(newInstance.gameObject);
        return newInstance;
    }

    public void ShowLoading()
    {
        loadingOverlayInstance = Instantiate(loadingOverlay, GameObject.Find("Canvas").transform);
        if (loadingOverlayInstance != null)
        {
            Debug.LogWarning("Called ShowLoading when loadingOverlayInstance is not null");
        }
    }

    public void HideLoading()
    {
        Destroy(loadingOverlayInstance);
        if (loadingOverlayInstance == null)
        {
            Debug.LogWarning("Called HideLoading when loadingOverlayInstance is null");
        }
    }

    // Instance method, this method can be accesed through the singleton instance
    public void LoadScene(int buildIndex, params Action[] callAfter)
    {
        // We don't need to set loadingOverlayInstance, since it will be removed anyway
        // when the new scene loads
        ShowLoading();
        if (callAfter != null)
        {
            StartCoroutine(AsyncLoadScene(buildIndex, callAfter));
        }
        else
        {
            StartCoroutine(AsyncLoadScene(buildIndex));
        }
    }

    IEnumerator AsyncLoadScene(int buildIndex, params Action[] callAfter)
    {
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(buildIndex);

        Debug.Log("Pro :" + asyncOperation.progress);
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress
            Debug.Log("Loading progress: " + (asyncOperation.progress * 100) + "%");

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                Debug.Log("Finished");
                foreach (Action method in callAfter)
                {
                    method();
                }
            }

            yield return null;
        }
    }
}
