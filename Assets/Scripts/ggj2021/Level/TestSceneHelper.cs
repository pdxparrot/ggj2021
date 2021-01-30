using pdxpartyparrot.Core.World;
using pdxpartyparrot.ggj2021;

namespace pdxpartyparrot.gg2021.Level
{
    public class TestSceneHelper : Game.Level.TestSceneHelper
    {
        public void SpawnSheep()
        {
            SpawnPoint spawnPoint = SpawnManager.Instance.GetSpawnPoint(GameManager.Instance.GameGameData.SheepSpawnTag);
            spawnPoint.SpawnNPCPrefab(GameManager.Instance.GameGameData.SheepPrefab, GameManager.Instance.GameGameData.SheepBehaviorData);
        }
    }
}
