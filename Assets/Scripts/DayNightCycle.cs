using UnityEngine;

namespace DeloG
{
    public class DayNightCycle : MonoBehaviour
    {
        const float SecondsInDay = 120; // секунд в дне

        [SerializeField] Light Sun;
        [Range(0, 1)] public float Time = 0;

        float SunInitialIntensity;

        void Start() => SunInitialIntensity = Sun.intensity;

        void Update()
        {
            UpdateSun();
            Time = (Time + (UnityEngine.Time.deltaTime / SecondsInDay)) % 1;
        }

        void UpdateSun()
        {
            Sun.transform.localRotation = Quaternion.Euler((Time * 360f) - 90, 170, 0);

            float intensityMultiplier;
            if (Time <= 0.23f || Time >= 0.75f) intensityMultiplier = 0;
            else if (Time <= 0.25f) intensityMultiplier = Mathf.Clamp01((Time - 0.23f) * (1 / 0.02f));
            else if (Time >= 0.73f) intensityMultiplier = Mathf.Clamp01(1 - ((Time - 0.73f) * (1 / 0.02f)));
            else intensityMultiplier = 1;

            Sun.intensity = SunInitialIntensity * intensityMultiplier;
        }
    }
}