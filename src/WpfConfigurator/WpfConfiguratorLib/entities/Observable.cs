using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfConfiguratorLib.Annotations;

namespace WpfConfiguratorLib.entities
{
    public class Observable<T> : INotifyPropertyChanged
    {
        #region Private Fields

        private object _value;

        #endregion



        #region Public Properties

        public object Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion



        #region Private Methods



        #endregion



        #region Public Methods

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion



        #region Operators

        public static implicit operator Observable<T>(string value)
        {
            return new Observable<T> { Value = value };
        }

        public static implicit operator Observable<T>(int value)
        {
            return new Observable<T> { Value = value };
        }

        public static implicit operator Observable<T>(long value)
        {
            return new Observable<T> { Value = value };
        }

        public static implicit operator Observable<T>(float value)
        {
            return new Observable<T> { Value = value };
        }

        public static implicit operator Observable<T>(double value)
        {
            return new Observable<T> { Value = value };
        }

        public static implicit operator Observable<T>(bool value)
        {
            return new Observable<T> { Value = value };
        }

        #endregion
    }
}
