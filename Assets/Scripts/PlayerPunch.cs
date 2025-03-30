using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    [SerializeField] private Transform rightPunchPoint;
    [SerializeField] private Transform leftPunchPoint;
    [SerializeField] private float punchRange = 0.5f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int punchDamage = 1;
    public bool IsPunching => isPunching;

    private Animator animator;
    private bool isPunching = false; // Tambahkan variabel untuk cek apakah sedang memukul

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isPunching) // Hanya bisa memukul jika tidak sedang memukul
        {
            Punch();
        }
    }

    private void Punch()
    {
        isPunching = true; // Set isPunching agar tidak bisa spam pukulan
        animator.SetBool("IsPunching", true); // Aktifkan animasi pukulan

        Collider[] hitEnemiesRight = Physics.OverlapSphere(rightPunchPoint.position, punchRange, enemyLayer);
        Collider[] hitEnemiesLeft = Physics.OverlapSphere(leftPunchPoint.position, punchRange, enemyLayer);

        foreach (Collider enemy in hitEnemiesRight)
        {
            Debug.Log("Pukulan kanan kena " + enemy.name);
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(punchDamage); // Kurangi nyawa musuh
            }
        }

        foreach (Collider enemy in hitEnemiesLeft)
        {
            Debug.Log("Pukulan kiri kena " + enemy.name);
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(punchDamage);
            }
        }

        // Reset pukulan setelah animasi selesai
        Invoke(nameof(ResetPunch), 0.5f); // Ubah 0.5f sesuai dengan durasi animasi pukulan
    }

    private void ResetPunch()
    {
        isPunching = false; // Reset status agar bisa memukul lagi
        animator.SetBool("IsPunching", false); // Kembali ke Idle atau Run
    }

    private void OnDrawGizmosSelected()
    {
        if (rightPunchPoint == null || leftPunchPoint == null) return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(rightPunchPoint.position, punchRange);
        Gizmos.DrawWireSphere(leftPunchPoint.position, punchRange);
    }

    
}
