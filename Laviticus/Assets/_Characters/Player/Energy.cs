
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Character
{
    //[RequireComponent(typeof(RawImage))]
    public class Energy : MonoBehaviour
    {
        [SerializeField] const int walkableLayerNumber = 8;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] RawImage energyBarRawImage;
        [SerializeField] float energyRegenPerSecond = 1f;

        Player player;
        
        float lastHitTime = 0f;
        float currentEnergyPoints;

        public float EnergyAsPercentage
        {
            get
            {
                return currentEnergyPoints / (float)maxEnergyPoints;
            }
        }

        public void ConsumeEnergy(float amount)
        {

            float newEnergyPoints = currentEnergyPoints - amount;
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);
            UpdateEnergyBar();
        }
        
        public bool IsEnergyAvailable(float amount)
        {
            return amount <= currentEnergyPoints;
        }
        public void UpdateEnergyBar()
        {
            float xValue = -(EnergyAsPercentage / 2f) - 0.5f;
            energyBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }

        void RegenEnergy()
        {
            var energyRegen = energyRegenPerSecond * Time.deltaTime;

            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + energyRegen, 0, maxEnergyPoints);
        }
        // Use this for initialization
        void Start()
        {

            currentEnergyPoints = maxEnergyPoints;
        }

        // Update is called once per frame
        void Update()
        {
            if (currentEnergyPoints < maxEnergyPoints)
            {
                RegenEnergy();
                UpdateEnergyBar();
            }
        }
        
    }
}