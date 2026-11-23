using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Corpanimente.Rules.Cards;
using Corpanimente.Rules.Characters;
using UnityEngine;

namespace Corpanimente.Content
{
    [CreateAssetMenu(fileName = "NewCard", menuName = "Corpanimente/Card Data")]
    public class CardData : ScriptableObject
    {
        public const string IdPattern = "^card_[a-z0-9]+(_[a-z0-9]+)*$";

        [SerializeField] private string id;
        [SerializeField] private string displayName;
        [SerializeField, TextArea(2, 5)] private string description;
        [SerializeField] private Sprite illustration;
        [SerializeField] private CardCategory category;
        [SerializeField] private List<CharacterType> eligibleTypes = new List<CharacterType>();

        public string GetId() => id;
        public string GetDisplayName() => displayName;
        public string GetDescription() => description;
        public Sprite GetIllustration() => illustration;
        public CardCategory GetCategory() => category;
        public IReadOnlyList<CharacterType> GetEligibleTypes() => eligibleTypes;

        public bool CanBeUsedBy(CharacterType type) => eligibleTypes.Contains(type);

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                Debug.LogWarning($"{name}: card id is empty.", this);
            }
            else if (!Regex.IsMatch(id, IdPattern))
            {
                Debug.LogWarning($"{name}: card id '{id}' does not match {IdPattern}.", this);
            }

            if (eligibleTypes.Count == 0)
            {
                Debug.LogWarning($"{name}: no eligible character type.", this);
            }
            else if (eligibleTypes.Distinct().Count() != eligibleTypes.Count)
            {
                Debug.LogWarning($"{name}: eligible types contain duplicates.", this);
            }
        }
    }
}