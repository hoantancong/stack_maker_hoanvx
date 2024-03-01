using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum GameState { MainMenu, GamePlay, Pause }
public class GameManager : SingletonManager<GameManager>
{

    private int gameScore;
    private int gameLevel;
    private GameState gameState;

    public GameState GameState { get => gameState; set => gameState = value; }

    private void Start()
    {
        //FIRST INIT
        gameLevel = 1;
        GameState = GameState.MainMenu;
        //load default level
        LevelManager.Instance.LoadLevel(1);
        //
    }
    // Start is called before the first frame update
    public void StartGame()
    {
        //hide 
        UIManager.Instance.SetCoinNumber(0);
        //
        StartLevel();

    }
    public void StartLevel()
    {
        GameState = GameState.GamePlay;

        UIManager.Instance.HideLevelNumber();
    }
    public void PlayerReady()
    {
        //change state
        if (GameManager.Instance.GameState == GameState.MainMenu)
        {
            //do nothing
        }
        else
        {
            //player ready => game ready
            GameManager.Instance.GameState = GameState.GamePlay;
            UIManager.Instance.ShowLevelNumber(GameManager.Instance.gameLevel);
        }
    }
    public void AddCoin(int coin)
    {
        gameScore += coin;
        UIManager.Instance.SetCoinNumber(gameScore);
    }
    
    public void WinLevel(int coin)
    {
        gameScore += coin;
        Debug.Log(gameScore);
        UIManager.Instance.SetCoinNumber(gameScore);
        UIManager.Instance.ShowWinUI(coin);
    }

    //next or restart
    public void NextLevel()
    {
        gameLevel++;
        LevelManager.Instance.LoadLevel(gameLevel);
    }
    public void RestartLevel()
    {
        LevelManager.Instance.RestartLevel(gameLevel);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
