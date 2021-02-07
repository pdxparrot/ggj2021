using UnityEngine;

namespace pdxpartyparrot.ggj2021.Level
{
    public interface IBaseLevel
    {
        Transform SheepPen { get; }

        float TimePercent { get; }
    }
}
