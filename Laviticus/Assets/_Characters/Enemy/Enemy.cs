using UnityEngine;

namespace RPG.Character
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] float damagePerShot = 1f;
        [SerializeField] float attackRadius = 4f;
        [SerializeField] float firingPeriodSeconds = 0.5f;
        [SerializeField] float firingPeriodVariation = 0.1f;
        [SerializeField] float moveToAttackRadius = 6f;
        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;
        [SerializeField] AnimatorOverrideController animOverrideController;
        [SerializeField] Vector3 verticalAimOffset = new Vector3(0, 1f, 0);

        const string ATACK_ANIM = "Attack";

        bool isAttacking = false;

        AudioSource audioPlayer;
        Animator animator;
        PlayerControl player = null;
        GameObject fireFrom;

        void Start()
        {
            player = FindObjectOfType<PlayerControl>();

            audioPlayer = GetComponent<AudioSource>();
        }

        void Update()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceToPlayer <= moveToAttackRadius)
            {
                // aiCharacterControl.SetTarget(player.transform);
            }
            else
            {
                // aiCharacterControl.SetTarget(transform);
            }
            if (distanceToPlayer <= attackRadius && !isAttacking)
            {
                isAttacking = true;
                float randomisedDelay = Random.Range(firingPeriodSeconds - firingPeriodVariation, firingPeriodSeconds + firingPeriodVariation);
                InvokeRepeating("FireProjectile", 0f, randomisedDelay); //TODO switch to coroutines
            }
            if (distanceToPlayer > attackRadius)
            {
                isAttacking = false;
                CancelInvoke();
            }
        }

        void FireProjectile()
        {
            GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.SetDamage(damagePerShot);
            projectileComponent.SetFirer(gameObject);

            float projectileSpeed = projectileComponent.GetDefaultLaunchSpeed();
            Vector3 unitVectorToPlayer = (player.transform.position + verticalAimOffset - projectileSocket.transform.position).normalized;
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;

            //Destroy(newProjectile, 0.8f);

        }
    }
}