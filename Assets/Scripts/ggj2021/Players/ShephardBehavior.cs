using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Interactables;
using pdxpartyparrot.ggj2021.NPCs;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2021.Players
{
    // TODO: make an ActorComponent
    [RequireComponent(typeof(Interactables3D))]
    public sealed class ShephardBehavior : MonoBehaviour
    {
        [SerializeField]
        private Player _owner;

        public Player Owner => _owner;

        [Space(10)]

        #region Sheep

        [SerializeField]
        private Transform _chamberParent;

        [SerializeField]
        [ReadOnly]
        [CanBeNull]
        private Sheep _chamber;

        [SerializeField]
        [ReadOnly]
        private List<Sheep> _magazine = new List<Sheep>();

        public bool HasCapacity => null == _chamber || (GameManager.HasInstance && _magazine.Count < GameManager.Instance.GameGameData.MaxQueuedSheep);

        #endregion

        [SerializeField]
        [ReadOnly]
        private ITimer _launchCooldown;

        public bool CanLaunch => null != _chamber && !_launchCooldown.IsRunning;

        private Interactables _interactables;

        #region Unity Lifecycle

        private void Awake()
        {
            _interactables = GetComponent<Interactables>();

            _launchCooldown = TimeManager.Instance.AddTimer();
        }

        private void Update()
        {
            CatchSheep();
        }

        private void OnDestroy()
        {
            if(TimeManager.HasInstance) {
                TimeManager.Instance.RemoveTimer(_launchCooldown);
            }
        }

        #endregion

        public void Initialize()
        {
        }

        #region Sheep

        public bool CatchSheep()
        {
            if(!HasCapacity) {
                return false;
            }

            Sheep sheep = _interactables.GetRandomInteractable<Sheep>();
            if(null == sheep || sheep.CanScore) {
                return false;
            }

            Debug.Log($"Caught sheep {sheep.Id}");

            if(null == _chamber) {
                ChamberSheep(sheep);
            } else {
                EnqueueSheep(sheep);
            }

            _interactables.RemoveInteractable(sheep);

            return true;
        }

        private void ChamberSheep(Sheep sheep)
        {
            Assert.IsNull(_chamber);

            _chamber = sheep;
            sheep.OnChambered(_chamberParent);

            Debug.Log($"Chambered sheep {_chamber.Id}");
        }

        private void EnqueueSheep(Sheep sheep)
        {
            Assert.IsTrue(HasCapacity);

            _magazine.Add(sheep);

            if(_magazine.Count == 1) {
                sheep.OnEnqueued(Owner.transform);
            } else {
                sheep.OnEnqueued(GameManager.Instance.GameGameData.SheepTargetPlayer
                    ? Owner.transform
                    : _magazine.ElementAt(_magazine.Count - 2).transform);
            }
        }

        public bool LaunchSheep()
        {
            if(!CanLaunch) {
                return false;
            }

            Sheep sheep = _chamber;
            _chamber = null;

            Debug.Log($"Launching sheep {sheep.Id}");

            sheep.OnLaunch(Owner.Movement.Position, Owner.FacingDirection);

            CycleRound();

            _launchCooldown.Start(Owner.GamePlayerBehavior.GamePlayerBehaviorData.LaunchCooldown);

            return true;
        }

        private bool CycleRound()
        {
            if(_magazine.Count < 1) {
                return false;
            }

            Sheep sheep = _magazine.ElementAt(0);
            _magazine.RemoveAt(0);

            ChamberSheep(sheep);

            RackSheep();

            return true;
        }

        private void RackSheep()
        {
            if(_magazine.Count < 1) {
                return;
            }

            _magazine.ElementAt(0).OnEnqueued(Owner.transform);
            for(int i = 1; i < _magazine.Count; ++i) {
                _magazine.ElementAt(i).OnEnqueued(GameManager.Instance.GameGameData.SheepTargetPlayer
                    ? Owner.transform
                    : _magazine.ElementAt(i - 1).transform);
            }
        }

        private void FreeSheep()
        {
            // we get to keep the chambered sheep

            foreach(Sheep sheep in _magazine) {
                sheep.OnFree();
            }
            _magazine.Clear();
        }

        #endregion

        #region Spawn

        public bool OnSpawn(SpawnPoint spawnpoint)
        {
            return true;
        }

        public bool OnReSpawn(SpawnPoint spawnpoint)
        {
            FreeSheep();

            return true;
        }

        public void OnDeSpawn()
        {
            FreeSheep();
        }

        #endregion
    }
}
