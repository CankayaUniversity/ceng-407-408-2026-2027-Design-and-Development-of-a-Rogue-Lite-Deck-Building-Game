using System;
using Corpanimente.Rules.Characters;
using NUnit.Framework;

namespace Corpanimente.Tests.Characters
{
    public class HealthTests
    {
        [Test]
        public void Constructor_ValidMaximum_StartsFullAndAlive()
        {
            var health = new Health(10);

            Assert.AreEqual(10, health.Maximum);
            Assert.AreEqual(10, health.Current);
            Assert.IsTrue(health.IsAlive);
        }

        [TestCase(0)]
        [TestCase(-5)]
        public void Constructor_NonPositiveMaximum_Throws(int maximum)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Health(maximum));
        }

        [Test]
        public void TakeDamage_LessThanCurrent_ReducesCurrent()
        {
            var health = new Health(10);

            health.TakeDamage(3);

            Assert.AreEqual(7, health.Current);
        }

        [Test]
        public void TakeDamage_MoreThanCurrent_StopsAtZero()
        {
            var health = new Health(10);

            health.TakeDamage(25);

            Assert.AreEqual(0, health.Current);
            Assert.IsFalse(health.IsAlive);
        }

        [Test]
        public void TakeDamage_AfterDeath_RaisesDiedOnlyOnce()
        {
            var health = new Health(10);
            int diedCount = 0;
            health.Died += _ => diedCount++;

            health.TakeDamage(10);
            health.TakeDamage(5);

            Assert.AreEqual(1, diedCount);
        }

        [Test]
        public void TakeDamage_Zero_DoesNotRaiseChanged()
        {
            var health = new Health(10);
            int changedCount = 0;
            health.Changed += _ => changedCount++;

            health.TakeDamage(0);

            Assert.AreEqual(0, changedCount);
        }

        [Test]
        public void TakeDamage_Negative_Throws()
        {
            var health = new Health(10);

            Assert.Throws<ArgumentOutOfRangeException>(() => health.TakeDamage(-1));
        }

        [Test]
        public void Heal_BelowMaximum_IncreasesCurrent()
        {
            var health = new Health(10);
            health.TakeDamage(5);

            health.Heal(3);

            Assert.AreEqual(8, health.Current);
        }

        [Test]
        public void Heal_MoreThanMissing_StopsAtMaximum()
        {
            var health = new Health(10);
            health.TakeDamage(5);

            health.Heal(20);

            Assert.AreEqual(10, health.Current);
        }

        [Test]
        public void Heal_VeryLargeAmount_DoesNotOverflow()
        {
            var health = new Health(10);
            health.TakeDamage(5);

            health.Heal(int.MaxValue);

            Assert.AreEqual(10, health.Current);
        }

        [Test]
        public void Heal_AtMaximum_DoesNotRaiseChanged()
        {
            var health = new Health(10);
            int changedCount = 0;
            health.Changed += _ => changedCount++;

            health.Heal(5);

            Assert.AreEqual(0, changedCount);
        }

        [Test]
        public void Heal_WhenDead_DoesNotRevive()
        {
            var health = new Health(10);
            health.TakeDamage(10);

            health.Heal(5);

            Assert.AreEqual(0, health.Current);
            Assert.IsFalse(health.IsAlive);
        }

        [Test]
        public void Heal_Negative_Throws()
        {
            var health = new Health(10);

            Assert.Throws<ArgumentOutOfRangeException>(() => health.Heal(-1));
        }

        [Test]
        public void Changed_WhenRaised_ListenerSeesNewValue()
        {
            var health = new Health(10);
            int seenCurrent = -1;
            health.Changed += h => seenCurrent = h.Current;

            health.TakeDamage(4);

            Assert.AreEqual(6, seenCurrent);
        }
    }
}