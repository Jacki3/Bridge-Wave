using UnityEngine;
using System.Collections;
using BogGames.Helpers;

namespace BogGames.Gameplay
{
    /// <summary>
    /// Cleaner version using BogFloatRange and BogIntegerRange classes
    /// </summary>
    public class ChainObjectSpawnerRanges : ChainObjectSpawner
    {
        [Space]
        [Header("Bog Variable Ranges")]
        [SerializeField] private BogFloatRange spawnGapRange;
        [SerializeField] protected BogIntegerRange chainLengthRange;

        protected override void Start()
        {
            SyncBogVariablesToBase();
            base.Start();
        }

        protected virtual void SyncBogVariablesToBase()
        {
            minSpawnGap = spawnGapRange.Min;
            maxSpawnGap = spawnGapRange.Max;
            minObjectsInChain = chainLengthRange.Min;
            maxObjectsInChain = chainLengthRange.Max;
        }

        protected override float GetRandomSpawnDelay()
        {
            return spawnGapRange.Random;
        }

        protected override IEnumerator SpawnObjects()
        {
            if (objectsToSpawn.Count <= 0)
                yield break;

            for (int i = 0; i < spawnCount; i++)
            {
                index = i;
                SpawnRandomObject();

                var spawndelay = GetRandomSpawnDelay();

                if (i < spawnCount - 1)
                    yield return new WaitForSeconds(spawndelay);                
            }
        }

        public void AdjustSpawnGap(bool adjustMin, float amount)
        {
            spawnGapRange.Adjust(adjustMin, amount);
            SyncBogVariablesToBase();
        }

        public void AdjustChainLength(bool adjustMin, int amount)
        {
            chainLengthRange.Adjust(adjustMin, amount);
            SyncBogVariablesToBase();
        }
    }
}