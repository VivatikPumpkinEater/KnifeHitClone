using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region test

    [SerializeField] private SpriteRenderer _test = null;

    #endregion

    [Header("Menu")] [SerializeField] private GameObject _startScreen = null;
    [SerializeField] private RawImage _activeKnife = null;
    [SerializeField] private Button _start = null;
    [SerializeField] private Button _shop = null;

    [Header("GameScreen")] [SerializeField]
    private GameScreen _gameScreen = null;

    [Header("GameOverScreen")] [SerializeField]
    private GameObject _gameOverScreen = null;

    [SerializeField] private Button _restart = null;
    [SerializeField] private Button _back = null;
    [SerializeField] private TextMeshProUGUI _knifeScore = null;
    [SerializeField] private TextMeshProUGUI _bestKnifeScore = null;
    [SerializeField] private GameObject _theBestBanner = null;

    public System.Action RestartAction;
    
    private void Start()
    {
        GameManager.instance.UIManager = this;

        _start.onClick.AddListener(StartGame);
        _shop.onClick.AddListener(Shop);
        
        _restart.onClick.AddListener(RestartGame);
        _back.onClick.AddListener(BackToMenu);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            UpdateActiveKnife(_test);
        }
    }

    #region Menu

    private void StartGame()
    {
        _startScreen.SetActive(false);

        if (GameManager.instance.Player.GameStart)
        {
            Debug.Log("SecondRestart");
            RestartAction?.Invoke();
        }

        GameManager.instance.Player.GameStart = true;
    }

    private void Shop()
    {
    }

    #endregion

    #region InGame

    public void GameOver()
    {
        _gameOverScreen.SetActive(true);
        _knifeScore.text = _gameScreen.AllKnifeCount.ToString();

        if (PlayerPrefs.GetFloat("KnifeScore") < _gameScreen.AllKnifeCount)
        {
            _theBestBanner.SetActive(true);

            PlayerPrefs.SetFloat("KnifeScore", _gameScreen.AllKnifeCount);
        }

        _bestKnifeScore.text = "Best: " + PlayerPrefs.GetFloat("KnifeScore");
    }

    private void RestartGame()
    {
        _gameOverScreen.SetActive(false);

        RestartAction?.Invoke();
        //GameManager.instance.Player.GlobalRestart();
    }

    private void UpdateActiveKnife(SpriteRenderer knife)
    {
        _activeKnife.texture = knife.sprite.texture;
    }

    #endregion

    #region GameOver

    private void BackToMenu()
    {
        _startScreen.SetActive(true);
        _gameOverScreen.SetActive(false);
    }

    #endregion
}