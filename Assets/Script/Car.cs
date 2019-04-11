using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

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

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PhotonView))]
public class Car : MonoBehaviourPun, IPunObservable
{
    [Header("Particles")]
    public ParticleSystem changeParticle;
    public ParticleSystem flashParticle;

    //Componentes
    protected Rigidbody rb;
    protected PhotonView pv;

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

    [Header("Car Config")]
    //Normal [0], Stun[1] , Slow[2] , Disable[3] , Marked[4]
    public bool[] state;

    protected float nextFire = 0, //Tiempo antes del siguiente disparo
                  fireRate = 1.4f, //Tiempo entre disparos
                  maxAngle = 30; //Velocidad de rotacion del personaje

    protected float rotation;

    public float motorForce = 400f;
    public float reverseForce = 200f;
    public float brakeForce = 50f;

    [SerializeField]
    protected bool acelera = false,
                reversa = false,
                freno = false;

    public GameObject Weapon;

    public GameObject[] Bombs;

    public int life = 5; //Vida

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

    public void Start()
    {
        Data = pv.InstantiationData;
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
        if (photonView.IsMine)
        {
            if (PC)
            {
                if (acelera)
                {
                    BackLeft.motorTorque = motorForce;
                    BackRight.motorTorque = motorForce;
                    FrontLeft.motorTorque = motorForce;
                    FrontRight.motorTorque = motorForce;
                    BackLeft.brakeTorque = 0;
                    BackRight.brakeTorque = 0;
                    FrontLeft.brakeTorque = 0;
                    FrontRight.brakeTorque = 0;
                }
                else if (reversa)
                {
                    BackLeft.motorTorque = -reverseForce;
                    BackRight.motorTorque = -reverseForce;
                    FrontLeft.motorTorque = -reverseForce;
                    FrontRight.motorTorque = -reverseForce;
                    BackLeft.brakeTorque = 0;
                    BackRight.brakeTorque = 0;
                    FrontLeft.brakeTorque = 0;
                    FrontRight.brakeTorque = 0;
                }
                else
                {
                    BackLeft.brakeTorque = brakeForce;
                    BackRight.brakeTorque = brakeForce;
                    FrontRight.brakeTorque = brakeForce;
                    FrontLeft.brakeTorque = brakeForce;
                    FrontLeft.motorTorque = 0;
                    FrontRight.motorTorque = 0;
                    BackLeft.motorTorque = 0;
                    BackRight.motorTorque = 0;
                }
            }

            FrontLeft.transform.localEulerAngles = new Vector3(0, rotation, 0);
            FrontRight.transform.localEulerAngles = new Vector3(0, rotation, 0);
            FrontWheel.transform.localEulerAngles = new Vector3(0, rotation, 0);
        }
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
                if (!isStun) StartCoroutine(Stun());
                break;
            case "PEM": //Deshabilita disparro
                if (!shootDisable) StartCoroutine(Disabled());
                break;
            case "Normal": //Damage
                life--;
                break;
            case "Pua": //Damage
                break;
            case "RedBomb": //Insta kill
                break;
            case "BuckBomb": //Vel Red
                break;
            case "Vision": //Marcado
                break;

        }
    }

    bool isStun = false;
    private IEnumerator Stun()
    {
        isStun = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        yield return new WaitForSeconds(3f);
        isStun = false;
        rb.constraints = RigidbodyConstraints.None;
    }

    private IEnumerator VelRed()
    {
        yield return new WaitForSeconds(5f);
    }

    bool shootDisable = false;
    private IEnumerator Disabled()
    {
        shootDisable = true;
        yield return new WaitForSeconds(5f);
        shootDisable = false;
    }

    private IEnumerator Market()
    {
        yield return new WaitForSeconds(5f);
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

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
        }
        else
        {
        }
    }
}
