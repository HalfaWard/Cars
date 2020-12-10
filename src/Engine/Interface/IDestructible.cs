using System.Collections.Generic;
using Engine.Items;

namespace Engine.Interface
{
    public interface IDestructible
    {
        float Health { get; set; }
        float MaxHealth { get; set; }
        List<Item> DropItems { get; set; }
        void OnDestruction();
    }
}
