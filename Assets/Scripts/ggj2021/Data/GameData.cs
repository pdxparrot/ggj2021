using System;

using pdxpartyparrot.ggj2021.Camera;
using pdxpartyparrot.ggj2021.Data.NPCs;
using pdxpartyparrot.ggj2021.NPCs;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.Data
{
    [CreateAssetMenu(fileName = "GameData", menuName = "pdxpartyparrot/ggj2021/Data/Game Data")]
    [Serializable]
    public sealed class GameData : Game.Data.GameData
    {
        public GameViewer GameViewerPrefab => (GameViewer)ViewerPrefab;

        [SerializeField]
        private string _sheepSpawnTag = "Sheep";

        public string SheepSpawnTag => _sheepSpawnTag;

        [SerializeField]
        private Sheep _sheepPrefab;

        public Sheep SheepPrefab => _sheepPrefab;

        [SerializeField]
        private SheepBehaviorData _sheepBehaviorData;

        public SheepBehaviorData SheepBehaviorData => _sheepBehaviorData;

        [SerializeField]
        private int _maxQueuedSheep = 4;

        public int MaxQueuedSheep => _maxQueuedSheep;
    }
}
