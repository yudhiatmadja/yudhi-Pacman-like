using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    private bool _isMoving = false;
    private Vector3 _destination;

    public void EnterState(Enemy enemy)
    {
        _isMoving = false;
    }

    public void UpdateState(Enemy enemy)
    {
        if (Vector3.Distance(enemy.transform.position, enemy.Player.transform.position) < enemy.ChaseDistance)
        {
            enemy.SwitchState(enemy.ChaseState);
            return;
        }

        if (!_isMoving)
        {
            int index = Random.Range(0, enemy.Waypoints.Count);
            _destination = enemy.Waypoints[index].position;

            enemy.NavMeshAgent.SetDestination(_destination);
            _isMoving = true;
        }
        else
        {
            if (enemy.NavMeshAgent.remainingDistance <= enemy.NavMeshAgent.stoppingDistance)
            {
                _isMoving = false;
            }
        }
    }

    public void ExitState(Enemy enemy)
    {
        Debug.Log("Stop Patrol");
    }
}
