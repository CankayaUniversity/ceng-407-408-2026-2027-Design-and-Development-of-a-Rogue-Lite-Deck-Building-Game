using System;

namespace Corpanimente.Rules.Characters
{
    /// <summary>A health bar owned by one character or one enemy.</summary>
    public sealed class Health
    {
        public int Maximum { get; }
        public int Current { get; private set; }
        public bool IsAlive => Current > 0;

        public event Action<Health> Changed;
        public event Action<Health> Died;

        public Health(int maximum)
        {
            if (maximum <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maximum), "Maximum health must be positive.");
            }

            Maximum = maximum;
            Current = maximum;
        }

        public void TakeDamage(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Damage cannot be negative.");
            }

            if (!IsAlive)
            {
                return;
            }

            int newCurrent = Math.Max(0, Current - amount);
            if (newCurrent == Current)
            {
                return;
            }

            Current = newCurrent;
            Changed?.Invoke(this);

            if (!IsAlive)
            {
                Died?.Invoke(this);
            }
        }

        public void Heal(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Healing cannot be negative.");
            }

            if (!IsAlive)
            {
                return;
            }

            int missing = Maximum - Current;
            int newCurrent = Current + Math.Min(amount, missing);
            if (newCurrent == Current)
            {
                return;
            }

            Current = newCurrent;
            Changed?.Invoke(this);
        }
    }
}