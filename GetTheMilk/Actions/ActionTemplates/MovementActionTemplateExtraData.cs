using System.Collections.Generic;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Common;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Settings;
using System.Linq;
using GetTheMilk.Navigation;

namespace GetTheMilk.Actions.ActionTemplates
{
    public class MovementActionTemplateExtraData
    {
        public IEnumerable<NonCharacterObject> ObjectsBlocking { get; set; }

        public IEnumerable<Character> CharactersBlocking { get; set; }

        public IEnumerable<Character> CharactersInRange { get; set; }

        public IEnumerable<NonCharacterObject> ObjectsInRange { get; set; }

        public IEnumerable<NonCharacterObject> ObjectsInCell { get; set; }

        public List<BaseActionTemplate> AvailableActionTemplates { get; set; }

        public Cell MovedToCell { get; set; }

        public string GetPositioningInformation()
        {
            var gameSettings = GameSettings.GetInstance();
            var objectsReachable = string.Empty;
            var objectsInCell = (ObjectsInCell) ?? new NonCharacterObject[0];
            var objectsInRange = (ObjectsInRange) ?? new NonCharacterObject[0];
            var allObjectsReachable = objectsInCell.Union(objectsInRange).ToList();
            if (allObjectsReachable.Any())
                objectsReachable = string.Join("\r\n",
                            allObjectsReachable.OrderBy(o => o.CellNumber).Select(
                                o =>
                                string.Format(gameSettings.MessageForObjectsInRange,
                                              MovedToCell.GetDirectionToCell(o.CellNumber),
                                              o.Name.Description)));

            var charactersInRange = string.Empty;
            if (CharactersInRange != null && CharactersInRange.Any())
                charactersInRange = string.Join("\r\n",
                        CharactersInRange.OrderBy(o => o.CellNumber).Select(
                            o =>
                            string.Format(gameSettings.MessageForObjectsInRange,
                                          MovedToCell.GetDirectionToCell(o.CellNumber),
                                          o.Name.Description)));
            return string.Format("{0}{1}", objectsReachable, charactersInRange).Trim();

        }

        internal string GetBlockers()
        {
            var objectsBlocking = FormatList(ObjectsBlocking).Trim();
            var charactersBlocking = FormatList(CharactersBlocking).Trim();

            var result = (!string.IsNullOrEmpty(objectsBlocking)&&!string.IsNullOrEmpty(charactersBlocking))?(objectsBlocking+", "+charactersBlocking): objectsBlocking+charactersBlocking;
            var andReplacementPosition = result.LastIndexOf(",");
            if (((ObjectsBlocking==null)?0:ObjectsBlocking.Count()) 
                + ((CharactersBlocking==null)?0:CharactersBlocking.Count()) > 1)
                result = string.Format("{0} and {1}", result.Substring(0, andReplacementPosition),
                                   result.Substring(andReplacementPosition + 2, result.Length - andReplacementPosition - 2));
            return result + ".";
        }

        private string FormatList(IEnumerable<IPositionable> objects)
        {
            if (objects == null || !objects.Any())
                return "";
            string result = objects.Aggregate(string.Empty,
                                                      (current, currentObject) =>
                                                      string.Format("{0}, {1}", current, currentObject.Name.Narrator));
            if (result.StartsWith(","))
                result = result.Substring(2, result.Length - 2);
            return result;
        }

    }
}
