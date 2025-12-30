using UnityEngine;

namespace BogGames.Gameplay
{    
    public class TrafficConductor : MonoBehaviour
    {
        [Header("Rates (baseline per side)")]
        public float belowSpawnsPerMin = 36f; 
        public float bridgeSpawnsPerMin = 20f;

        [Header("Gameplay Multipliers")]
        public float speedMultiplier = 1f;

        [Header("Readability / Fairness")]
        public float minHeadwaySeconds = 0.9f;
        public float readableLeadTime = 1.0f;

        [Header("Camping Pressure")]
        public float graceSeconds = 2f;
        public float rampSeconds = 6f;
        [Range(0, 0.9f)] public float maxInhibitPct = 0.6f;
        [Range(0, 1.0f)] public float maxBoostPct = 0.5f;
        [Range(0, 3f)]  public float maxRareBoost = 2.0f;

        [Header("Waves")]
        public bool useWaves = true;
        public float waveDuration = 4f;        // seconds spawning is allowed
        public float breakDuration = 2f;       // seconds with no spawns
        public float waveRateMultiplier = 1.0f; // e.g., 1.0–1.5 to make waves feel punchier

        [Header("Player Hook (set each frame)")]
        public Side playerSide = Side.Left;
        public bool playerIsCrossing;

        [Header("Runtime")]
        public int randomSeed = 12345;

        System.Random _rng;
        float _dwell; bool _cooling; float _cooldownT; Side _campSide;

        // wave state
        bool _waveActive = true;
        float _nextPhaseAt;

        void Awake()
        {
            _rng = new System.Random(randomSeed);
            _campSide = playerSide;
            float now = Time.time;
            _waveActive = true;
            _nextPhaseAt = now + waveDuration;
        }

        public System.Random RNG => _rng;

        public void SetPlayerState(Side side, bool isCrossing)
        {
            if (side != playerSide)
            {
                playerSide = side;
                _campSide = side;
                _dwell = 0f;
            }
            if (isCrossing && !playerIsCrossing)
            {
                _cooling = true; _cooldownT = 1.2f;
            }
            playerIsCrossing = isCrossing;
        }

        void Update()
        {
            float dt = Time.deltaTime;

            // camping timer
            if (_cooling)
            {
                _cooldownT -= dt;
                if (_cooldownT <= 0f) { _cooling = false; _dwell = 0f; }
            }
            else _dwell += dt;

            // wave timer
            if (useWaves)
            {
                if (Time.time >= _nextPhaseAt)
                {
                    _waveActive = !_waveActive;
                    _nextPhaseAt = Time.time + (_waveActive ? waveDuration : breakDuration);
                }
            }
        }

        // ---- Public accessors ----
        public bool IsWaveActive => !useWaves || _waveActive;
        public float NextPhaseSwitchTime => useWaves ? _nextPhaseAt : Time.time;
        public float WaveRateMul => !useWaves ? 1f : (_waveActive ? Mathf.Max(0.01f, waveRateMultiplier) : 0f);

        public void GetBelowChannel(Channel ch, out float lambdaPerSec, out float rareMul)
        {
            float baseLambda = Mathf.Max(0f, belowSpawnsPerMin / 60f) * WaveRateMul;

            float rateLeftMul, rateRightMul, rareLeftMul, rareRightMul;
            Camping(out rateLeftMul, out rateRightMul, out rareLeftMul, out rareRightMul);

            if (ch == Channel.BelowLeft)
            {
                lambdaPerSec = baseLambda * rateLeftMul;
                rareMul = rareLeftMul;
            }
            else // BelowRight
            {
                lambdaPerSec = baseLambda * rateRightMul;
                rareMul = rareRightMul;
            }
        }

        public float GetBridgeLambdaPerSec()
        {
            return Mathf.Max(0f, bridgeSpawnsPerMin / 60f) * WaveRateMul;
        }

        public float SpeedMult => speedMultiplier;
        public float MinHeadway => minHeadwaySeconds;
        public float LeadTime => readableLeadTime;

        public float SampleExp(float lambdaPerSec)
        {
            double u = 1.0 - _rng.NextDouble();
            return (float)(-System.Math.Log(u) / System.Math.Max(1e-4, lambdaPerSec));
        }

        public void IncreaseWaveMultiplier(float amount)
        {
            waveRateMultiplier += amount;
        }

        public void IncreaseSpeedMultiplier(float amount)
        {
            speedMultiplier += amount;
        }

        public void IncreaseSpawnsPerMin(int amount)
        {
            belowSpawnsPerMin += amount;
            bridgeSpawnsPerMin += amount;
        }

        void Camping(out float rateLeftMul, out float rateRightMul, out float rareLeftMul, out float rareRightMul)
        {
            float tEff = Mathf.Max(0f, _dwell - graceSeconds);
            float k = Mathf.Clamp01(rampSeconds <= 0f ? 1f : (tEff / rampSeconds));

            float inhibit = 1f - maxInhibitPct * k;
            float boost   = 1f + maxBoostPct * k;
            float rareB   = 1f + maxRareBoost * k;

            rateLeftMul = rateRightMul = 1f;
            rareLeftMul = rareRightMul = 1f;

            if (_campSide == Side.Left)
            {
                rateLeftMul  = inhibit;
                rateRightMul = boost;
                rareRightMul = rareB;
            }
            else
            {
                rateRightMul = inhibit;
                rateLeftMul  = boost;
                rareLeftMul  = rareB;
            }
        }
    }
}
