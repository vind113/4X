using System;

namespace Logic.GameClasses {
    public class TurnDate {
        private int currentTurn;
        private byte currentMonth;
        private short currentYear;

        public TurnDate() {
            currentTurn = 1;
            currentMonth = 1;
            currentYear = 2500;
        }

        public TurnDate(int turn, byte month, short year) {
            this.currentTurn = turn;
            this.currentMonth = month;
            this.currentYear = year;
        }

        public TurnDate NextTurn() {
            int newTurn = currentTurn + 1;

            byte newMonth = (byte)(currentMonth + 1);
            short newYear = this.currentYear;
            if (newMonth == 13) {
                newMonth = 1;
                newYear++;
            }

            currentTurn = newTurn;
            currentMonth = newMonth;
            currentYear = newYear;

            return this;
        }

        public int Turn {
            get => currentTurn;
        }

        public string Date {
            get => $"{currentMonth}.{currentYear}";
        }
    }
}
