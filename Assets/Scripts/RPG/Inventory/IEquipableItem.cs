namespace RPG.Inventory
{
    public interface IEquipableItem
    {
        EquipLocation EquipLocation { get; }
    }
    
    public enum EquipLocation
    {
        Helmet,
        Necklace,
        Chest,
        Trousers,
        Boots,
        Gloves,
        Weapon,
        Shield,
    }
}