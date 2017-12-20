
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Character
{

    public class SpecialAbilities : MonoBehaviour
    {
        [SerializeField] SpecialAbilityConfig[] abilities;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] Image energyImage;
        [SerializeField] float energyRegenPerSecond = 1f;


        float currentEnergyPoints;

        AudioSource audioSource;

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

        public void UpdateEnergyBar()
        {
            energyImage.fillAmount = EnergyAsPercentage;
        }

        void RegenEnergy()
        {
            var energyRegen = energyRegenPerSecond * Time.deltaTime;

            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + energyRegen, 0, maxEnergyPoints);
        }
        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            currentEnergyPoints = maxEnergyPoints;
            AttachInitialAbilities();
            UpdateEnergyBar();

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

        public int GetNumberOfAbilities()
        {
            return abilities.Length;
        }
        void AttachInitialAbilities()
        {
            for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
            {
                abilities[abilityIndex].AttachAbility(gameObject);
            }

        }
        public void AttemptSpecialAbility(int abilityIndex)
        {


            var energyCost = abilities[abilityIndex].GetEnergyCost();
            if (energyCost <= currentEnergyPoints)
            {
                ConsumeEnergy(energyCost);

            }
            else
            {
                //todo play out of energy sound
            }
        }
    }
}