using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] public List<Transform> Waypoints = new List<Transform>();
    [SerializeField] public float ChaseDistance;
    [SerializeField] public Player Player;
    private BaseState _currentState;
    [HideInInspector] public PatrolState PatrolState = new PatrolState();
    [HideInInspector] public ChaseState ChaseState = new ChaseState();
    [HideInInspector] public RetreatState RetreatState = new RetreatState();
    [HideInInspector] public NavMeshAgent NavMeshAgent;
    [HideInInspector] public Animator animator;
    public bool isAlive = true;
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;

    public void SwitchState(BaseState state)
    {
        _currentState.ExitState(this);
        _currentState = state;
        _currentState.EnterState(this);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        _currentState = PatrolState;
        _currentState.EnterState(this);
        NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (Player != null)
        {
            Player.OnPowerUpStart += StartRetreating;
            Player.OnPowerUpStop += StopRetreating;
        }
    }

    private void Update()
    {
        if (_currentState != null)
        {
            _currentState.UpdateState(this);
        }
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        float speed = NavMeshAgent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }

    private void StartRetreating()
    {
        SwitchState(RetreatState);
    }

    private void StopRetreating()
    {
        SwitchState(PatrolState);
    }

    public void Dead()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Dead();
        }
    }

}