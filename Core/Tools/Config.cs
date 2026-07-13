using System.IO;
using System.Linq;
using Core.Tools.Field;

namespace Core.Tools {
    public class Config {
        public static Config Instance;

        #region ConnectionInfo
        public readonly StringField Address = new StringField();
        public readonly UintField Port = new UintField();
        public readonly StringField Slot = new StringField();
        public readonly BoolField Proxy = new BoolField();
        #endregion

        #region QoL
        public readonly FloatField CutsceneSpeed = new FloatField();
        public readonly BoolField EnableSpeedOnCredits = new BoolField();
        public readonly BoolField ShowLevelTitle = new BoolField();
        #endregion

        private Config() { }

        #region Parse Implementation
        public static bool Parse() {
            if (Instance != null) return true;

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
                    Static.Log("Config parsed successfully !");
                    Static.StartConfigEvents(config);
                    Instance = config;

                    return true;
                }
                Static.Error($"Missing required fields : {string.Join(",", missing)}");
            } catch (System.Exception e) {
                Static.Error($"Failed to parse your config : {e}");
            }

            return false;
        }
        
        private void ParseLine(string line) {
            if (string.IsNullOrEmpty(line)) return;

            var lineByComment = line.Split(new char[] { '#' }, 2);
            if (string.IsNullOrEmpty(lineByComment[0])) return;

            var keyVal = lineByComment[0].Split(':');
            if (keyVal.Length != 2) {
                Static.Warn($"Invalid key-value line : '{line}'");
                return;
            }

            var field = typeof(Config).GetField(keyVal[0].Trim());
            if (field is null || !typeof(IField).IsAssignableFrom(field.FieldType)) {
                Static.Warn($"Invalid key : {keyVal[0]}");
                return;
            }

            var configField = (IField)field.GetValue(this);
            configField.Parse(keyVal[1].Trim());
        }
        #endregion
    }
}
