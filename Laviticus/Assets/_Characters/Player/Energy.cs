using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO Consider rewiring
using RPG.CameraUI;


namespace RPG.Character
{
    //[RequireComponent(typeof(RawImage))]
    public class Energy : MonoBehaviour
    {
        [SerializeField] const int walkableLayerNumber = 8;
        [SerializeField] const int enemyLayerNumber = 9;
        [SerializeField] const int stiffLayerNumber = 10;

        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float energyPerHit = 10f;

        [SerializeField] RawImage energyBarRawImage;
        Player player;

        CameraRaycaster cameraRaycaster;

        float lastHitTime = 0f;
        float currentEnergyPoints;

        public float EnergyAsPercentage
        {
            get
            {
                return currentEnergyPoints / (float)maxEnergyPoints;
            }
        }

        // Use this for initialization
        void Start()
        {
            RegisterForRightMouseClick();
            currentEnergyPoints = maxEnergyPoints;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void RegisterForRightMouseClick()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.NotifyRightMouseClickObservers += OnRightMouseClick;
        }

        void OnRightMouseClick(RaycastHit raycastHit, int layerHit)
        {

            if (layerHit == enemyLayerNumber)
            {

                var enemy = raycastHit.collider.gameObject;
                //currentTarget = enemy;

                //if (IsEnemyInRange(enemy))
                //{
                EnergyAttackTarget(enemy);
                UpdateEnergyBar();
                //}



            }

        }

        private void EnergyAttackTarget(GameObject target)
        {

            //Component damagableComponent = target.gameObject.GetComponent(typeof(IDamagable));
            var enemyComponent = target.GetComponent<Enemy>();
            float newEnergyPoints = currentEnergyPoints - energyPerHit;

            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);

            //if (Time.time - lastHitTime)
            //{
            //    /animator.SetTrigger("Attack");
            //    enemyComponent.TakeDamage(damageCaused);
            //    lastHitTime = Time.time;
            //}

        }




        private void UpdateEnergyBar()
        {
            float xValue = -(EnergyAsPercentage / 2f) - 0.5f;
            energyBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }
    }
}