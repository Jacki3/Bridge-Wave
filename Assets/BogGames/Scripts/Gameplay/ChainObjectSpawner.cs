using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BogGames.Gameplay
{
    /// Spawns a 'chain' or 'group' of objects using a random value between a min and max range
    public class ChainObjectSpawner : MonoBehaviour
    {
        [Range(0,999)]
        [SerializeField] protected float minSpawnGap;
        [Range(1, 999)]
        [SerializeField] protected float maxSpawnGap;
        [Range(1, 999)]
        [SerializeField] protected int minObjectsInChain;
        [Tooltip("Will spawn a random number of objects based from min value to the value set here")]
        [Range(1, 999)]
        [SerializeField] protected int maxObjectsInChain;
        [Tooltip("The minimum gap allowed between cars - often setting this to 0 causes overlaps")]
        [SerializeField] protected float minimumGapAllowed = 0.5f;
        [Tooltip("If not provided the object (s) spawn at this objects location")]
        [SerializeField] protected Transform spawnPoint;
        [SerializeField] protected bool spawnForward;
        [SerializeField] protected List<GameObject> objectsToSpawn = new List<GameObject>();

        protected int spawnCount;
        protected int index = 0;

        public virtual void SpawnObjectSet()
        {
            spawnCount = Random.Range(minObjectsInChain, maxObjectsInChain);
            StartCoroutine(SpawnObjects());
        }

        public void AdjustSpawnGap(bool changeMin, bool increase, float amount)
        {
            float adjustment = increase ? amount : -amount;
            
            if (changeMin)
                minSpawnGap += adjustment;
            else
                maxSpawnGap += adjustment;
        }

        protected virtual void Start()
        {
        }

        protected virtual void Update()
        {
            if(minSpawnGap <= minimumGapAllowed)
                minSpawnGap = minimumGapAllowed;

            // the minimum and maximum cars in a chain has to be at least 1 otherwise this breaks
            if(minObjectsInChain <= 1)
                minObjectsInChain = 1;
            if(maxObjectsInChain <= 1)
                maxObjectsInChain = 1;
        }

        protected virtual IEnumerator SpawnObjects()
        {
            if (objectsToSpawn.Count <= 0)
                yield break;
                        
            for (int i = 0; i < spawnCount; i++)
            {
                index = i;
                SpawnRandomObject();
                yield return new WaitForSeconds(GetRandomSpawnDelay());
            }
        }

        protected virtual void SpawnRandomObject()
        {
            int randomIndex = Random.Range(0, objectsToSpawn.Count);
            GameObject prefab = objectsToSpawn[randomIndex];
            
            Transform spawnTransform = spawnPoint != null ? spawnPoint : transform;
            Quaternion rotation = spawnForward 
                ? Quaternion.LookRotation(spawnTransform.forward, Vector3.up) 
                : Quaternion.identity;
            
            Instantiate(prefab, spawnTransform.position, rotation);
        }

        protected virtual float GetRandomSpawnDelay()
        {
            return Random.Range(minSpawnGap, maxSpawnGap);
        }
    }
}
