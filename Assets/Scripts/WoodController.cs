using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WoodController : MonoBehaviour
{
    [SerializeField] private int _rotationSpeed = 200;
    [SerializeField] private float _timer = 5;

    [SerializeField] private GameObject _destroyVariant = null;

    [SerializeField] private bool _randomizeDirection = false;

    #region Testing

    [SerializeField] private Player _player = null;

    [SerializeField] private GameObject _fullVariant = null;

    #endregion


    private Coroutine _coroutine = null;

    private bool _test = true;

    private void Start()
    {
        GameManager.instance.GameOverRestart += GameOverRestart;
        
        _player.Win += DestroyWood;
        _player.RestartGame += RestartWood;
        Initialized();
    }

    private void Initialized()
    {
        if (_randomizeDirection)
        {
            _coroutine = StartCoroutine(RotationWood());
        }
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, -_rotationSpeed * Time.deltaTime));

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _test = false;
        }
    }

    private IEnumerator RotationWood()
    {
        while (_test)
        {
            int speed = Random.Range(100, 251);

            if (Random.Range(0, 2) == 1)
            {
                speed *= -1;
            }

            while (_rotationSpeed != speed)
            {
                if (_rotationSpeed > speed)
                {
                    _rotationSpeed--;
                    yield return null;
                }

                if (_rotationSpeed < speed)
                {
                    _rotationSpeed++;
                    yield return null;
                }
            }

            yield return new WaitForSecondsRealtime(_timer);
        }

        Debug.Log("end");
    }

    private void DestroyWood()
    {
        //gameObject.SetActive(false);
        //Destroy(_fullVariant);
        _fullVariant.SetActive(false);

        var circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = false;

        var destroyWood = Instantiate(_destroyVariant, transform);
        destroyWood.transform.parent = null;

        var collider = Physics.OverlapSphere(transform.position, 3f);

        for (int i = 0; i < collider.Length; i++)
        {
            Rigidbody col = collider[i].attachedRigidbody;

            if (col)
            {
                col.isKinematic = false;

                col.AddExplosionForce(1000, transform.position + (Vector3.back + Vector3.down), 4.5f);
            }
        }
        
        ClearChildren();

        Destroy(destroyWood,5f);
        //Explosion(transform.position, 1, 200f);
    }

    private void ClearChildren()
    {
        var toDestroyElement = GetComponentsInChildren<Rigidbody2D>();

        for (int i = 0; i < toDestroyElement.Length; i++)
        {
            Destroy(toDestroyElement[i].gameObject);
        }
    }

    private void RestartWood()
    {
        _fullVariant.SetActive(true);

        var circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = true;
    }

    private void GameOverRestart()
    {
        ClearChildren();
        
        RestartWood();
    }
}