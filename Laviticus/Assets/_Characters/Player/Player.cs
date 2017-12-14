
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

//TODO Consider rewiring
using RPG.CameraUI;
using RPG.Core;

using System.Collections;

namespace RPG.Character
{
    [RequireComponent(typeof(AudioSource))]
    public class Player : MonoBehaviour, IDamagable
    {
        [SerializeField] SpecialAbilityConfig[] abilities;
        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;

        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float baseDamage = 10f;

        [SerializeField] Weapon currentWeaponConfig;

        [SerializeField] AnimatorOverrideController animOverrideController;

        
        
        [Range(.1f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float criticalHitMultiplier = 1.25f;
        [SerializeField] ParticleSystem criticalHitParticleSystem;

        const string DEATH_ANIM = "Death";
        const string ATTACK_ANIM = "Attack";
        const string DEFAULT_ATTACK = "Default attack";

        AudioSource audio;
        Animator animator;
        PowerAttackConfig config;
        Enemy currentTarget;
        GameObject weaponObject;
        

        private float currentHealthPoints = 100f;
        float lastHitTime = 0f;
        //GameObject currentTarget;
        CameraRaycaster cameraRaycaster;

        //bool playerisDead;
        //public bool PlayerIsDead
        //{
        //    get
        //    {
        //        return playerisDead;
        //    }
        //}
        public float HealthAsPercentage
        {
            get
            {
                return currentHealthPoints / (float)maxHealthPoints;
            }
        }



        public void TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            audio.clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
            //TODO remove once ready audio.Play();

            if (currentHealthPoints <= 0)
            {

                              
                StartCoroutine(KillPlayer());
                //Play death sound

                //Trigger death animation

                //Reload the scene

                //TODO remove to allow death and implement reload funtionality //if (currentHealthPoints <= 0) { Destroy(gameObject); }
            }
            
        }

        public void AddHealth(float healAmount)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + healAmount, 0f, maxHealthPoints);
        }

        public void PickUpWeapon(Weapon weaponToUse)
        {
            currentWeaponConfig = weaponToUse;
            var weaponPrefab = weaponToUse.GetWeaponPrefab();
            GameObject weaponSocket = RequestDominantHand();
            Destroy(weaponObject);

            weaponObject = Instantiate(weaponPrefab, weaponSocket.transform); //TODO Move to hand

            weaponObject.transform.localPosition = currentWeaponConfig.gripTransform.localPosition;
            weaponObject.transform.localRotation = currentWeaponConfig.gripTransform.localRotation;
        }

        IEnumerator KillPlayer()
        {
            animator.SetTrigger(DEATH_ANIM);

            audio.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
            //TODO remove once ready audio.Play();
            
            yield return new WaitForSecondsRealtime(audio.clip.length);

            SceneManager.LoadScene(0);
            
        }




        // Use this for initialization
        void Start()
        {

            audio = GetComponent<AudioSource>();
            RegisterForMouseClick();
            SetCurrentMaxHealth();
            PickUpWeapon(currentWeaponConfig);
            SetAttackAnimation();
            AttachInitialAbilities();
            
        }

        private void AttachInitialAbilities()
        {
            for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
            {
                abilities[abilityIndex].AttachAbility(gameObject);
            }
            
        }

        void RegisterForMouseClick()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        void OnMouseOverEnemy(Enemy enemy)
        {
            currentTarget = enemy;
            //var enemyHit = enemy.gameObject;

            if (Input.GetMouseButtonDown(0) && IsEnemyInRange(enemy))
            {
                AttackTarget(enemy);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                AttemptSpecialAbility(0);

            }
        }

        private void AttemptSpecialAbility(int abilityIndex)
        {
            
            var energyComponent = GetComponent<Energy>();
            var energyCost = abilities[abilityIndex].GetEnergyCost();
            if (energyComponent.IsEnergyAvailable(energyCost))
            {
                energyComponent.ConsumeEnergy(energyCost);
                var abilityParams = new AbilityUseParams(currentTarget, baseDamage);
                abilities[abilityIndex].Engage(abilityParams);
            }
        }

        void SetCurrentMaxHealth()
        {
            currentHealthPoints = maxHealthPoints;
        }



        void SetAttackAnimation()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animOverrideController;
            animOverrideController[DEFAULT_ATTACK] = currentWeaponConfig.GetAttackAnimClip(); //Remove Constant
        }

       

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();

            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No Dominant Hand found on Player, please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "More than on Dominant Hand scripts on player please remove one");
            return dominantHands[0].gameObject;

        }

        


        private bool IsEnemyInRange(Enemy target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= currentWeaponConfig.GetMaxAttackRange();    
        }


        private void AttackTarget(Enemy target)
        {

           if (Time.time - lastHitTime > currentWeaponConfig.GetMinTimeBetweenHits())
            {
                SetAttackAnimation();
                animator.SetTrigger(ATTACK_ANIM);
                target.TakeDamage(CalculateDamage());
                lastHitTime = Time.time;
            }

        }
        private float CalculateDamage()
        {
            bool isCriticalHit = UnityEngine.Random.Range(0f, 1f) <= criticalHitChance;
            float damageBeforeCritical = baseDamage + currentWeaponConfig.GetAdditionalDamage();
            if (isCriticalHit)
            {
                criticalHitParticleSystem.Play();
                return damageBeforeCritical * criticalHitMultiplier;
            }
            else
            {
                return damageBeforeCritical;
            }
        }


        void Update()
        {
            if (HealthAsPercentage > Mathf.Epsilon)
            {
                ScanForAbilityKeyDown();
            }
        }

        private void ScanForAbilityKeyDown()
        {

            for (int keyIndex = 1; keyIndex < abilities.Length; keyIndex++)
            {
                if (Input.GetKeyDown(keyIndex.ToString()))
                {

                    AttemptSpecialAbility(keyIndex);
                }
            }

        }
    }
}