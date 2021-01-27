using pdxpartyparrot.Game.State;

namespace pdxpartyparrot.ggj2021.Menu
{
    public sealed class MainMenu : Game.Menu.MainMenu
    {
        #region Events

        public override void OnStart()
        {
            base.OnStart();

            GameStateManager.Instance.StartLocal(GameManager.Instance.GameData.MainGameStatePrefab);
        }

        #endregion
    }
}
