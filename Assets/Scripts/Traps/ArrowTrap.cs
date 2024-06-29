using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [Header("Timers")] [SerializeField] private float attackCooldown;
    [SerializeField] private float initialDelay;
    [Header("Arrows")] [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] arrows;
    private float cooldownTimer;

    [Header("SFX")] [SerializeField] private AudioClip arrowSound;

    private void Start()
    {
        cooldownTimer = -initialDelay;
    }

    private void Attack()
    {
        cooldownTimer = 0;

        SoundManager.instance.PlaySound(arrowSound);
        arrows[FindArrow()].transform.position = firePoint.position;
        arrows[FindArrow()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private int FindArrow()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)
                return i;
        }

        return 0;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= attackCooldown)
            Attack();
    }
}