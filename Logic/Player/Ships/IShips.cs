using Logic.Resource;

namespace Logic.PlayerClasses {
    public interface IShips {
        Colonizer GetColonizer(IResources from);
        int GetMiners(Resources from, int quantity);
    }
}