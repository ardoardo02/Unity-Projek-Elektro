using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoaderManager : MonoBehaviour
{
    [SerializeField] GameObject[] destroyGameObjects;

    public static GameObject[] destroyObjects;
    private void Awake() {
        destroyObjects = destroyGameObjects;
    }

    // level
    public static void Load(string sceneName)
    {
        SceneLoader.Load(sceneName);

        if (destroyObjects.Length > 0) {
            foreach (GameObject gameObject in destroyObjects) {
                Destroy(gameObject);
            }
        }
    }

    // progress load
    // public static void ProgressLoad(string sceneName)
    // {
    //     SceneLoader.ProgressLoad(sceneName);
    // }

    // reload level
    public static void ReloadLevel()
    {
        SceneLoader.ReloadLevel();
    }

    // load next level
    public static void LoadNextLevel()
    {
        SceneLoader.LoadNextLevel();
    }

    // quit game
    public static void QuitGame()
    {
        Application.Quit();
    }
}