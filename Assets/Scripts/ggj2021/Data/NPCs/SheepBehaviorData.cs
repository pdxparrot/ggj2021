using System;

using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.Data.NPCs
{
    [CreateAssetMenu(fileName = "SheepBehaviorData", menuName = "pdxpartyparrot/ggj2021/Data/NPCs/SheepBehavior Data")]
    [Serializable]
    public sealed class SheepBehaviorData : Game.Data.Characters.NPCBehaviorData
    {
        [SerializeField]
        private IntRangeConfig _noiseFrequency;

        public IntRangeConfig NoiseFrequency => _noiseFrequency;
    }
}
