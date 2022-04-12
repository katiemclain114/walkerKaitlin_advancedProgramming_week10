using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public BallInfoScriptableObject ballInfo;
    public GameObject ballPrefab;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        var ballInfoList = ballInfo.ballsInGame;
        for (int i = 0; i < ballInfoList.Count; i++)
        {
            var newBall = Instantiate(ballPrefab);
            var ballAgent = newBall.GetComponent<AgentMove>();
            ballAgent.BallStart(i, ballInfoList[i].scale, ballInfoList[i].startingPos, ballInfoList[i].color);
        }
    }

    public void ResetColor(int placeInList, Color newColor)
    {
        ballInfo.ballsInGame[placeInList].color = newColor;
    }
}
