using System;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.Data.Players
{
    [CreateAssetMenu(fileName = "PlayerBehaviorData", menuName = "pdxpartyparrot/ggj2021/Data/Players/PlayerBehavior Data")]
    [Serializable]
    public sealed class PlayerBehaviorData : Game.Data.Characters.PlayerBehaviorData
    {
        [SerializeField]
        private string _launchSheepAnimationEvent = "sheep_launch";

        public string LaunchSheepAnimationEvent => _launchSheepAnimationEvent;

        [SerializeField]
        private float _teleportCooldown = 2.0f;

        public float TeleportCooldown => _teleportCooldown;

        [SerializeField]
        private Vector3 _launchDirection = new Vector3(0.0f, 1.0f, 0.0f);

        public Vector3 LaunchDirection => _launchDirection;
    }
}
