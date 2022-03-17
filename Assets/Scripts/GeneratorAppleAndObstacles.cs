using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GeneratorAppleAndObstacles : MonoBehaviour
{
    [SerializeField] private ChanceApple _apple = null;
    [SerializeField] private Knife _knifeObstacle = null;
    [SerializeField] private float _radius = 0f;

    private float _angel = 0;

    private void Start()
    {
        GameManager.instance.GeneratorAppleAndObstacles = this;

        GameManager.instance.StartGame += Initialized;
    }

    private void Initialized()
    {
        GameManager.instance.Player.RestartGame += RestartGenerate;
        
        if (Random.Range(0, 100) > _apple.Chance)
        {
            Generate();
        }
        
        GenerateObstacles();
    }

    private void Generate()
    {
        _angel = Random.Range(0, 360);

        if (_angel % 5 != 0)
        {
            while (_angel % 5 != 0)
            {
                _angel = Random.Range(0, 361);
            }
        }
        
        var apple = Instantiate(_apple.Apple, transform);
        apple.transform.position = FindSpawnPosition();

        ReRotation(apple.transform, mirrorY: true);
    }

    private void GenerateObstacles()
    {
        int count = Random.Range(0, 4);

        for (int i = 0; i < count; i++)
        {
            _angel = Random.Range(0, 360);

            if (_angel % 5 != 0)
            {
                while (_angel % 5 != 0)
                {
                    _angel = Random.Range(0, 361);
                }
            }
            
            var obstacleKnife = Instantiate(_knifeObstacle, transform);
            obstacleKnife.transform.position = FindSpawnPosition();

            obstacleKnife.Rigidbody2D.isKinematic = true;
            obstacleKnife.IsFire = true;
            obstacleKnife.IsHitted = true;

            ReRotation(obstacleKnife.transform, mirrorY: false);
        }
    }

    private Vector2 FindSpawnPosition()
    {
        float positionX = transform.position.x + Mathf.Cos(_angel) * _radius;
        float positionY = transform.position.y + Mathf.Sin(_angel) * _radius;

        return new Vector2(positionX, positionY);
    }

    private void ReRotation(Transform reRotateObject, bool mirrorY)
    {
        Vector2 direction = transform.position - reRotateObject.position;

        if (mirrorY)
        {
            _angel = Mathf.Atan2(direction.x, -direction.y);
        }
        else
        {
            _angel = Mathf.Atan2(-direction.x, direction.y);
        }

        reRotateObject.rotation = quaternion.Euler(0, 0, _angel);
    }

    private void RestartGenerate()
    {
        if (Random.Range(0, 100) >= _apple.Chance)
        {
            Generate();
        }
        
        GenerateObstacles();
    }
}