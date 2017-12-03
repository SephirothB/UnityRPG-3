using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Character
{
    [RequireComponent(typeof(Image))]
    public class PlayerHealthBar : MonoBehaviour
    {
        Player player;
        Image healthOrb;

       
        void Start()
        {
            player = FindObjectOfType<Player>();
            healthOrb = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            healthOrb.fillAmount = player.HealthAsPercentage;

        }


    }
}