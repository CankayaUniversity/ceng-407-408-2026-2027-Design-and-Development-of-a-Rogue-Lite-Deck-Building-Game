using System.Linq;
using Corpanimente.Rules.Cards;
using NUnit.Framework;

namespace Corpanimente.Tests.Architecture
{
    public class ArchitectureTests
    {
        [Test]
        public void RulesAssembly_DoesNotReferenceUnity()
        {
            var unityReferences = typeof(CardCategory).Assembly
                .GetReferencedAssemblies()
                .Select(reference => reference.Name)
                .Where(name => name.StartsWith("UnityEngine") || name.StartsWith("UnityEditor"))
                .ToList();

            Assert.IsEmpty(unityReferences,
                "Corpanimente.Rules must not reference Unity: " + string.Join(", ", unityReferences));
        }
    }
}