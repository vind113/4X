namespace Logic.TechnologyClasses {
    public abstract class Technology {
        public string Name { get; }
        public int Level { get; }

        protected Technology(string name, int level) {
            this.Name = $"{name} {level}";
            this.Level = level;
        }
    }
}