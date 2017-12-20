
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
    public class Player : MonoBehaviour
    {

        [SerializeField] float baseDamage = 10f;
        [SerializeField] Weapon currentWeaponConfig;
        [SerializeField] AnimatorOverrideController animOverrideController;

        [Range(.1f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float criticalHitMultiplier = 1.25f;
        [SerializeField] ParticleSystem criticalHitParticleSystem;


        const string ATTACK_ANIM = "Attack";
        const string DEFAULT_ATTACK = "Default attack";

        Animator animator;
        Enemy currentTarget;
        GameObject weaponObject;
        SpecialAbilities abilities;
        
        float lastHitTime = 0f;
        CameraRaycaster cameraRaycaster;

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


        // Use this for initialization
        void Start()
        {
            abilities = GetComponent<SpecialAbilities>();
            RegisterForMouseClick();
            PickUpWeapon(currentWeaponConfig);
            SetAttackAnimation();
            
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
                abilities.AttemptSpecialAbility(0);

            }
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
            var healthPercent = GetComponent<HealthSystem>().HealthAsPercentage;
            if (healthPercent > Mathf.Epsilon)
            {
                ScanForAbilityKeyDown();
            }
        }

        private void ScanForAbilityKeyDown()
        {

            for (int keyIndex = 1; keyIndex < abilities.GetNumberOfAbilities(); keyIndex++)
            {
                if (Input.GetKeyDown(keyIndex.ToString()))
                {

                    abilities.AttemptSpecialAbility(keyIndex);
                }
            }

        }
    }
}