using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ArithmosModels
{
    /// <summary>
    /// Base class to allow derived classes implement property changed notifications and error notifications
    /// </summary>
    public class ModelBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        protected virtual void OnPropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        public IEnumerable GetErrors(string propertyName)
        {
            if (this.errors.ContainsKey(propertyName))
            {
                return this.errors[propertyName];
            }
            else
            {
                return null;
            }
        }

        public bool HasErrors
        {
            get { return this.errors.Count > 0; }
        }

        public void AddError(string propertyName, string error)
        {
            if (!this.errors.ContainsKey(propertyName))
            {
                this.errors[propertyName] = new List<string>();
            }

            if (!this.errors[propertyName].Contains(error))
            {
                this.errors[propertyName].Add(error);
                RaiseErrorsChanged(propertyName);
            }
        }

        public void RemoveError(string propertyName, string error)
        {
            if (this.errors.ContainsKey(propertyName) && errors[propertyName].Contains(error))
            {
                this.errors[propertyName].Remove(error);
                if (this.errors[propertyName].Count == 0) errors.Remove(propertyName);
                RaiseErrorsChanged(propertyName);
            }
        }

        public void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    }
}