using System.Linq;
using System.Text.RegularExpressions;
using Corpanimente.Content;
using NUnit.Framework;
using UnityEditor;

namespace Corpanimente.Tests.Data
{
    public class CardDataTests
    {
        private const string DataFolder = "Assets/_Project/Data";

        private static CardData[] LoadAllCards()
        {
            return AssetDatabase.FindAssets("t:CardData", new[] { DataFolder })
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Select(path => AssetDatabase.LoadAssetAtPath<CardData>(path))
                .ToArray();
        }

        [Test]
        public void AllCards_InDataFolder_AtLeastOneFound()
        {
            Assert.IsNotEmpty(LoadAllCards(), $"No CardData asset found under {DataFolder}.");
        }

        [Test]
        public void AllCards_HaveUniqueIds()
        {
            var duplicates = LoadAllCards()
                .GroupBy(card => card.GetId())
                .Where(group => group.Count() > 1)
                .Select(group => $"{group.Key}: {string.Join(", ", group.Select(card => card.name))}")
                .ToList();

            Assert.IsEmpty(duplicates, "Duplicate card ids:\n" + string.Join("\n", duplicates));
        }

        [Test]
        public void AllCards_HaveValidIds()
        {
            var invalid = LoadAllCards()
                .Where(card => string.IsNullOrEmpty(card.GetId())
                            || !Regex.IsMatch(card.GetId(), CardData.IdPattern))
                .Select(card => card.name)
                .ToList();

            Assert.IsEmpty(invalid, "Cards with invalid ids:\n" + string.Join("\n", invalid));
        }
    }
}