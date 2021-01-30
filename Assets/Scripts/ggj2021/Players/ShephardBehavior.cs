using JetBrains.Annotations;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Interactables;
using pdxpartyparrot.ggj2021.NPCs;

using Spine.Unity;

using UnityEngine;

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
        private BoneFollower _sheepAttachment;

        [SerializeField]
        [ReadOnly]
        [CanBeNull]
        private Sheep _heldSheep;

        public bool IsHoldingSheep => _heldSheep != null;

        public bool CanPickUpSheep => !IsHoldingSheep;

        #endregion

        private Interactables _interactables;

        #region Unity Lifecycle

        private void Awake()
        {
            _interactables = GetComponent<Interactables>();
        }

        #endregion

        public void Initialize()
        {
        }

        #region Sheep

        private bool PickUpSheep()
        {
            if(!CanPickUpSheep) {
                return false;
            }

            Sheep sheep = _interactables.GetRandomInteractable<Sheep>();
            if(null == sheep) {
                return false;
            }

            /*if(!sheep.Hold(this)) {
                return false;
            }*/

            _heldSheep = sheep;
            _interactables.RemoveInteractable(_heldSheep);

            _heldSheep.transform.SetParent(_sheepAttachment.transform);

            return true;
        }

        #endregion
    }
}
