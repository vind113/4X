using Logic.PlayerClasses;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Logic.GameClasses {
    [Serializable]
    public class Game : INotifyPropertyChanged {
        private Player player;
        private TurnDate gameDate;
        private bool isAutoColonizationEnabled = false;

        [field:NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Game() {
            Player = new Player();
            GameDate = new TurnDate();
        }

        #region Properties
        public bool IsAutoColonizationEnabled {
            get => isAutoColonizationEnabled;
            set => isAutoColonizationEnabled = value;
        }

        public Player Player {
            get => this.player;
            set {
                if (this.Player == value) {
                    return;
                }
                this.player = value;
                OnPropertyChanged();
            }
        }

        public TurnDate GameDate {
            get => this.gameDate;
            set {
                this.gameDate = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Next Turn Functionality
        public void NextTurn() {
            GameDate = GameDate.NextTurn();
            player.NextTurn(IsAutoColonizationEnabled, true);
        }
        #endregion
    }
}
