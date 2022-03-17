using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChanceApple", menuName = "Chance/Apple")]
public class ChanceApple : ScriptableObject
{
    [field: SerializeField] public Apple Apple { get; private set; }
    [field: Range(0,100)]
    [field: SerializeField] public float Chance { get; private set; }
}
