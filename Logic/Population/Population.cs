using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Logic.PopulationClasses {
    [Serializable]
    public class Population : INotifyPropertyChanged {
        private long value;
        private long maxValue;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        #region Constructors
        public Population(long value, long maxValue) {
            this.MaxValue = maxValue;
            this.Value = value;
        }

        public Population(double value, double maxValue)
            : this((long)Math.Floor(value), (long)Math.Floor(maxValue)) { }

        public Population(long value, double maxValue)
            : this(value, (long)Math.Floor(maxValue)) { }

        public Population(double value, long maxValue)
            : this((long)Math.Floor(value), maxValue) { }
        #endregion

        public long Value {
            get { return this.value; }
            private set {
                if (0 <= value && value <= this.MaxValue) this.value = value;
                OnPropertyChanged();
            }
        }

        public long MaxValue {
            get { return this.maxValue; }
            private set {
                if (value >= 0) this.maxValue = value;
            }
        }

        public void Add(long change) {
            if (change > 0) this.Value += change;
        }

        public void Add(double change) {
            this.Add((long)change);
        }

        public void Subtract(long change) {
            if (change > 0) this.Value -= change;
        }

        public void Subtract(double change) {
            this.Subtract((long)change);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
