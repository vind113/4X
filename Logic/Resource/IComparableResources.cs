namespace Logic.Resource {
    public interface IComparableResources : IBasicResources {
        bool IsEqual(IBasicResources res);
        bool IsNotEqual(IBasicResources res);
        bool IsStrictlyGreater(IBasicResources res);
    }
}