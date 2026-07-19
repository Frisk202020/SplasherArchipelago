namespace Core.Tools.Field {
    public abstract class ConfigField<T> : IField {
        private bool isSet = false;  
        private bool hasDefault = false;
        private T defaultBinding;

        public T DefaultBinding { 
            get { return defaultBinding; }
            set { hasDefault = true; defaultBinding = value; _value = defaultBinding; }
        }

        public bool HasDefault() => hasDefault;

        private T _value;
        public T Value {
            get { return _value; }
            private set {
                isSet = true;
                _value = value;
            }
        }

        protected abstract T ParseInner(string input);
        public void Parse(string input) {
            Value = ParseInner(input);
        }

        public bool IsSet() {
            return isSet;
        }
    }
}
