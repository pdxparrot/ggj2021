using System;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.Data.Players
{
    [CreateAssetMenu(fileName = "PlayerBehaviorData", menuName = "pdxpartyparrot/ggj2021/Data/Players/PlayerBehavior Data")]
    [Serializable]
    public sealed class PlayerBehaviorData : Game.Data.Characters.PlayerBehaviorData
    {
        [SerializeField]
        private float _launchCooldown = 0.25f;

        public float LaunchCooldown => _launchCooldown;
    }
}
