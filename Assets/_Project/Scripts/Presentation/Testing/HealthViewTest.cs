using System.Collections;
using Corpanimente.Presentation.Battle;
using Corpanimente.Rules.Characters;
using UnityEngine;

namespace Corpanimente.Presentation.Testing
{
    public sealed class HealthViewTest : MonoBehaviour
    {
        [SerializeField] private HealthView healthView;

        private IEnumerator Start()
        {
            if (healthView == null)
            {
                Debug.LogError("HealthView is not assigned.");
                yield break;
            }

            Health health = new Health(100);

            healthView.Bind(health);
            Debug.Log("Initial Health: 100/100");

            yield return new WaitForSeconds(3f);

            health.TakeDamage(30);
            Debug.Log("Damage: 70/100");

            yield return new WaitForSeconds(3f);

            health.Heal(20);
            Debug.Log("Healing: 90/100");

            yield return new WaitForSeconds(3f);

            health.TakeDamage(100);
            Debug.Log("Death: 0/100");
        }
    }
}