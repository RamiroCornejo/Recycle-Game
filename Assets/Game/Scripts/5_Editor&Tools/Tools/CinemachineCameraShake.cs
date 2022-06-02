using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineCameraShake : MonoBehaviour
{

    #region VARS
    public enum CameraShakeType { hit_enemy, eartquake }
    public static CinemachineCameraShake instance;
    CinemachineBasicMultiChannelPerlin perlin; 
    CinemachineVirtualCamera mycam;
    float time;
    float timer;
    bool anim;
    bool use_lerp;
    Dictionary<string, ShakeProfile> profiles = new Dictionary<string, ShakeProfile>();
    #endregion

    #region PLANTILLAS
    // porque lo harcodeo? 
    // porque cuando tire esto en otro lado voy a perder los valores que me gustan
    public const string HIT_ENEMY = "Hit_Enemy";
    public const string EARTQUAKE = "Eartquake";
    void DesignProfiles()
    {
        ShakeProfile hit_enemy = new ShakeProfile();
        hit_enemy.Name = HIT_ENEMY;
        hit_enemy.intensity = 1f;
        hit_enemy.frecuency = 3;
        hit_enemy.time = 0.1f;
        hit_enemy.offset = Vector3.zero;
        hit_enemy.use_lerp_to_disapear = false;
        profiles.Add(HIT_ENEMY, hit_enemy);

        ShakeProfile eartquake = new ShakeProfile();
        eartquake.Name = EARTQUAKE;
        eartquake.intensity = 2f;
        eartquake.frecuency = 1f;
        eartquake.time = 2f;
        eartquake.offset = Vector3.zero;
        eartquake.use_lerp_to_disapear = true;
        profiles.Add(EARTQUAKE, eartquake);
    }
    #endregion

    #region PUBLIC
    public static void Shake(CameraShakeType type)
    {
        if (type == CameraShakeType.hit_enemy) instance.BeginShake(instance.profiles[HIT_ENEMY]);
        if (type == CameraShakeType.eartquake) instance.BeginShake(instance.profiles[EARTQUAKE]);
    }
    public static void Shake(float time, float intensity, float frecuency, Vector3 offset, bool use_lerp_to_disapear)
    {
        instance.BeginShake(time, intensity, frecuency, offset, use_lerp_to_disapear);
    }
    #endregion

    #region PRIVATE
    private void Awake() { instance = this; }
    private void Start()
    {
        mycam = GetComponent<CinemachineVirtualCamera>();
        perlin = mycam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        DesignProfiles();
    }
    void BeginShake(ShakeProfile profile)
    {
        anim = true;
        timer = 0;
        this.time = profile.time;
        use_lerp = profile.use_lerp_to_disapear;
        Amplitude(profile.intensity);
        Frecuency(profile.frecuency);
        Offset(profile.offset);
    }
    void BeginShake(float time, float intensity, float frecuency, Vector3 offset, bool use_lerp_to_disapear)
    {
        anim = true;
        timer = 0;
        this.time = time;
        use_lerp = use_lerp_to_disapear;
        Amplitude(intensity);
        Frecuency(frecuency);
        Offset(offset);
    }

    private void Update()
    {
        if (anim)
        {
            if (timer < time)
            {
                timer = timer + 1 * Time.deltaTime;

                if (use_lerp)
                {

                }
            }
            else
            {
                Amplitude(0);
                timer = 0;
                anim = false;
            }
        }
    }

    void Amplitude(float val) => perlin.m_AmplitudeGain = val;
    void Frecuency(float val) => perlin.m_FrequencyGain = val;
    void Offset(Vector3 val) => perlin.m_PivotOffset = val;

    internal class ShakeProfile
    {
        public string Name;
        public float intensity;
        public float frecuency;
        public float time;
        public Vector3 offset;
        public bool use_lerp_to_disapear;
    }

    #endregion
}