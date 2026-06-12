namespace SplasherArchipelago.Public.Field {
    public class FloatField : ConfigField<float> {
        protected override float ParseInner(string value) {
            return float.Parse(value);
        }
    }
}
