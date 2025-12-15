using UnityEngine;
using UnityEngine.XR.Content.Interaction;

namespace UnityEngine.XR.Content.Interaction
{
    [RequireComponent(typeof(XRKnob))]
    public class KnobController : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("The GameObject representing the gas burner flame to be enabled/disabled.")]
        public GameObject gasBurnerFlame;

        [Header("Settings")]
        [Tooltip("Number of full rotations required to turn on the gas (default: 3).")]
        public int rotationsToEnable = 3;

        [Tooltip("Threshold for detecting a full rotation (0.9 means crossing from 0.9 to near 0, or vice versa).")]
        [Range(0.7f, 0.95f)]
        public float rotationThreshold = 0.9f;

        [Header("Debug Info (Read Only)")]
        [Tooltip("Current rotation count. Positive = clockwise, Negative = counter-clockwise.")]
        [SerializeField]
        private int currentRotationCount = 0;

        [Tooltip("Current knob value for debugging.")]
        [SerializeField]
        private float currentValue = 0f;

        private XRKnob knob;
        private float previousValue;
        private bool isFlameOn = false;

        void Start()
        {
            // Get the XRKnob component
            knob = GetComponent<XRKnob>();

            if (knob != null)
            {
                // Check if the knob is set to unclamped motion (required for multiple rotations)
                if (knob.clampedMotion)
                {
                    Debug.LogWarning("KnobController: XRKnob is set to 'Clamped Motion'. " +
                                   "For multiple rotations, set 'Clamped Motion' to false in the XRKnob component.", this);
                }

                // Subscribe to the knob's value change event
                knob.onValueChange.AddListener(HandleKnobValueChanged);

                // Initialize previous value
                previousValue = knob.value;
                currentValue = knob.value;
            }
            else
            {
                Debug.LogError("KnobController: XRKnob component not found on this GameObject. The controller will not function.", this);
            }

            // Validate references
            if (gasBurnerFlame == null)
            {
                Debug.LogWarning("KnobController: Gas Burner Flame GameObject is not assigned.", this);
            }
            else
            {
                // Ensure flame starts disabled
                gasBurnerFlame.SetActive(false);
                isFlameOn = false;
            }
        }

        void OnDestroy()
        {
            // Unsubscribe from the event when destroyed to prevent memory leaks
            if (knob != null)
            {
                knob.onValueChange.RemoveListener(HandleKnobValueChanged);
            }
        }

        private void HandleKnobValueChanged(float newValue)
        {
            currentValue = newValue;

            // Detect full rotation by checking if we crossed the boundary
            // Clockwise: value goes from high (>threshold) to low (<1-threshold)
            // Counter-clockwise: value goes from low (<1-threshold) to high (>threshold)

            // Clockwise rotation detection (turning right)
            if (previousValue > rotationThreshold && newValue < (1f - rotationThreshold))
            {
                currentRotationCount++;
                Debug.Log($"Clockwise rotation detected! Total rotations: {currentRotationCount}");
                CheckFlameState();
            }
            // Counter-clockwise rotation detection (turning left)
            else if (previousValue < (1f - rotationThreshold) && newValue > rotationThreshold)
            {
                currentRotationCount--;
                Debug.Log($"Counter-clockwise rotation detected! Total rotations: {currentRotationCount}");
                CheckFlameState();
            }

            previousValue = newValue;
        }

        private void CheckFlameState()
        {
            if (gasBurnerFlame == null)
            {
                Debug.LogWarning("KnobController: Cannot control flame, gasBurnerFlame is null.");
                return;
            }

            // Enable flame when we reach the required number of rotations
            if (currentRotationCount >= rotationsToEnable && !isFlameOn)
            {
                gasBurnerFlame.SetActive(true);
                isFlameOn = true;
                Debug.Log($"Gas burner flame ENABLED after {currentRotationCount} rotations.");
            }
            // Disable flame when we return to original position (0 rotations or less)
            else if (currentRotationCount <= 0 && isFlameOn)
            {
                gasBurnerFlame.SetActive(false);
                isFlameOn = false;
                currentRotationCount = 0; // Clamp to 0 to prevent negative rotations
                Debug.Log("Gas burner flame DISABLED - returned to original position.");
            }
        }

        /// <summary>
        /// Public method to manually reset the rotation count (useful for testing or resetting the cooker)
        /// </summary>
        public void ResetRotationCount()
        {
            currentRotationCount = 0;
            if (gasBurnerFlame != null)
            {
                gasBurnerFlame.SetActive(false);
                isFlameOn = false;
            }
            Debug.Log("KnobController: Rotation count reset to 0, flame disabled.");
        }

        /// <summary>
        /// Public method to get the current rotation count
        /// </summary>
        public int GetRotationCount()
        {
            return currentRotationCount;
        }

        /// <summary>
        /// Public method to check if the flame is currently on
        /// </summary>
        public bool IsFlameOn()
        {
            return isFlameOn;
        }
    }
}