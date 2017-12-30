
using UnityEngine;
using UnityEngine.Assertions;
using RPG.CameraUI;

namespace RPG.Character
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] AnimatorOverrideController animOverrideController;
        
        Character character;
        Enemy currentTarget;
        SpecialAbilities abilities;
        CameraRaycaster cameraRaycaster;
        WeaponSystem weaponSystem;


        void Start()
        {
            character = GetComponent<Character>();
            abilities = GetComponent<SpecialAbilities>();
            weaponSystem = GetComponent<WeaponSystem>();
            RegisterForMouseEvents();
            
        }

        void Update()
        {
            ScanForAbilityKeyDown();
        }

        void RegisterForMouseEvents()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.onMouseOverTerrain += OnMouseOverTerrain;

        }

        void OnMouseOverTerrain(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                character.SetDestination(destination);
            }
        }

        void OnMouseOverEnemy(Enemy enemy)
        {
            currentTarget = enemy;
            if (Input.GetMouseButtonDown(0) && IsEnemyInRange(enemy))
            {
                weaponSystem.AttackTarget(enemy.gameObject);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                abilities.AttemptSpecialAbility(0);

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

        private bool IsEnemyInRange(Enemy target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
        }
    }
}