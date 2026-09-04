namespace Archipelago.Helpers {
    internal static class Language {
        internal static string Get(string category, string id, params string[] wildcards) {
            var str = LanguageMgr.GetStringById($"{category}.{id}").Split(new[] { '*' });
            var placeholders = str.Length - 1;
            var nWild = wildcards.Length;

            if (nWild != placeholders) {
                Core.Static.Error($"Expected {nWild} wildcards, got {placeholders} for {category}.{id}");
                return null;
            }

            var result = str[0];
            for (var i = 0; i < nWild; i++) {
                result += wildcards[i];
                result += str[i+1];
            }

            return result;
        }
    }
}
