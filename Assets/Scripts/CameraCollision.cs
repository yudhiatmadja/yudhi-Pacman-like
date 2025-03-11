using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Transform player;  // Referensi ke objek pemain
    public float minDistance = 1f;  // Jarak minimum kamera ke pemain
    public float maxDistance = 4f;  // Jarak maksimum kamera ke pemain
    public float smoothSpeed = 10f; // Kecepatan smoothing pergerakan kamera
    public LayerMask collisionLayer; // Layer yang bisa bertabrakan dengan kamera

    private Vector3 dollyDirection; // Posisi awal relatif terhadap pemain
    private float currentDistance;  // Jarak kamera dari pemain saat ini

    private void Start()
    {
        dollyDirection = transform.localPosition.normalized; 
        currentDistance = maxDistance;
    }

    private void LateUpdate()
    {
        Vector3 desiredCameraPosition = player.position + player.TransformDirection(dollyDirection * maxDistance);
        RaycastHit hit;

        // **Raycast dari pemain ke kamera untuk mendeteksi tabrakan**
        if (Physics.Raycast(player.position, (desiredCameraPosition - player.position).normalized, out hit, maxDistance, collisionLayer))
        {
            currentDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            currentDistance = maxDistance;
        }

        
        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDirection * currentDistance, Time.deltaTime * smoothSpeed);
    }
}
