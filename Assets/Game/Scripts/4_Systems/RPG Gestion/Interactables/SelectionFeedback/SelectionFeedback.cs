using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionFeedback : MonoBehaviour
{
    public static SelectionFeedback instance;
    [SerializeField] BeatAnimation beat;
    [SerializeField] ParticleSystem velocity_emmiter;
    [SerializeField] ParticleSystem constant_emmiter;
    float timer = 0f;
    bool anim;
    float max_time_by_curve = 0f;
    public AnimationCurve anim_curve;
    Vector3 newPos;
    Vector3 veryfar = new Vector3(10000,10000,10000);
    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(this.gameObject); throw new System.Exception("ERROR: No pueden haber dos Seleccion Feedback"); }
    }
    private void Start()
    {
        max_time_by_curve = anim_curve.keys[anim_curve.length-1].time;
    }

    public static void SetPosition(Vector3 position, bool isNew = false) => instance.SetNewPosition(position, isNew);
    public static void Hide() => instance.HideFeedbacks();

    void Animation(float val)
    {
        this.transform.position = Vector3.Lerp(this.transform.position, newPos, val);
    }

    void SetNewPosition(Vector3 position, bool isNew = false)
    {
        newPos = position;
        if (isNew) this.transform.position = Character.instance.transform.position + Vector3.up;
        anim = true;
        timer = 0;
        beat.BeginBeatAnimation();
        velocity_emmiter.Play();
        constant_emmiter.Play();
    }

    void HideFeedbacks()
    {
        anim = false;
        timer = 0;
        this.transform.position = veryfar;
        beat.StopBeatAnimation();
        velocity_emmiter.Stop();
        constant_emmiter.Stop();
    }

    private void Update()
    {
        if (anim)
        {
            if (timer < max_time_by_curve)
            {
                timer = timer + 1 * Time.deltaTime;
                Animation(anim_curve.Evaluate(timer));
            }
            else
            {
                timer = 0;
                anim = false;
            }
        }
    }
}
