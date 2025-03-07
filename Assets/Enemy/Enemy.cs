using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private BaseState _currentState;
    public PatrolState PatrolState = new PatrolState();
    public ChaseState ChaseState = new ChaseState();
    public RetreatState RetreatState = new RetreatState();
    [SerializeField]
    public List<Transform> Waypoints = new List<Transform>();
    [HideInInspector]
    [SerializeField]
    public float ChaseDistance;
    [SerializeField]
    public Player Player;
    public NavMeshAgent NavMeshAgent;

    private void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        _currentState = PatrolState;
        _currentState.EnterState(this);
    }

    private void Start()
    {
        if (Player != null)
        {
            Player.OnPowerUpStart += StartRetreating;
            Player.OnPowerUpStop += StopRetreating;
        }
        NavMeshAgent.isStopped = false;
        NavMeshAgent.updateRotation = true;

        if (NavMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent tidak ditemukan pada Enemy!");
        }
        else if (!NavMeshAgent.isOnNavMesh)
        {
            Debug.LogError("Enemy tidak berada di atas NavMesh!");
        }
    }

    private void Update()
    {
        if (_currentState != null)
        {
            _currentState.UpdateState(this);
        }
        if (Player == null)
        {
            Debug.LogError("Player tidak ditemukan!");
        }
    }
    public void SwitchState(BaseState state)
    {
        Debug.Log("Beralih dari " + _currentState.GetType().Name + " ke " + state.GetType().Name);

        if (_currentState != null)
        {
            _currentState.ExitState(this);
        }

        _currentState = state;
        _currentState.EnterState(this);
    }

    private void StartRetreating()
    {
        SwitchState(RetreatState);
    }

    private void StopRetreating()
    {
        SwitchState(PatrolState);
    }
}