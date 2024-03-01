using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : SingletonManager<LevelManager>
{
    [SerializeField] GameObject playerPrefab; // Prefab của Player để khởi tạo
    private GameObject currentPlayer; // Tham chiếu đến thể hiện hiện tại của Player
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void InitLevel()
    {
        //Load level
        //Init player
    }
    private IEnumerator LoadLevelAdditiveAsync(string levelName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);

        // wait for load has finished
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        LoadPlayer();
     
    }
    public void LoadLevel(int levelNumber)
    {

        //unload old level if exits
        if (SceneManager.GetSceneByName($"Level{levelNumber-1}").isLoaded)
        {
            SceneManager.UnloadSceneAsync($"Level{levelNumber - 1}").completed += (AsyncOperation operation) =>
            {
                Debug.Log("Unload scene");
                StartCoroutine(LoadLevelAdditiveAsync($"Level{levelNumber}"));
               
            };
        }
        else
        {
            StartCoroutine(LoadLevelAdditiveAsync($"Level{levelNumber}"));
        }
       
    }
    public void RestartLevel(int levelNumber)
    {
        if (SceneManager.GetSceneByName($"Level{levelNumber}").isLoaded)
        {
            SceneManager.UnloadSceneAsync($"Level{levelNumber}").completed += (AsyncOperation operation) =>
            {
                Debug.Log("Unload scene");
                StartCoroutine(LoadLevelAdditiveAsync($"Level{levelNumber}"));

            };
        }
    }
    private void LoadPlayer()
    {
        //find position
        Vector3 playerPos = GameObject.Find("PlayerPosition").transform.position;
        if (currentPlayer == null)
        {
            currentPlayer = Instantiate(playerPrefab, playerPos, Quaternion.identity);
        }
        else
        {
            currentPlayer.transform.position = playerPos;
            //reset state
            currentPlayer.GetComponent<PlayerController>().OnInit();
        }
        //after finished loading player change state to start game...
        //if first time
        GameManager.Instance.PlayerReady();
 
        //
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
