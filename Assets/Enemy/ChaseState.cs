using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ChaseState : BaseState
{
    public void EnterState(Enemy enemy)
    {
      // if (enemy.isAlive && enemy.animator != null)
      // {
      //  enemy.animator.SetTrigger("ChaseState");
      // }
    }

    public void UpdateState(Enemy enemy)
    {
       if (enemy.Player != null)
       {
            enemy.NavMeshAgent.destination = enemy.Player.transform.position;
            if (Vector3.Distance(enemy.transform.position, enemy.Player.transform.position) > enemy.ChaseDistance)
            {
                enemy.SwitchState(enemy.PatrolState);
            }
       }
    }

    public void ExitState(Enemy enemy)
    {
      Debug.Log("Stop Chase");
    }
}