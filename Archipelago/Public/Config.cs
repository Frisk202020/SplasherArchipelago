using SplasherArchipelago.Public.Field;
using System.IO;
using System.Linq;

namespace SplasherArchipelago.Public {
    public class Config {
        #region ConnectionInfo
        public readonly StringField Address = new StringField();
        public readonly UintField Port = new UintField();
        public readonly StringField Slot = new StringField();
        public readonly BoolField Proxy = new BoolField();
        #endregion

        #region QoL
        public readonly FloatField CutsceneSpeed = new FloatField();
        #endregion

        private Config() { }

        #region Parse Implementation
        public static Config Parse() {
            try {
                var config = new Config();
                var sr = new StreamReader("connection.yaml");
                var line = sr.ReadLine();

                while (line != null) {
                    config.ParseLine(line);
                    line = sr.ReadLine();
                }

                var missing = typeof(Config)
                    .GetFields()
                    .Where(f => typeof(IField).IsAssignableFrom(f.FieldType) && !((IField)f.GetValue(config)).IsSet())
                    .Select(f => f.Name)
                    .ToArray();

                if (missing.Length == 0) {
                    Util.Log("Config parsed successfully !");
                    return config;
                }
                Util.Error($"Missing required fields : {string.Join(",", missing)}");
            } catch (System.Exception e) {
                Util.Error($"Failed to parse your config : {e}");
            }

            return null;
        }
        
        private void ParseLine(string line) {
            if (string.IsNullOrEmpty(line)) return;

            var lineByComment = line.Split(new char[] { '#' }, 2);
            if (string.IsNullOrEmpty(lineByComment[0])) return;

            var keyVal = lineByComment[0].Split(':');
            if (keyVal.Length != 2) {
                Util.Warn($"Invalid key-value line : '{line}'");
                return;
            }

            var field = typeof(Config).GetField(keyVal[0].Trim());
            if (field is null || !typeof(IField).IsAssignableFrom(field.FieldType)) {
                Util.Warn($"Invalid key : {keyVal[0]}");
                return;
            }

            var configField = (IField)field.GetValue(this);
            configField.Parse(keyVal[1].Trim());
        }
        #endregion
    }
}
