using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private float _speed = 5f;
    private float _sprintMultiplier = 2f;
    private float _mouseSensitivity = 2f;
    private Transform _cameraTransform;
    private float _rotationX = 0f;
    private bool _isMagnetActive = false;
    private float _magnetRange = 5f;

    private float _jumpForce = 5f;
    private bool _isGrounded;

    private bool _isTeleporting = false;
    [SerializeField]
    private float _powerupDuration;
    private Coroutine _powerupCoroutine;
    public Action OnPowerUpStart;

    public Action OnPowerUpStop;
    private bool isPowerUpActive = false;
    private AudioSource enemyDeathAudioSource;
    public AudioClip enemyDeath;
    // private AudioSource gameOverAudioSource;
    // public AudioClip gameOverAudio;

    private AudioSource respawnAudioSource;
    public AudioClip respawnAudio;
    // [SerializeField] private GameObject gameOver;
    [SerializeField] private int _health = 3;
    [HideInInspector] public Animator animator;
    [SerializeField] Transform _respawnPoint;
    [SerializeField] private float blinkDuration = 1f;
    [SerializeField] private int blinkCount = 5;
    private Renderer _renderer;


    private void Start()
    {
        enemyDeathAudioSource = GetComponent<AudioSource>();
        // gameOverAudioSource = GetComponent<AudioSource>();
        respawnAudioSource = GetComponent<AudioSource>();
        UIHealthManager.Instance.UpdateHealth(_health);
        _renderer = GetComponent<Renderer>();
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();

        _rigidBody.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _cameraTransform = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        if (_isMagnetActive)
        {
            TarikKoinDekat();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            _isTeleporting = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        if (_isTeleporting && Input.GetMouseButtonDown(0))
        {
            TeleportPlayer();
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 movementDirection = (forward * vertical + right * horizontal).normalized;

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? _speed * _sprintMultiplier : _speed;

        if (movementDirection.magnitude > 0)
        {
            _rigidBody.linearVelocity = new Vector3(movementDirection.x * currentSpeed, _rigidBody.linearVelocity.y, movementDirection.z * currentSpeed);
        }
        else
        {
            _rigidBody.linearVelocity = new Vector3(0, _rigidBody.linearVelocity.y, 0);
        }

        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        _rotationX -= mouseY;
        _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);

        _cameraTransform.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rigidBody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (isPowerUpActive)
            {
                collision.gameObject.GetComponent<Enemy>().Dead();
                if (enemyDeathAudioSource != null)
                {
                    enemyDeathAudioSource.PlayOneShot(enemyDeath);
                }
            }
            else
            {
                // Debug.Log("Player terkena musuh, health berkurang!");
                reduceHealth(1);
            }
        }
    }

    public void AktifkanMagnet(bool aktif)
    {
        _isMagnetActive = aktif;
    }

    private void TarikKoinDekat()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _magnetRange);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Coin"))
            {
                Vector3 arahTarik = (transform.position - hitCollider.transform.position).normalized;
                hitCollider.transform.position += arahTarik * Time.deltaTime * 5f;
            }
        }
    }

    private void TeleportPlayer()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                transform.position = hit.point + Vector3.up * 1.0f;
                _rigidBody.linearVelocity = Vector3.zero;
            }
        }

        _isTeleporting = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        isPowerUpActive = true;
        if (OnPowerUpStart != null)
        {
            OnPowerUpStart();
        }
        yield return new WaitForSeconds(_powerupDuration);
        isPowerUpActive = false;
        if (OnPowerUpStop != null)
        {
            OnPowerUpStop();
        }
    }
    private void reduceHealth(int jumlah)
    {
        _health -= jumlah;
        UIHealthManager.Instance.UpdateHealth(_health);

        if (_health > 0)
        {
            Respawn();
        }

        if (_health <= 0)
        {
            Dead();
        }
    }

    private void Respawn()
    {
        transform.position = _respawnPoint.position;
        if (respawnAudio != null)
        {
            respawnAudioSource.PlayOneShot(respawnAudio);
            StartCoroutine(BlinkEffect());
        }
    }

    private void Dead()
    {
        
        Backsound.instance.StopMusicOnPlayerDeath();
        StartCoroutine(LoadLoseScene());


    }
    private IEnumerator BlinkEffect()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            _renderer.enabled = false;
            yield return new WaitForSeconds(blinkDuration / (blinkCount * 2));
            _renderer.enabled = true;
            yield return new WaitForSeconds(blinkDuration / (blinkCount * 2));
        }
    }

    private IEnumerator LoadLoseScene()
    {
        yield return new WaitForSeconds(1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("losescene");
    }
}
