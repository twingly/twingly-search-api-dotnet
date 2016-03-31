using System.Runtime.Serialization;

namespace Twingly.Search.Client.Domain
{
    /// <summary>
    /// Enumerates all the languages supported by Twingly. 
    /// </summary>
    /// <see cref="https://developer.twingly.com/resources/search-language/#supported-languages"/>
    public enum Language
    {
        /// <summary>
        /// Afrikaans language.
        /// </summary>
        [EnumMember(Value = "af")]
        Afrikaans,
        /// <summary>
        /// Arabic language.
        /// </summary>
        [EnumMember(Value = "ar")]
        Arabic,
        /// <summary>
        /// Bulgarian language.
        /// </summary>
        [EnumMember(Value = "bg")]
        Bulgarian,
        /// <summary>
        /// Bengali language.
        /// </summary>
        [EnumMember(Value = "bn")]
        Bengali,
        /// <summary>
        /// Catalan language.
        /// </summary>
        [EnumMember(Value = "ca")]
        Catalan,
        /// <summary>
        /// Czech language.
        /// </summary>
        [EnumMember(Value = "cs")]
        Czech,
        /// <summary>
        /// Welsh language.
        /// </summary>
        [EnumMember(Value = "cy")]
        Welsh,
        /// <summary>
        /// Danish language.
        /// </summary>
        [EnumMember(Value = "da")]
        Danish,
        /// <summary>
        /// German language.
        /// </summary>
        [EnumMember(Value = "de")]
        German,
        /// <summary>
        /// Greek language.
        /// </summary>
        [EnumMember(Value = "el")]
        Greek,
        /// <summary>
        /// English language.
        /// </summary>
        [EnumMember(Value = "en")]
        English,
        /// <summary>
        /// Spanish language.
        /// </summary>
        [EnumMember(Value = "es")]
        Spanish,
        /// <summary>
        /// Estonian language.
        /// </summary>
        [EnumMember(Value = "et")]
        Estonian,
        /// <summary>
        /// Persian language.
        /// </summary>
        [EnumMember(Value = "fa")]
        Persian,
        /// <summary>
        /// Finnish language.
        /// </summary>
        [EnumMember(Value = "fi")]
        Finnish,
        /// <summary>
        /// French language.
        /// </summary>
        [EnumMember(Value = "fr")]
        French,
        /// <summary>
        /// Gujarati language.
        /// </summary>
        [EnumMember(Value = "gu")]
        Gujarati,
        /// <summary>
        /// Hebrew language.
        /// </summary>
        [EnumMember(Value = "he")]
        Hebrew,
        /// <summary>
        /// Hindi language.
        /// </summary>
        [EnumMember(Value = "hi")]
        Hindi,
        /// <summary>
        /// Croatian language.
        /// </summary>
        [EnumMember(Value = "hr")]
        Croatian,
        /// <summary>
        /// Hungarian language.
        /// </summary>
        [EnumMember(Value = "hu")]
        Hungarian,
        /// <summary>
        /// Indonesian language.
        /// </summary>
        [EnumMember(Value = "id")]
        Indonesian,
        /// <summary>
        /// Icelandic language.
        /// </summary>
        [EnumMember(Value = "is")]
        Icelandic,
        /// <summary>
        /// Italian language.
        /// </summary>
        [EnumMember(Value = "it")]
        Italian,
        /// <summary>
        /// Japanese language.
        /// </summary>
        [EnumMember(Value = "ja")]
        Japanese,
        /// <summary>
        /// Georgian language.
        /// </summary>
        [EnumMember(Value = "ka")]
        Georgian,
        /// <summary>
        /// Kannada language.
        /// </summary>
        [EnumMember(Value = "kn")]
        Kannada,
        /// <summary>
        /// Korean language.
        /// </summary>
        [EnumMember(Value = "ko")]
        Korean,
        /// <summary>
        /// Lithuanian language.
        /// </summary>
        [EnumMember(Value = "lt")]
        Lithuanian,
        /// <summary>
        /// Macedonian language.
        /// </summary>
        [EnumMember(Value = "mk")]
        Macedonian,
        /// <summary>
        /// Malayalam language.
        /// </summary>
        [EnumMember(Value = "ml")]
        Malayalam,
        /// <summary>
        /// Marathi language.
        /// </summary>
        [EnumMember(Value = "mr")]
        Marathi,
        /// <summary>
        /// Nepali language.
        /// </summary>
        [EnumMember(Value = "ne")]
        Nepali,
        /// <summary>
        /// Dutch language.
        /// </summary>
        [EnumMember(Value = "nl")]
        Dutch,
        /// <summary>
        /// Norwegian language.
        /// </summary>
        [EnumMember(Value = "no")]
        Norwegian,
        /// <summary>
        /// Punjabi language.
        /// </summary>
        [EnumMember(Value = "pa")]
        Punjabi,
        /// <summary>
        /// Polish language.
        /// </summary>
        [EnumMember(Value = "pl")]
        Polish,
        /// <summary>
        /// Portuguese language.
        /// </summary>
        [EnumMember(Value = "pt")]
        Portuguese,
        /// <summary>
        /// Romanian language.
        /// </summary>
        [EnumMember(Value = "ro")]
        Romanian,
        /// <summary>
        /// Russian language.
        /// </summary>
        [EnumMember(Value = "ru")]
        Russian,
        /// <summary>
        /// Slovak language.
        /// </summary>
        [EnumMember(Value = "sk")]
        Slovak,
        /// <summary>
        /// Slovenian language.
        /// </summary>
        [EnumMember(Value = "sl")]
        Slovenian,
        /// <summary>
        /// Somali language.
        /// </summary>
        [EnumMember(Value = "so")]
        Somali,
        /// <summary>
        /// Albanian language.
        /// </summary>
        [EnumMember(Value = "sq")]
        Albanian,
        /// <summary>
        /// Serbian language.
        /// </summary>
        [EnumMember(Value = "sr")]
        Serbian,
        /// <summary>
        /// Swedish language.
        /// </summary>
        [EnumMember(Value = "sv")]
        Swedish,
        /// <summary>
        /// Swahili language.
        /// </summary>
        [EnumMember(Value = "sw")]
        Swahili,
        /// <summary>
        /// Tamil language.
        /// </summary>
        [EnumMember(Value = "ta")]
        Tamil,
        /// <summary>
        /// Telugu language.
        /// </summary>
        [EnumMember(Value = "te")]
        Telugu,
        /// <summary>
        /// Thai language.
        /// </summary>
        [EnumMember(Value = "th")]
        Thai,
        /// <summary>
        /// Tagalog language.
        /// </summary>
        [EnumMember(Value = "tl")]
        Tagalog,
        /// <summary>
        /// Turkish language.
        /// </summary>
        [EnumMember(Value = "tr")]
        Turkish,
        /// <summary>
        /// Ukrainian language.
        /// </summary>
        [EnumMember(Value = "uk")]
        Ukrainian,
        /// <summary>
        /// Urdu language.
        /// </summary>
        [EnumMember(Value = "ur")]
        Urdu,
        /// <summary>
        /// Vietnamese language.
        /// </summary>
        [EnumMember(Value = "vi")]
        Vietnamese,
        /// <summary>
        /// Chinese language.
        /// </summary>
        [EnumMember(Value = "zh")]
        Chinese,
    }
}
