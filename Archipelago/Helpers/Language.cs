namespace Archipelago.Helpers {
    internal static class Language {
        internal static string Get(string category, string id, string[] wildcards = null) {
            var str = LanguageMgr.GetStringById($"{category}.{id}").Split(new[] { '*' });
            var placeholders = str.Length - 1;
            var nWild = (wildcards?.Length) ?? 0;

            if (
                (wildcards == null && placeholders != 0) ||
                nWild != placeholders
            ) {
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
