using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Logic.Resourse;

namespace Logic.Space_Objects {

    [Serializable]
    public abstract class CelestialBody {
        protected string name;                //имя небесного тела
        protected double area;      //площадь небесного тела
        protected double radius;    //радиус небесного тела
        protected Resourses bodyResourse;   //ресурсы на небесном теле
        /*private double moneyIncome;       //приносит столько денег в ход
        private double moneyExpenses;     //требует столько денег в ход*/

        public string Name { get => this.name; set => this.name = value; }
        public double Area { get => this.area; }
        public double Radius { get => this.radius; }
        internal Resourses BodyResourse { get => this.bodyResourse; }
        /*protected double MoneyIncome { get => this.moneyIncome; }
        protected double MoneyExpenses { get => this.moneyExpenses; }*/
    }
}
