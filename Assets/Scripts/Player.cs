using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [SerializeField] private Knife _knifePrefab = null;

    [SerializeField] private int _knivesCount = 8;

    [SerializeField] private List<Knife> _knives = new List<Knife>();

    private bool _gameStart = false, _ready = false;

    public bool Ready
    {
        get => _ready;
        set => _ready = value;
    }
    
    public bool GameStart
    {
        get => _gameStart;
        set => _gameStart = value;
    }
    
    public int KnifeCount
    {
        get => _knivesCount;
        set => _knivesCount = value;
    }

    public System.Action DropKnife;
    public System.Action Win;
    public System.Action RestartGame;

    private void Start()
    {
        //_gameManager.SetPlayer(this);
        GameManager.instance.Player = this;

        GameManager.instance.StartGame += StartGame;
        GameManager.instance.GameOverRestart += GameOverRestart;
        
        _ready = true;
    }

    private void StartGame()
    {
        
    }
    
    private void Update()
    {
        
        if(_gameStart && !GameManager.instance.Win && !GameManager.instance.GameOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                DropKnife?.Invoke();
            }

            if (Input.GetMouseButtonUp(0))
            {
                SpawnKnife();
            }
        }
    }

    private void SpawnKnife()
    {
        if (_knives.Count < _knivesCount && !GameManager.instance.GameOver)
        {
            _knives.Add(Instantiate(_knifePrefab, transform));
        }
        else
        {
            foreach (var knife in _knives)
            {
                knife.transform.parent = null;
                knife.Rigidbody2D.isKinematic = false;
                
                knife.Rigidbody2D.AddTorque(5f, ForceMode2D.Impulse);
            }
            
            Win?.Invoke();

            GameManager.instance.Win = true;
            
            Debug.Log("Win");
            
            StartCoroutine(ClearKnife());
        }
    }

    private IEnumerator ClearKnife()
    {
        for (int i = 0; i < _knives.Count; i++)
        {
            Destroy(_knives[i].gameObject, Random.Range(1,4));
        }

        while (_knives.Count != 0)
        {
            for (int i = 0; i < _knives.Count; i++)
            {
                if (_knives[i] == null)
                {
                    _knives.Remove(_knives[i]);
                    break;
                }
            }

            yield return null;
        }
        
        Debug.Log("Clear: done");
        
        Restart();
    }

    private void Restart()
    {
        _knivesCount = Random.Range(5, 11);
        GameManager.instance.Win = false;
        GameManager.instance.GameOver = false;
        
        RestartGame?.Invoke();
        
        SpawnKnife();
    }

    private void GameOverRestart()
    {
        for (int i = 0; i < _knives.Count; i++)
        {
            Destroy(_knives[i].gameObject);
        }
        
        _knives.RemoveRange(0, _knives.Count);
        
        _knivesCount = Random.Range(5, 11);
        
        GameManager.instance.GameScreen.RestartUI();
        
        GameManager.instance.Win = false;
        GameManager.instance.GameOver = false;
    }
}
