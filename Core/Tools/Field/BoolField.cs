namespace Core.Tools.Field {
    public class BoolField : ConfigField<bool> {
        protected override bool ParseInner(string value) {
            return value == "true";
        }
    }
}
