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
        if (enemy.Player == null)
        {
            Debug.LogError("Player tidak ditemukan!");
            return;
        }

        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);
        // Debug.Log("Jarak ke Player: " + distanceToPlayer + ", ChaseDistance: " + enemy.ChaseDistance);

        if (distanceToPlayer < enemy.ChaseDistance)
        {
            Debug.Log("Player dalam jarak kejar, switching ke ChaseState");
            enemy.SwitchState(enemy.ChaseState);
            return;
        }

        // Jika tidak sedang mengejar, lakukan patrol
        if (!_isMoving)
        {
            if (enemy.Waypoints.Count == 0)
            {
                Debug.LogWarning("Tidak ada waypoint yang tersedia!");
                return;
            }

            int index = Random.Range(0, enemy.Waypoints.Count);
            _destination = enemy.Waypoints[index].position;

            enemy.NavMeshAgent.isStopped = false;
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
