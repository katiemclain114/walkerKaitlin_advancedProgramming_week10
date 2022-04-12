using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AgentMove : MonoBehaviour
{
    [HideInInspector]
    public float waitTime;
    public float maxWaitTime;
    [HideInInspector]
    public float timeWaited = 0;

    public NavMeshAgent agent;
    public Vector3 currentMovePos;

    public Vector2 minMaxX;
    public Vector2 minMaxZ;


    private Material _mat;
    private Tree<AgentMove> _tree;

    public int ballNum;

    public void BallStart(int ballNum, float scaleNum, Vector2 startingPos, Color startColor)
    {
        _mat = GetComponent<MeshRenderer>().material;
        agent = GetComponent<NavMeshAgent>();
        SetBallScale(scaleNum);
        SetBallStartingPos(startingPos);
        SetBallStartColor(startColor);
        this.ballNum = ballNum;
        
        Reset();
        ConstructBehaviorTree();
    }

    private void SetBallScale(float scaleNum)
    {
        transform.localScale = new Vector3(scaleNum, scaleNum, scaleNum);
    }

    private void SetBallStartingPos(Vector2 startingPos)
    {
        var yPos = transform.localScale.x / 2;
        transform.position = new Vector3(startingPos.x, yPos, startingPos.y);
    }

    private void SetBallStartColor(Color startColor)
    {
        _mat.color = startColor;
    }

    private void ConstructBehaviorTree()
    {
        WaitNode waitNode = new WaitNode();
        GoToSpot goToSpotNode = new GoToSpot();

        var moveSequence = new Sequence<AgentMove>(waitNode, goToSpotNode);
        _tree = new Tree<AgentMove>(moveSequence);
    }

    private void Update()
    {
        _tree.Update(this);
    }

    public void Reset()
    {
        waitTime = Random.Range(.5f, maxWaitTime);
        timeWaited = 0;
        var randomX = Random.Range(minMaxX.x, minMaxX.y);
        var randomZ = Random.Range(minMaxZ.x, minMaxZ.y);
        currentMovePos = new Vector3(randomX, transform.position.y, randomZ);
    }

    private void OnMouseDown()
    {
        var randomColor = Random.ColorHSV();
        _mat.color = randomColor;
        GameManager.instance.ResetColor(ballNum, randomColor);
    }
}

public class WaitNode : Node<AgentMove>
{
    public override bool Update(AgentMove context)
    {
        if(context.timeWaited <= context.waitTime) context.timeWaited += Time.deltaTime;
        return context.timeWaited >= context.waitTime;
    }
}

public class GoToSpot : Node<AgentMove>
{
    public override bool Update(AgentMove context)
    {
        float distance = Vector3.Distance(context.currentMovePos, context.agent.transform.position);
        if (distance > .2f)
        {
            context.agent.isStopped = false;
            context.agent.SetDestination(context.currentMovePos);
            return true;
        }

        context.agent.isStopped = true;
        context.Reset();
        return false;
    }
}
