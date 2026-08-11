namespace Core.Tools.Field {
    public class StringField : ConfigField<string> {
        protected override string ParseInner(string value) {
            return value;
        }

        public bool? AsNullableBool() {
            switch(Value) {
                case "true": return true;
                case "false": return false;
                default: return null;
            }
        }
    }
}
