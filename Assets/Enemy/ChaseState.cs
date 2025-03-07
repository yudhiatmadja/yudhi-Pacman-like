using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : BaseState
{
    public void EnterState(Enemy enemy)
    {
        Debug.Log("Start Chasing");
        enemy.NavMeshAgent.isStopped = false; // Pastikan Agent tidak berhenti
        enemy.NavMeshAgent.updateRotation = true; // Agar bisa berputar menghadap Player
    }
    public void UpdateState(Enemy enemy)
    {
        if (enemy.Player == null) return;

        enemy.NavMeshAgent.isStopped = false; // Pastikan Agent bisa bergerak
        enemy.NavMeshAgent.destination = enemy.Player.transform.position;

        Debug.Log("Mengejar Player ke posisi: " + enemy.Player.transform.position);

        if (Vector3.Distance(enemy.transform.position, enemy.Player.transform.position) > enemy.ChaseDistance)
        {
            Debug.Log("Jarak terlalu jauh, kembali ke PatrolState");
            enemy.SwitchState(enemy.PatrolState);
        }
    }

    public void ExitState(Enemy enemy)

    {

        Debug.Log("Stop Chasing");

    }

}