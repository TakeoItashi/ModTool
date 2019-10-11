using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModTool
{
    public class ObjectBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string _propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(_propertyName));
            }
        }

        protected virtual void SetProperty<T>(ref T _property, T _value)
        {
            _property = _value;
            OnPropertyChanged(nameof(_property));
        }
    }
}
