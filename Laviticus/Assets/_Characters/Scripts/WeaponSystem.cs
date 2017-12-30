using UnityEngine;
using UnityEngine.Assertions;

namespace RPG.Character
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] WeaponConfig currentWeaponConfig;
        [SerializeField] float baseDamage = 10f;
        [Range(.1f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float criticalHitMultiplier = 1.25f;
        [SerializeField] ParticleSystem criticalHitParticleSystem;

        const string ATTACK_ANIM = "Attack";
        const string DEFAULT_ATTACK = "Default attack";

        float lastHitTime;

        GameObject target;
        GameObject weaponObject;
        Animator animator;
        Character character;

        public WeaponConfig GetCurrentWeapon()
        {
            return currentWeaponConfig;
        }
        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            character = GetComponent<Character>();
            PickUpWeapon(currentWeaponConfig);
            SetAttackAnimation();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PickUpWeapon(WeaponConfig weaponToUse)
        {
            currentWeaponConfig = weaponToUse;
            var weaponPrefab = weaponToUse.GetWeaponPrefab();
            GameObject weaponSocket = RequestDominantHand();
            Destroy(weaponObject);

            weaponObject = Instantiate(weaponPrefab, weaponSocket.transform); //TODO Move to hand

            weaponObject.transform.localPosition = currentWeaponConfig.gripTransform.localPosition;
            weaponObject.transform.localRotation = currentWeaponConfig.gripTransform.localRotation;
        }

        void SetAttackAnimation()
        {
            var animOverrideController = character.GetAnimatorOverride();
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

        public void AttackTarget(GameObject targetToAttack)
        {
            target = targetToAttack;
           // AttackTarget(target);
            //todo use repeat attack coroutine
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
            return baseDamage + currentWeaponConfig.GetAdditionalDamage();

            //bool isCriticalHit = UnityEngine.Random.Range(0f, 1f) <= criticalHitChance;
            //float damageBeforeCritical = baseDamage + currentWeaponConfig.GetAdditionalDamage();
            //if (isCriticalHit)
            //{
            //    criticalHitParticleSystem.Play();
            //    return damageBeforeCritical * criticalHitMultiplier;
            //}
            //else
            //{
            //    return damageBeforeCritical;
            //}
        }
    }
}