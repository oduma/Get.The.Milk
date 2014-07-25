namespace GetTheMilk.Utils
{
    public static class GenericInteractionRulesKeys
    {
        /// <summary>
        /// Interactions that apply to all the Characters, 
        /// things that any character can do to any other character
        /// </summary>
        public static readonly string All = "All";

        /// <summary>
        /// Interactions that apply to All Characters in relation to the specific object or character,
        /// things that any character can INITIATE and do to this object or character
        /// </summary>
        public static readonly string AnyCharacter = "AnyCharacter";

        /// <summary>
        /// Interactions that apply to All Characters in relation to the specific object or character,
        /// things that any character can do in RESPONSE to specific actions of this object or character
        /// </summary>
        public static readonly string AnyCharacterResponses = "AnyCharacterResponses";

        public static readonly string CharacterSpecific = "CharacterSpecific";
        public static readonly string PlayerResponses = "PlayerResponses";


    }
}
