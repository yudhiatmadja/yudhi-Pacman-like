using UnityEngine;

public class CoinRotation : MonoBehaviour
{
    [SerializeField] private float speed = 100f;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
    }

    private void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0); 
    }
}
