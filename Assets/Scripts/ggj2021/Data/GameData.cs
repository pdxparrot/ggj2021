using System;

using pdxpartyparrot.ggj2021.Camera;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.Data
{
    [CreateAssetMenu(fileName = "GameData", menuName = "pdxpartyparrot/ggj2021/Data/Game Data")]
    [Serializable]
    public sealed class GameData : Game.Data.GameData
    {
        public GameViewer GameViewerPrefab => (GameViewer)ViewerPrefab;
    }
}
