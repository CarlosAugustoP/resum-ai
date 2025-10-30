namespace Resumai.DTOs
{
    public class Language
    {
        public const string English = "English";
        public const string PortugueseBR = "Portuguese - BR";
        public const string Spanish = "Spanish";
        public const string PortuguesePT = "Portuguese - PT";

        public static bool IsValidLanguage(string language)
        {
            return language == English ||
                   language == PortugueseBR ||
                   language == Spanish ||
                   language == PortuguesePT;
        }
    }
}