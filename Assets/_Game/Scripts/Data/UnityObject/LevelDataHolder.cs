using System.Collections;
using System.Collections.Generic;
using Scripts.Data.ValueObject;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData",menuName = "ScriptableObjects/LevelData",order = 1)]
public class LevelDataHolder : ScriptableObject
{
    public LevelData LevelData;
}
