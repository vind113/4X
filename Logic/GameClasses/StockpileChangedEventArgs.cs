﻿using Logic.Resource;
using System;

namespace Logic.GameClasses {
    public class StockpileChangedEventArgs : EventArgs {
        private double money = 0; 
        private Resources argResources;

        public StockpileChangedEventArgs(double money, Resources argResources) {
            this.money = money;
            this.argResources = argResources ?? throw new ArgumentNullException(nameof(argResources));
        }

        public double Money { get => this.money; }
        public Resources ArgResources { get => this.argResources; }
    }
}
