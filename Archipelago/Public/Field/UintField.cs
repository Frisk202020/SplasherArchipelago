namespace SplasherArchipelago.Public.Field {
    public class UintField : ConfigField<uint> {
        protected override uint ParseInner(string value) {
            return uint.Parse(value);
        }
    }
}
