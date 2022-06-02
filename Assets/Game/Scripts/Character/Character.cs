using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static Character instance;
    List<CharacterTools> tools = new List<CharacterTools>() { CharacterTools.Pikes };

    [SerializeField] bool hasShovel = false;
    [SerializeField] bool hasGloves = false;
    private void Awake()
    {
        instance = this;
        interactor.InitializeInteractor();
    }

    private void Start()
    {
        if(hasShovel)AddTool(CharacterTools.Shovel);
        if (hasGloves) AddTool(CharacterTools.Gloves);
    }

    public void AddTool(CharacterTools tool)
    {
        if (!tools.Contains(tool)) { tools.Add(tool); UIManager.instance.EquipTool(tool); }
    }

    #region My Statics
    public static void TrackInput(bool tracking_value) => instance.TrackInputPrivate(tracking_value);
    #endregion

    void TrackInputPrivate(bool b)
    {
        isTracking = b;
        dir = new Vector3(0, 0, 0);
        view.Walk(dir != Vector3.zero);
        myRig.velocity = Vector3.zero;
    }

    public bool CheckTools(CharacterTools tool) { attackType = (int)tool; return tools.Contains(tool); }

    [SerializeField] Transform _toLookAt;
    [SerializeField] bool isTracking = true;
    [SerializeField] Rigidbody myRig;
    [SerializeField] float speed;
    [SerializeField] HitSensor hitSensor;
    [SerializeField] GenericInteractor interactor = null;
    [SerializeField] CharacterView view = null;
    public UiBar myBar;

    Vector3 dir;
    int attackType = 0;
    #region TICK
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.T))
        {
            AddTool(CharacterTools.Shovel);
            AddTool(CharacterTools.Gloves);
        }

        if (!isTracking) return;

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");


        dir = new Vector3(x, 0, y);
        view.Walk(dir != Vector3.zero);

        view.Flip(x, y);

        hitSensor.UpdateDirection(x, y, view.Flipped, transform.position);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool b = hitSensor.ExecuteHit(x, y, view.Flipped, transform.position);
            view.Attack(true, b ? attackType : 0);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            ReleaseAttack();
        }

        if (Input.GetKeyDown(KeyCode.E))
            interactor.Execute();

        //bla bla
    }

    public void ReleaseAttack()
    {
        hitSensor.ReleaseHit(dir.x, dir.y, view.Flipped, transform.position);
        view.Attack(false, 0);
    }
    private void FixedUpdate()
    {
        if (!isTracking) return;

        myRig.velocity = dir * speed;
    }
    #endregion
}
