using UnityEngine;
using BogGames.Helpers;
using BogGames.Variables;
using System.Collections.Generic;
using System.Linq;

namespace BogGames.Gameplay
{
    [CreateAssetMenu(fileName = "Bog Car Choices", menuName = "Scriptable Objects/BogCarChoiceList")]
    public class BogCarChoices : ScriptableObject
    {
        [SerializeField] protected List<WavingCar> carChoices = new List<WavingCar>();

        public List<WavingCar> CarChoices { get { return carChoices; } protected set { carChoices = value; } }
    }

    /// Essentially spawning chains of cars rather than generic objects
    /// We set the car speed and the latest spawned car variable values on spawned as well as ensuring telling the car what 'channel' it belongs in
    /// We also calculate the rarity of the car and adjust spawning a random car in the list based on this
    public class CarSpawner : ChainObjectSpawnerRanges
    {
        [SerializeField] protected BogFloatRange defaultSpeedRange;
        [Space]
        [SerializeField] protected Channel channel = Channel.Bridge;
        [SerializeField] protected Transform exitTarget;
        [Tooltip("When true, the next set of cars won't try to spawn until the last car reaches the the exit target.")]
        [SerializeField] protected bool waitUntilCarReachesEnd = true;
        [Header("Bog Variables")]
        [SerializeField] protected BogCarChoices carChoiceList;
        [SerializeField] protected MovingCarVariable setSpawnedCar;
        [SerializeField] protected BogFloatVariable currentWaveWaitTime;
        [SerializeField] protected BogFloatVariable defaultWaitTime;
        [SerializeField] protected BogFloatVariable speedMultiplier;
        [SerializeField] protected BogFloatVariable rarityMultiplier;
        [Header("Events")]
        [SerializeField] protected UnityEngine.Events.UnityEvent onCarSpawned;
        [SerializeField] protected UnityEngine.Events.UnityEvent onLastCarSpawned;


        private float carSpeed = 1;
        private List<float> waitTimes = new List<float>();
        private bool firstSpawn = true;

        public override void SpawnObjectSet()
        {
            spawnCount = chainLengthRange.Random;

            if(waitUntilCarReachesEnd || firstSpawn)
            {            
                carSpeed = defaultSpeedRange.Random;
                carSpeed *= speedMultiplier.Value;

                firstSpawn = false;
            }

            for(int i = 0; i < spawnCount -1; i++)
            {
                waitTimes.Add(Random.Range(minSpawnGap, maxSpawnGap));
            }

            currentWaveWaitTime.Value = waitTimes.Sum() + 1;

            StartCoroutine(SpawnObjects());
        }

        protected override void SyncBogVariablesToBase()
        {
            base.SyncBogVariablesToBase();

            if(carChoiceList == null)
                return;

            var cars = carChoiceList.CarChoices;

            if(cars.Count <= 0)
                return;

            foreach(var car in cars)
            {
                objectsToSpawn.Add(car.gameObject);
            }
        }

        protected override void SpawnRandomObject()
        {
            if(carChoiceList == null)
                return;

            var cars = carChoiceList.CarChoices;

            WavingCar randomCar = GetWeightedRandomCar(cars);

            if(randomCar == null)
                return;

            Transform spawnTransform = spawnPoint != null ? spawnPoint : transform;
            Quaternion rotation = spawnForward
                ? Quaternion.LookRotation(spawnTransform.forward, Vector3.up)
                : Quaternion.identity;

            var carGo = Instantiate(randomCar.gameObject, spawnTransform.position, rotation);

            WavingCar newCar = carGo.GetComponent<WavingCar>();
            if(newCar != null)
            {
                if(setSpawnedCar)
                    setSpawnedCar.Value = newCar;

                newCar.channel = channel;

                newCar.speed = carSpeed;

                if(exitTarget)
                    newCar.exitTarget = exitTarget;
            }

            onCarSpawned?.Invoke();

            if(spawnCount >= 1 && index == spawnCount -1)
            {
                if(exitTarget)
                {
                    float actualWait = 0;

                    float distance = Vector3.Distance(spawnTransform.position, exitTarget.position);
                    float waitTime = distance / carSpeed;

                    actualWait = waitTime;

                    if(!waitUntilCarReachesEnd)
                    {
                        float oldSpeed = carSpeed;

                        carSpeed = defaultSpeedRange.Random;
                        carSpeed *= speedMultiplier.Value;

                        float relativeSpeed = carSpeed - oldSpeed;

                        // new object is faster so can collide
                        if(relativeSpeed > 0) 
                        {
                            float distLastNeedsToTravel = Vector3.Distance(newCar.transform.position, exitTarget.position);
                            float timeUntilDestroyed = distLastNeedsToTravel / oldSpeed;
                            float currentGap = Vector3.Distance(spawnTransform.position, newCar.transform.position);

                            float timeUntilCollision = currentGap / relativeSpeed;

                            if(timeUntilCollision > timeUntilDestroyed)
                            {
                                actualWait = 0;
                            }
                            else
                            {
                                actualWait = waitTime;
                            }
                        }
                        else
                        {
                            // new object slower so you can spawn right now
                            actualWait = 0;
                        }
                    }

                    currentWaveWaitTime.Value = actualWait;
                    onLastCarSpawned?.Invoke();
                }
            }
        }

        protected override float GetRandomSpawnDelay()
        {
            if (waitTimes == null || waitTimes.Count == 0)
            {
                return Random.Range(minSpawnGap, maxSpawnGap);
            }

            int randIndex = Random.Range(0, waitTimes.Count);
            float selectedTime = waitTimes[randIndex];
            waitTimes.RemoveAt(randIndex);
            return selectedTime;
        }

        protected WavingCar GetWeightedRandomCar(List<WavingCar> cars)
        {
            if(cars.Count <= 0)
                return null;

            float totalWeight = 0f;
            foreach (var obj in cars)
            {
                float weight = Mathf.Pow(obj.CarRarity, 1f / rarityMultiplier.Value);
                totalWeight += weight;
            }

            float randomValue = Random.Range(0f, totalWeight);

            float cumulative = 0f;
            foreach (var obj in cars)
            {
                float weight = Mathf.Pow(obj.CarRarity, 1f / rarityMultiplier.Value);
                cumulative += weight;

                if (randomValue <= cumulative)
                    return obj;
            }

            return cars[cars.Count - 1];
        }
    }
}
