namespace LDtkUnity
{
    public class InjectorDataPair
    {
        private LDtkFieldInjectorData _field;
        private FieldInstance _data;

        public InjectorDataPair(FieldInstance data, LDtkFieldInjectorData field)
        {
            this._data = data;
            this._field = field;
        }

        public LDtkFieldInjectorData Field => _field;

        public FieldInstance Data => _data;
    }
}