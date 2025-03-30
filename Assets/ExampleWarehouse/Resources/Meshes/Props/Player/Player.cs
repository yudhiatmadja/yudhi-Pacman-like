using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Action OnPowerUpStart;
    public Action OnPowerUpStop;
    public Transform _respawnPoint;
    private bool _isMagnetActive = false;
    [SerializeField] private int _health = 3;
    [SerializeField] private TextMeshProUGUI _healthText;

    [SerializeField] private float _speed = 5f;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _powerupDuration;
    [SerializeField] private float _rotationTime = 0.1f;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float teleportDistance = 10f;
    [SerializeField] private float magnetRadius = 5f;
    [SerializeField] private LayerMask coinLayer;
    // private AudioSource bgm;
    
    private Rigidbody _rigidbody;
    private Coroutine _powerupCoroutine;
    private bool _isPowerUpActive = false;
    private bool _isJumping = false;
    private bool isTeleportMode = false;
    private PlayerPunch playerPunch; 

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        HideAndLockCursor();
        UpdateHealthUI();
        playerPunch = GetComponent<PlayerPunch>();
    }

    private void HideAndLockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        HandleTeleport();
        HandleMagnet();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movementDirection = new Vector3(horizontal, 0, vertical);

        if (movementDirection.magnitude >= 0.1f)
        {
            float rotationAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);
            movementDirection = Quaternion.Euler(0f, rotationAngle, 0f) * Vector3.forward;
        }

        _rigidbody.linearVelocity = movementDirection * _speed;
        _animator.SetFloat("Velocity", _rigidbody.linearVelocity.magnitude);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_isJumping)
        {
            _isJumping = true;
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _animator.SetBool("isJump", true);
        }
    }

    private void HandleTeleport()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            isTeleportMode = !isTeleportMode;
        }

        if (isTeleportMode && Input.GetMouseButtonDown(0))
        {
            Vector3 teleportPosition = transform.position + transform.forward * teleportDistance;
            transform.position = teleportPosition;
            isTeleportMode = false;
        }
    }

    private void HandleMagnet()
    {
        Collider[] coins = Physics.OverlapSphere(transform.position, magnetRadius, coinLayer);
        foreach (Collider coin in coins)
        {
            coin.transform.position = Vector3.MoveTowards(coin.transform.position, transform.position, Time.deltaTime * _speed);
        }
    }

    public void PickPowerUp()
    {
        if (_powerupCoroutine != null)
        {
            StopCoroutine(_powerupCoroutine);
        }
        _powerupCoroutine = StartCoroutine(StartPowerUp());
    }

    private IEnumerator StartPowerUp()
    {
        _isPowerUpActive = true;
        OnPowerUpStart?.Invoke();
        yield return new WaitForSeconds(_powerupDuration);
        _isPowerUpActive = false;
        OnPowerUpStop?.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isJumping = false;
            _animator.SetBool("isJump", false);
        }
        else if (collision.gameObject.CompareTag("Enemy") && !playerPunch.IsPunching)
        {
            TakeDamage();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !playerPunch.IsPunching)
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        // currentHealth--;
        _health--;
        AudioManager.Instance.PlaySound("playerHurt");
        
        UpdateHealthUI();
        if (_health <= 0)
        {
            Die();
        }
        else
        {
            Respawn();
        }
    }

    private void UpdateHealthUI()
    {
        _healthText.text = "Health: " + _health;
    }

    private void Die()
    {
        // deathSound.Play();
        // bgm.Stop();
        gameObject.SetActive(false);
    }

    private void Respawn()
    {
        // respawnSound.Play();
        transform.position = _respawnPoint.position;
        
    }
public void AktifkanMagnet(bool aktif)
    {
        _isMagnetActive = aktif;
    }
   
}