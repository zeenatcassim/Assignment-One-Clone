using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameState", menuName = "ScriptableObjects/GameData", order = 2)]
public class GameData : ScriptableObject
{
    public int levelsUnlocked;
    public int Score;
}
