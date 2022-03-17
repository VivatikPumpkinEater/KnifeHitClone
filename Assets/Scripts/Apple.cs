using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Apple : MonoBehaviour
{
    #region Testing

    [SerializeField] private GameObject _target = null;

    #endregion
    
    [SerializeField] private GameObject _halfApple = null;

    private SpriteRenderer _fullApple = null;

    private bool _sliced = false;

    private void Start()
    {
        _fullApple = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Knife") && !_sliced && !GameManager.instance.Win && !GameManager.instance.GameOver)
        {
            _sliced = true;
            
            SlicedApple();
            
            Destroy(gameObject,2f);
        }
    }

    private void SlicedApple()
    {
        Destroy(_fullApple);
        
        transform.parent = null;
        
        for (int i = 0; i < 2; i++)
        {
            var halfApple = Instantiate(_halfApple, transform);
            
            halfApple.transform.parent = null;
            
            if (i % 1 == 0)
            {
                halfApple.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 20, ForceMode2D.Impulse);
            }
            else
            {
                halfApple.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 20, ForceMode2D.Impulse);
            }
            
            Destroy(halfApple, 2f);
        }
        
        GameManager.instance.ChangeAppleValue();
    }
}
