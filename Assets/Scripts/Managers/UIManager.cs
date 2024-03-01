using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : SingletonManager<UIManager>
{
    //ui
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject winUI;
    //
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI levelNumberText;
    [SerializeField] private TextMeshProUGUI levelCoinText;
    public void StartGame()
    {
        menuUI.SetActive(false);
        GameManager.Instance.StartGame();
    }
    public void ShowLevelNumber(int level)
    {
        StartCoroutine(Show3sLevelTitle());
        levelNumberText.text = $"Level {level}";

    }
    IEnumerator Show3sLevelTitle()
    {
        Debug.Log("Show level");
        levelNumberText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        levelNumberText.gameObject.SetActive(false);
    }
    public void HideLevelNumber()
    {
        Debug.Log("hide");
        levelNumberText.gameObject.SetActive(false);
    }
    public void SetCoinNumber(int coin)
    {
        coinText.text = $"{coin}";
    }
    //WIN UI HANDLE
    public void ShowWinUI(int levelCoin)
    {
        levelCoinText.text = $"Coin {levelCoin}";
        winUI.SetActive(true);
        
    }
    public void OnNextLevel()
    {
        winUI.SetActive(false);
        GameManager.Instance.NextLevel();
    }
    public void OnRestartLevel()
    {
        winUI.SetActive(false);
        GameManager.Instance.RestartLevel();
    }

}
