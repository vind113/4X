using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Logic.Resourse;

namespace Logic.Player {
    public static class Player {
        public static double PlayerCitizenHub {
            get => CitizenHub.CitizensInHub;
            set => CitizenHub.CitizensInHub = value;
        }

        public static Resourses PlayerResourses {
            get => Stockpile.PlayerResourses;
            set => Stockpile.PlayerResourses = value;
        }

        public static double PlayerMoney {
            get => Stockpile.Money;
            set => Stockpile.Money = value;
        }

        public static void InitGame() {
            Stockpile.PlayerResourses = new Resourses(0, 0, 0);
            Stockpile.Money = 0;
            CitizenHub.CitizensInHub = 0;
        }

        public static void NextTurn() {

        }
    }
}
