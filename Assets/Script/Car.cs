using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarTheme
{
    public int Mat;
    public int aFront;
    public int aMiddle;
    public int aBack;
    public int wheelF;
    public int wheelB;
}

[System.Serializable]
public class LifeEvent : UnityEngine.Events.UnityEvent<int>
{

}

[RequireComponent(typeof(Rigidbody))]
public class Car : MonoBehaviour
{
    [Header("Particles")]
    public ParticleSystem changeParticle;
    public ParticleSystem flashParticle;

    //Componentes
    protected Rigidbody rb;

    private object[] Data;

    [Header("Car Set")]
    public GameObject car;
    public Material[] ChassisMaterials;
    public GameObject[] AddonFront;
    public GameObject[] AddonMiddle;
    public GameObject[] AddonBack;
    public GameObject[] WheelFront;
    public GameObject[] WheelBack;
    public CarTheme[] Theme;
    public CarTheme cT;

    //Normal [0], Stun[1] , Slow[2] , Disable[3] , Marked[4]
    enum States { Normal , Stun , Slow , Disable , Marked }
    States state = States.Normal;

    [Header("Car Config")]
    protected float nextFire = 0, //Tiempo antes del siguiente disparo
                  fireRate = 1.4f, //Tiempo entre disparos
                  maxAngle = 30; //Velocidad de rotacion del personaje

    protected float rotation;

    public float MaxMotorForce = 500f;
    public float MaxReverseForce = 250f;
    protected float motorForce = 0;
    public float BrakeForce = 50f;
    protected float actualBrakeForce = 0;

    [SerializeField]
    protected bool acelera = false,
                reversa = false,
                freno = false;

    public GameObject Weapon;
    public GameObject[] Bombs;

    int _lifes = 100;
    protected int Lifes
    {
        get { return _lifes; }
        set
        {
            _lifes = value;
            whenLifesChange.Invoke(50);
        }
    }

    [Header("Wheel Collider")]
    public WheelCollider FrontLeft;
    public WheelCollider FrontRight;
    public WheelCollider BackLeft;
    public WheelCollider BackRight;

    [Header("Config")]
    public bool PC = true; //Estoy jugando en pc?

    [HideInInspector]
    public GameObject FrontWheel;

    public int numeroRandom;


    [SerializeField]
    protected LifeEvent whenLifesChange;

    protected virtual void Start()
    {
        //Data = pv.InstantiationData;
        cT.Mat = (int)Data[0];
        cT.aFront = (int)Data[1];
        cT.aMiddle = (int)Data[2];
        cT.aBack = (int)Data[3];
        cT.wheelF = (int)Data[4];
        cT.wheelB = (int)Data[5];
        CarSet(cT);
    }


    private void FixedUpdate()
    {
        /*if (photonView.IsMine)
        {
            BackLeft.motorTorque = motorForce;
            BackRight.motorTorque = motorForce;
            FrontLeft.motorTorque = motorForce;
            FrontRight.motorTorque = motorForce;
            BackLeft.brakeTorque = actualBrakeForce;
            BackRight.brakeTorque = actualBrakeForce;
            FrontRight.brakeTorque = actualBrakeForce;
            FrontLeft.brakeTorque = actualBrakeForce;

            FrontLeft.transform.localEulerAngles = new Vector3(0, rotation, 0);
            FrontRight.transform.localEulerAngles = new Vector3(0, rotation, 0);
            FrontWheel.transform.localEulerAngles = new Vector3(0, rotation, 0);
            FrontLeft.steerAngle = rotation;
            FrontRight.steerAngle = rotation;
        }*/
    }


    protected void Shoot()
    {
        nextFire = Time.time + fireRate;

        GameObject bomb = Instantiate(Bombs[0], Weapon.transform.position, Weapon.transform.rotation) as GameObject;
        Physics.IgnoreCollision(bomb.GetComponent<Collider>(), GetComponent<BoxCollider>());
        Bullet bombScript = bomb.GetComponent<Bullet>();
        bombScript.Tirador = GetComponent<BoxCollider>();
    }

    private void OnCollisionEnter(Collision c)
    {
        switch (c.gameObject.tag)
        {
            case "Atomo": //Impacto
                // Calculate Angle Between the collision point and the player
                Vector3 dir = c.contacts[0].point - transform.position;
                // We then get the opposite (-Vector3) and normalize it
                dir = -dir.normalized;
                // And finally we add force in the direction of dir and multiply it by force. 
                // This will push back the player
                rb.AddForce(dir * 15000);
                break;
            case "Wood": //Stun
                if (state != States.Stun) StartCoroutine(Stun());
                break;
            case "PEM": //Deshabilita disparro
                if (state != States.Disable) StartCoroutine(Disabled());
                break;
            case "Normal": //Damage
                Lifes--;
                break;
            case "Pua": //Damage
                Lifes--;
                break;
            case "RedBomb": //Insta kill
                Lifes = 0;
                break;
            case "BuckBomb": //Vel Red
                if (state != States.Slow) StartCoroutine(VelRed());
                break;
            case "Vision": //Marcado
                break;

        }
    }

    private IEnumerator Stun()
    {
        state = States.Stun;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        yield return new WaitForSeconds(3f);
        state = States.Normal;
        rb.constraints = RigidbodyConstraints.None;
    }

    private IEnumerator VelRed()
    {
        state = States.Slow;
        yield return new WaitForSeconds(5f);
        state = States.Normal;
    }

    private IEnumerator Disabled()
    {
        state = States.Disable;
        yield return new WaitForSeconds(5f);
        state = States.Normal;
    }

    private IEnumerator Market()
    {
        state = States.Marked;
        yield return new WaitForSeconds(5f);
        state = States.Normal;
    }

    void DisableAllObjects()
    {
        foreach (GameObject gameObject in AddonFront)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }

        foreach (GameObject gameObject in AddonMiddle)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }

        foreach (GameObject gameObject in AddonBack)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }

        foreach (GameObject gameObject in WheelFront)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }

        foreach (GameObject gameObject in WheelBack)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
    }

    //Set a specific theme
    public void CarSet(CarTheme Set)
    {
        //switch material
        car.GetComponent<Renderer>().material = ChassisMaterials[Set.Mat];

        //remove all addons
        DisableAllObjects();

        //spawn particle
        changeParticle.Emit(30);
        flashParticle.Emit(2);

        //turn on random Addon in the Front
        if (Set.aFront > -1)
            AddonFront[Set.aFront].GetComponent<Renderer>().enabled = true;
        //turn on random Addon in the Middle
        if (Set.aMiddle > -1)
            AddonMiddle[Set.aMiddle].GetComponent<Renderer>().enabled = true;
        //turn on random Addon in the Back
        if (Set.aBack > -1)
            AddonBack[Set.aBack].GetComponent<Renderer>().enabled = true;

        //Wheels Front/ Back
        if (Set.wheelF > -1)
        {
            WheelFront[Set.wheelF].GetComponent<Renderer>().enabled = true;
            FrontWheel = WheelFront[Set.wheelF];
        }
        if (Set.wheelB > -1)
        {
            WheelBack[Set.wheelB].GetComponent<Renderer>().enabled = true;
        }
    }
}
