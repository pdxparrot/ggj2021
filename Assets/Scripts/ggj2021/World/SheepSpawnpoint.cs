using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Collections;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.ggj2021.NPCs;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2021.World
{
    public sealed class SheepSpawnpoint : SpawnPoint
    {
        [SerializeField]
        private string[] _sheepTags;

        protected override void InitActor(Actor actor)
        {
            Assert.IsTrue(actor is Sheep);

            base.InitActor(actor);

            Sheep sheep = (Sheep)actor;
            if(string.IsNullOrWhiteSpace(sheep.Tag)) {
                sheep.Tag = _sheepTags.GetRandomEntry();
            }
        }
    }
}
