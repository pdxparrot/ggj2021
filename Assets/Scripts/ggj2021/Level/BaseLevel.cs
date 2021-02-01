using JetBrains.Annotations;

using pdxpartyparrot.ggj2021.World;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.Level
{
    public interface IBaseLevel
    {
        Transform SheepPen { get; }

        [CanBeNull]
        Goal Goal { get; }
    }
}
