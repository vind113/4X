namespace Logic.TechnologyClasses {
    public abstract class Technology {
        public string Name { get; }
        public int Level { get; }

        public Technology(string name, int level) {
            this.Name = name;
            this.Level = level;
        }
    }
}