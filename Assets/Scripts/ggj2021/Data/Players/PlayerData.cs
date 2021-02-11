using System;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.Data.Players
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "pdxpartyparrot/ggj2021/Data/Players/Player Data")]
    [Serializable]
    public sealed class PlayerData : Game.Data.Players.PlayerData
    {
        [SerializeField]
        private string _respawnTag = "PlayerRespawn";

        public string RespawnTag => _respawnTag;
    }
}
