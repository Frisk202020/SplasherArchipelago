namespace SplasherArchipelago.Public.Field {
    public class StringField : ConfigField<string> {
        protected override string ParseInner(string value) {
            return value;
        }
    }
}
