using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/BallInfo")]
public class BallInfoScriptableObject : ScriptableObject
{
    public List<BallInfo> ballsInGame;
}

[System.Serializable]
public class BallInfo
{
    public float scale;
    public Color color;
    public Vector2 startingPos;
}
