using System;

namespace Logic.GameClasses {
    public class CurrentDate {
        private int currentTurn;
        private byte currentMonth;
        private short currentYear;

        public CurrentDate() {
            currentTurn = 1;
            currentMonth = 1;
            currentYear = 2500;
        }

        public CurrentDate(int turn, byte month, short year) {
            this.currentTurn = turn;
            this.currentMonth = month;
            this.currentYear = year;
        }

        public CurrentDate NextTurn() {
            int newTurn = currentTurn + 1;

            byte newMonth = (byte)(currentMonth + 1);
            short newYear = this.currentYear;
            if (newMonth == 13) {
                newMonth = 1;
                newYear++;
            }

            return new CurrentDate(newTurn, newMonth, newYear);
        }

        public int Turn {
            get => currentTurn;
        }

        public string Date {
            get => $"{currentMonth}.{currentYear}";
        }
    }
}
