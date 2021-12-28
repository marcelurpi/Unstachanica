using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Objective : ScriptableObject
{
    [SerializeField] private Spaceship spaceship = null;
    [SerializeField] private int piecesLeft = 0;

    public Spaceship GetSpaceship() => spaceship;
    public int GetPiecesLeft() => piecesLeft;
}
