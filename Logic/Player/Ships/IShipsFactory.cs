namespace Logic.PlayerClasses {
    public interface IShipsFactory {
        Colonizer GetColonizer();
        int GetMiners(int quantity);
    }
}