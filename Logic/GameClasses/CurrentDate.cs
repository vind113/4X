using System;

namespace Logic.GameClasses {
    public class CurrentDate {
        static int currentTurn;
        static byte currentMonth;
        static short currentYear;

        public CurrentDate() {
            currentTurn = 1;
            currentMonth = 1;
            currentYear = 2500;
        }

        public void NextTurn() {
            currentTurn++;

            currentMonth++;
            if (currentMonth == 13) {
                currentMonth = 1;
                currentYear++;
            }
        }

        public int Turn {
            get => currentTurn;
        }

        public string Date {
            get => $"{currentMonth}.{currentYear}";
        }
    }
}
