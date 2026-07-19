namespace Archipelago.Helpers {
    internal static class Language {
        internal static string Get(string category, string id, string[] wildcards = null) {
            var str = LanguageMgr.GetStringById($"{category}.{id}").Split(new[] { '*' });
            var placeholders = str.Length - 1;

            if (
                (wildcards == null && str.Length != 0) ||
                wildcards.Length != placeholders
            ) {
                Core.Static.Error($"Expected {wildcards.Length} wildcards, got {placeholders} for {category}.{id}");
                return null;
            }

            var result = str[0];
            for (var i = 0; i < wildcards.Length; i++) {
                result += wildcards[i];
                result += str[i+1];
            }

            return result;
        }
    }
}
