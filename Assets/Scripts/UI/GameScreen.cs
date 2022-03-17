using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameScreen : MonoBehaviour
{
    [SerializeField] private GameObject _knifeCount = null;
    [SerializeField] private RawImage _knifeIcon = null;
    [SerializeField] private Texture2D _disableIcon = null;

    [SerializeField] private TextMeshProUGUI _allKnifeCountTxt = null;
    [SerializeField] private TextMeshProUGUI _appleCountTxt = null;

    private int _allKnifeCount = 0;
    private int _appleCount = 0;

    public int AllKnifeCount
    {
        get => _allKnifeCount;
    }
    
    private float _alpha = 0.3f;

    private List<RawImage> _icons = new List<RawImage>();

    private void Start()
    {
        if (!PlayerPrefs.HasKey("AppleCount"))
        {
            PlayerPrefs.SetInt("AppleCount", _appleCount);
        }

        _appleCount = PlayerPrefs.GetInt("AppleCount");
        _appleCountTxt.text = _appleCount.ToString();
        
        GameManager.instance.GameScreen = this;

        GameManager.instance.StartGame += StartGame;
        //GameManager.instance.GameOverRestart += RestartUI;
    }

    private void StartGame()
    {
        FillKnifeCountPanel(GameManager.instance.Player.KnifeCount);

        GameManager.instance.Player.DropKnife += DisableIcon;
        GameManager.instance.Player.RestartGame += Continue;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            DisableAll();
        }
    }

    private void FillKnifeCountPanel(int knifeCount)
    {
        for (int i = 0; i < knifeCount; i++)
        {
            _icons.Add(Instantiate(_knifeIcon, _knifeCount.transform));
        }
    }

    private void DisableIcon()
    {
        //if(_icons.Count != 0)
        //{
        //    _icons[0].color = new Color(0, 0, 0, _alpha);

        //    _icons.RemoveAt(0);
        //}

        if (_icons.Count != 0)
        {
            _icons[0].texture = _disableIcon;
            _icons.RemoveAt(0);
        }
        
        UpdateKnifeCount();
    }

    private void DisableAll()
    {
        GameManager.instance.Player.DropKnife -= DisableIcon;
        
        ClearKnivesIcon();
    }

    private void ClearKnivesIcon()
    {
        var icons = _knifeCount.GetComponentsInChildren<RawImage>();

        for (int i = 0; i < icons.Length; i++)
        {
            Destroy(icons[i].gameObject);
        }
        
        _icons.Clear();
    }

    public void GameOver()
    {
        ClearKnivesIcon();
    }
    
    public void RestartUI()
    {
        FillKnifeCountPanel(GameManager.instance.Player.KnifeCount);
        _allKnifeCount = 0;
        
        _allKnifeCountTxt.text = _allKnifeCount.ToString();
    }
    
    private void Continue()
    {
        ClearKnivesIcon();
        
        FillKnifeCountPanel(GameManager.instance.Player.KnifeCount);
    }

    private void UpdateKnifeCount()
    {
        _allKnifeCount++;
        
        Debug.Log("Knife++");
        
        _allKnifeCountTxt.text = _allKnifeCount.ToString();
    }

    public void UpdateAppleCount()
    {
        _appleCount += 2;
        
        _appleCountTxt.text = _appleCount.ToString();
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("AppleCount", _appleCount);
        
        DisableAll();
    }
}