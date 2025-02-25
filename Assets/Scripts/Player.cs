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
    
    private float _jumpForce = 5f; // Kekuatan lompatan
    private bool _isGrounded; // Cek apakah pemain menyentuh tanah

    private bool _isTeleporting = false; // Mode teleportasi

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();

        _rigidBody.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) 
        {
            _isTeleporting = true; // Aktifkan mode teleportasi
            Cursor.lockState = CursorLockMode.None; // Bebaskan kursor untuk klik
            Cursor.visible = true;
            return;
        }

        if (_isTeleporting && Input.GetMouseButtonDown(0)) 
        {
            TeleportPlayer(); // Jalankan fungsi teleportasi
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
    }

    private void TeleportPlayer()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Ground")) // Pastikan teleport ke tempat valid
            {
                transform.position = hit.point + Vector3.up * 1.0f; // Tambah sedikit tinggi agar tidak menempel ke tanah
                _rigidBody.linearVelocity = Vector3.zero; // Hentikan kecepatan setelah teleportasi
            }
        }

        _isTeleporting = false; // Matikan mode teleportasi
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
