using BogGames.Gameplay;
using BogGames.Variables;
using UnityEngine;

namespace BogGames.Controls
{
    public class StepMovementController : MonoBehaviour
    {
        [Header("Movement")]
        [Tooltip("Distance moved each key press (in meters).")]
        public float stepDistance = 1f;
        [Tooltip("Minimum time between steps (in seconds).")]
        public float stepCooldown = 0.5f;

        [Header("Boundaries")]
        [Tooltip("Minimum Z position the player can step to.")]
        public float minZ = -5f;
        [Tooltip("Maximum Z position the player can step to.")]
        public float maxZ = 5f;

        [Header("Model Facing")]
        [Tooltip("Child transform that visually represents the player.")]
        public Transform modelRoot;
        [Tooltip("Yaw rotation when stepping forward (+Z).")]
        public float forwardYaw = 0f;
        [Tooltip("Yaw rotation when stepping backward (-Z).")]
        public float backwardYaw = 180f;

        [Header("Animation (optional)")]
        public Animator animator;
        public string stepTrigger = "StepSuccess";
        public string blockTrigger = "BlockStep";

        [Header("Variables")]
        public BogSideVariable playerSide;
        public BogBooleanVariable playerCrossing;
        public BogFloatVariable stepDecrease;
        public BogFloatVariable stepIncrease;

        private InputSystem_Actions controls;
        private float lastStepTime;
        private float midpoint;
        private int direction = 1;
        private Side currentSide = Side.Right;
        private bool isCrossing = true;

        private void Awake()
        {
            controls = new InputSystem_Actions();

            controls.Player.StepForward.performed += ctx => TryStep(1);
            controls.Player.StepBackward.performed += ctx => TryStep(-1);

            midpoint = transform.position.z;
        }

        private void OnEnable() => controls.Player.Enable();
        private void OnDisable() => controls.Player.Disable();

        private void Update()
        {
            if (transform.position.z > maxZ)
            {
                isCrossing = false;
                currentSide = Side.Right;
            }
            else if (transform.position.z < minZ)
            {
                isCrossing = false;
                currentSide = Side.Left;
            }
            else
            {
                currentSide = Side.Middle;
                isCrossing = true;
            }

            if (playerCrossing != null)
                playerCrossing.Value = isCrossing;

            if (playerSide != null)
                playerSide.Value = currentSide;
        }

        private void FaceDirection(float yaw)
        {
            if (modelRoot != null)
            {
                Vector3 e = modelRoot.eulerAngles;
                e.y = yaw;
                modelRoot.eulerAngles = e;
            }
            else
            {
                Vector3 e = transform.eulerAngles;
                e.y = yaw;
                transform.eulerAngles = e;
            }
        }

        public void TryStep(int direction)
        {
            float currentCooldown = stepCooldown + (stepIncrease?.Value ?? 0) - (stepDecrease?.Value ?? 0);

            if (Time.time - lastStepTime < currentCooldown)
                return;

            lastStepTime = Time.time;

            FaceDirection(direction > 0 ? forwardYaw : backwardYaw);

            if (direction > 0 && this.transform.position.z! < maxZ)
                StepSuccess(true);

            if (direction < 0 && this.transform.position.z! > minZ)
                StepSuccess(false);
        }

        public void TryStepNoInput()
        {
            float currentCooldown = stepCooldown + (stepIncrease?.Value ?? 0) - (stepDecrease?.Value ?? 0);

            if (Time.time - lastStepTime < currentCooldown)
                return;

            lastStepTime = Time.time;

            if (transform.position.z > maxZ)
                direction = -1;
            if (transform.position.z < minZ)
                direction = 1;

            FaceDirection(direction > 0 ? forwardYaw : backwardYaw);

            if (direction > 0)
            {
                StepSuccess(true);
            }

            if (direction < 0)
            {
                StepSuccess(false);
            }
        }

        protected void StepSuccess(bool forward)
        {
            if (forward)
                transform.position += Vector3.forward * stepDistance;
            else
                transform.position += Vector3.back * stepDistance;

            if (animator && !string.IsNullOrEmpty(stepTrigger))
                animator.SetTrigger(stepTrigger);
        }

        protected void StepBlocked()
        {
            if (animator && !string.IsNullOrEmpty(blockTrigger))
                animator.SetTrigger(blockTrigger);
        }
    }
}
