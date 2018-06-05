using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameLogic.Resourse;

namespace GameLogic {
    struct Stockpile {
        double money;               //деньги, доступные игроку
        Resourses playerResourses;  //ресурсы на складе
    }
}
