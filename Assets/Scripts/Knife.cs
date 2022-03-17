using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class Knife : MonoBehaviour
{
    [SerializeField] private float _speed = 80;

    private Player _playerController;

    private Player _player
    {
        get => _playerController = _playerController ?? GetComponentInParent<Player>();
    }

    //private bool _usedKnife = false;
    private bool _inWood = false;
    private bool _gameOver = false;
    private bool _isFire = false;
    private bool _isHitted = false;

    public bool IsFire
    {
        get => _isFire;
        set => _isFire = value;
    }

    public bool IsHitted
    {
        get => _isHitted;
        set => _isHitted = value;
    }

    private Rigidbody2D _rigidbody2D = null;

    public Rigidbody2D Rigidbody2D
    {
        get => _rigidbody2D = _rigidbody2D ?? GetComponent<Rigidbody2D>();
    } 

    public bool InWood
    {
        get => _inWood;
    }

    private void Start()
    {
        Vibration.Init();
        
        Rigidbody2D.isKinematic = true;
        if(_player != null)
        {
            _player.DropKnife += DropKnife;
        }
    }

    private void DropKnife()
    {
        if (!_isFire)
        {
            _isFire = true;

            Rigidbody2D.isKinematic = false;
            Rigidbody2D.AddForce(new Vector2(0f, _speed), ForceMode2D.Impulse);

            _player.DropKnife -= DropKnife;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Knife knife = col.gameObject.GetComponent<Knife>();

        if (col.gameObject.CompareTag("Knife") && !_isHitted && _isFire && col.gameObject.GetComponent<Knife>().IsFire && !GameManager.instance.GameOver)
        {
            Vibration.Vibrate();
            
            _isHitted = true;
            
            transform.parent = null;
            
            Rigidbody2D.AddTorque(5f, ForceMode2D.Impulse);

            GameManager.instance.GameOver = true;
            
            GameManager.instance.EndGame();

            //Rigidbody2D.velocity = Vector2.zero;
            //
            //
            //Rigidbody2D.angularVelocity = Random.Range(20f, 50f) * 25f;
            //Rigidbody2D.AddForce(new Vector2(Random.Range(-5f, 5f), -30f), ForceMode2D.Impulse);
        }
        else if (col.gameObject.CompareTag("Wood") && !_isHitted)
        {
            Vibration.VibratePeek();
            
            _isHitted = true;
            
            Rigidbody2D.isKinematic = true;
            Rigidbody2D.velocity = Vector2.zero;
            
            transform.parent = col.transform;
        }
    }
}