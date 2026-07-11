namespace Core.Tools.Field {
    public interface IField {
        void Parse(string value);
        bool IsSet();
    }
}
