using UnityEngine;
using System.Collections.Generic;

namespace BogGames.Gameplay
{
    public class TrafficSpawnerNode : MonoBehaviour
    {
        [Header("Manager & Types")]
        public TrafficConductor manager;
        public Channel channel = Channel.Bridge;
        public MovingCarVariable spawnedCar;
        public CarType[] vehicleTypes;

        [Header("Spawn/Exit Points")]
        public Transform spawnPoint;            
        public Transform exitPoint;                 

        [Header("Optional lane group (shared headway across nodes)")]
        public string laneGroupId = "";

        [Header("Despawn")]
        public float defaultLifetime = 14f;

        [Header("Events")]
        public UnityEngine.Events.UnityEvent onCarSpawned;

        float _next; float _lastSpawnTime; float _lastSpeed; float _lastLen;
        bool _active;

        static readonly Dictionary<string, Headway> Shared = new();
        struct Headway { public float time; public float speed; public float len; }

        void Reset() { spawnPoint = transform; }

        public void StartSpawning()
        {
            if (!manager) manager = FindFirstObjectByType<TrafficConductor>();
            _active = true;
            _next = Time.time + Random.Range(0.05f, 0.25f);
        }

        public void StopSpawning() => _active = false;

        void Update()
        {
            if (!_active || !manager || vehicleTypes == null || vehicleTypes.Length == 0 || !spawnPoint)
                return;

            if (!manager.IsWaveActive)
            {
                float jitter = Random.Range(0f, 0.2f);
                _next = Mathf.Max(_next, manager.NextPhaseSwitchTime + jitter);
                return;
            }

            // current lambda and rare multiplier
            float lambda = 0f, rareMul = 1f;
            if (channel == Channel.Bridge)
            {
                lambda = manager.GetBridgeLambdaPerSec();
                rareMul = 1f;
            }
            else manager.GetBelowChannel(channel, out lambda, out rareMul);

            if (Time.time >= _next)
            {
                _next = Time.time + (lambda > 0f ? manager.SampleExp(lambda) : 0.3f);
                TrySpawn(rareMul);
            }
        }

        void TrySpawn(float rareMul)
        {
            var type = PickWeighted(vehicleTypes, rareMul);
            if (!type || !type.prefab) return;

            float speed = type.SampleSpeed(manager.RNG, manager.SpeedMult);

            // headway check (shared group if provided)
            if (!HeadwayOk(speed, type.lengthMeters)) { _next += 0.12f; return; }

            Vector3 fwd = spawnPoint.forward;
            Vector3 pos = spawnPoint.position + fwd * speed * Mathf.Max(0.5f, manager.LeadTime);
            Quaternion rot = Quaternion.LookRotation(fwd, Vector3.up);
            var go = Instantiate(type.prefab, pos, rot);

            var mv = go.GetComponent<MovingCar>();
            mv.speed = speed;
            mv.exitTarget = exitPoint;
            mv.maxLifetime = defaultLifetime;

            WavingCar wc = mv as WavingCar;
            WavingCarChain wcc = mv as WavingCarChain;

            if(wc != null)
            {
                wc.channel = this.channel;

                if(spawnedCar)
                    spawnedCar.Value = mv;

                onCarSpawned?.Invoke();
            }
            else if(wcc != null)
            {
                var cars = wcc.WavingCarChainArray;

                if(cars == null || cars.Length <= 0)
                    return;

                foreach(var car in cars)
                {
                    if(spawnedCar)
                        spawnedCar.Value = car;
                    car.channel = this.channel;
                    onCarSpawned?.Invoke();
                }
            }

            StampHeadway(speed, type.lengthMeters);
            _lastSpawnTime = Time.time; _lastSpeed = speed; _lastLen = type.lengthMeters;
        }

        bool HeadwayOk(float newSpeed, float newLen)
        {
            float lastTime, lastSpeed, lastLen;
            if (TryGetShared(out lastTime, out lastSpeed, out lastLen))
            {
                float since = Time.time - lastTime;
                float prevClear = lastLen / Mathf.Max(0.1f, lastSpeed);
                float need = Mathf.Max(manager.MinHeadway, prevClear + 0.15f);
                return since >= need;
            }
            else
            {
                float since = Time.time - _lastSpawnTime;
                float prevClear = _lastLen / Mathf.Max(0.1f, _lastSpeed);
                float need = Mathf.Max(manager.MinHeadway, prevClear + 0.15f);
                return _lastSpawnTime <= 0f || since >= need;
            }
        }

        void StampHeadway(float speed, float len)
        {
            if (!string.IsNullOrEmpty(laneGroupId))
                Shared[laneGroupId] = new Headway { time = Time.time, speed = speed, len = len };
        }

        bool TryGetShared(out float time, out float speed, out float len)
        {
            if (!string.IsNullOrEmpty(laneGroupId) && Shared.TryGetValue(laneGroupId, out var h))
            { time = h.time; speed = h.speed; len = h.len; return true; }
            time = speed = len = 0f; return false;
        }

        CarType PickWeighted(CarType[] types, float rareMul)
        {
            if (types.Length == 1) return types[0];

            float minW = float.MaxValue, maxW = float.MinValue;
            for (int i = 0; i < types.Length; i++)
            {
                float w = Mathf.Max(0.0001f, types[i].rarityWeight);
                if (w < minW) minW = w; if (w > maxW) maxW = w;
            }
            float cut = Mathf.Lerp(minW, maxW, 0.7f);

            float total = 0f;
            for (int i = 0; i < types.Length; i++)
            {
                float w = Mathf.Max(0.0001f, types[i].rarityWeight);
                if (w >= cut) w *= rareMul;
                total += w;
            }

            double r = manager.RNG.NextDouble() * total; float acc = 0f;
            for (int i = 0; i < types.Length; i++)
            {
                float w = Mathf.Max(0.0001f, types[i].rarityWeight);
                if (w >= cut) w *= rareMul;
                acc += w;
                if (r <= acc) return types[i];
            }
            return types[types.Length - 1];
        }
    }
}
