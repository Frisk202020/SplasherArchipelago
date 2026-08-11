namespace Core.Tools.Field {
    public class IntField : ConfigField<int> {
        protected override int ParseInner(string value) {
            return int.Parse(value);
        }
    }
}