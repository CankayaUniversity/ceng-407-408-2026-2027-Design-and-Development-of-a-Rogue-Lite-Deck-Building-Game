using System;
using Corpanimente.Rules.Characters;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Corpanimente.Presentation.Battle
{
    public sealed class HealthView : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private TMP_Text healthText;

        private Health health;

        public void Bind(Health newHealth)
        {
            if (newHealth == null)
            {
                throw new ArgumentNullException(nameof(newHealth));
            }

            Unbind();

            health = newHealth;
            health.Changed += Refresh;

            Refresh(health);
        }

        private void Refresh(Health currentHealth)
        {
            healthSlider.minValue = 0;
            healthSlider.maxValue = currentHealth.Maximum;

            healthSlider.SetValueWithoutNotify(
                currentHealth.Current
            );

            if (healthText != null)
            {
                healthText.text =
                    $"{currentHealth.Current}/{currentHealth.Maximum}";
            }
        }

        private void Unbind()
        {
            if (health != null)
            {
                health.Changed -= Refresh;
                health = null;
            }
        }

        private void OnDestroy()
        {
            Unbind();
        }
    }
}
