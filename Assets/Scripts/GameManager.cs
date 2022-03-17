using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private Player _player = null;
    private GameScreen _gameScreen = null;
    private UIManager _uiManager = null;
    private GeneratorAppleAndObstacles _generatorAppleAndObstacles = null;

    private bool _initializedStatus = false;
    
    private bool _win = false, _gameOver = false;

    public System.Action StartGame;
    public System.Action GameOverRestart;

    public bool Win
    {
        get => _win;
        set => _win = value;
    }

    public bool GameOver
    {
        get => _gameOver;
        set => _gameOver = value;
    }

    public Player Player
    {
        get => _player;
        set => _player = value;
    }

    public GameScreen GameScreen
    {
        get => _gameScreen;
        set => _gameScreen = value;
    }

    public UIManager UIManager
    {
        get => _uiManager;
        set => _uiManager = value;
    }

    public GeneratorAppleAndObstacles GeneratorAppleAndObstacles
    {
        get => _generatorAppleAndObstacles;
        set => _generatorAppleAndObstacles = value;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Player && GameScreen&& UIManager && GeneratorAppleAndObstacles && !_initializedStatus)
        {
            _initializedStatus = true;
            
            Initialized();
            
            Debug.Log("Wow");
        }
    }

    public void Initialized()
    {
        StartGame?.Invoke();

        UIManager.RestartAction += RestartGame;
    }

    public void ChangeAppleValue()
    {
        GameScreen.UpdateAppleCount();
    }

    public void EndGame()
    {
        if (GameOver && !Win)
        {
            Player.Ready = false;
            
            UIManager.GameOver();
            GameScreen.GameOver();
        }
    }

    private void RestartGame()
    {
        GameOverRestart?.Invoke();
    }
}
