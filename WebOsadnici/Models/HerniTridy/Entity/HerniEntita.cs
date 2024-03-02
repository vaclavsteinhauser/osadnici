using System;
using System.ComponentModel;

namespace WebOsadnici.Models.HerniTridy
{
    /// <summary>
    /// Základní třída pro herní entity s implementací událostí pro notifikaci o změně vlastností.
    /// </summary>
    public class HerniEntita : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        private Guid _id = Guid.NewGuid();

        /// <summary>
        /// Identifikátor herní entity.
        /// </summary>
        public virtual Guid Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    OnPropertyChanging(nameof(Id));
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        /// <summary>
        /// Vyvolá událost o změně hodnoty vlastnosti.
        /// </summary>
        /// <param name="propertyName">Název změněné vlastnosti.</param>
        protected virtual void OnPropertyChanging(string propertyName)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// Vyvolá událost o změně hodnoty vlastnosti.
        /// </summary>
        /// <param name="propertyName">Název změněné vlastnosti.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}