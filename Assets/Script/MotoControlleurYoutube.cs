using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotoControlleurYoutube : MonoBehaviour
{
    //Public variable
    public Rigidbody theRB;

    public float forwardAccel, ReverseAccel, Maxspeed, turnstrength;

    public LayerMask whatIsGround;
    public float GroundRayLength;
    public Transform GroundRayPoint;
    public float Gravity;
    public Vector3 offset;
    public float AirResistanceEnChute;
    public float AirResistanceAuSol;
    //public Vector3 rotationDrift;
    public float AngleDrift;
    public float forceDrift;
    public float rotationDriftSpeed;
    //Local variable
    float speedInput, turnInput;
    bool drifting=false;
    bool grounded;
   

    void Start()
    {
        theRB.transform.parent = null;
    }

    private void Update()
    {
        speedInput = 0;
        float valueInput = Input.GetAxis("Acceleration");
        if (valueInput != 0) 
        {
            if (valueInput > 0)
            {
                speedInput = valueInput * forwardAccel;
            }
            if (valueInput < 0)
            {
                speedInput = valueInput * ReverseAccel;
            }
        }
        turnInput = Input.GetAxis("HorizontalManette");
        if (grounded&&!drifting)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnstrength * Time.deltaTime * Input.GetAxis("Acceleration"), 0f));
        }
        

        transform.position = theRB.transform.position+offset;
    }

    void FixedUpdate()
    {
        
        grounded = false;
        RaycastHit hit;
        Debug.DrawRay(GroundRayPoint.position, -transform.up * GroundRayLength,Color.magenta);
        if (Physics.Raycast(GroundRayPoint.position, -transform.up,out hit,GroundRayLength,whatIsGround))
        {
            grounded = true;
            print("ground");
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal)*transform.rotation;
        }
        if (grounded)
        {
            if (speedInput != 0 && !drifting)
            {
                theRB.AddForce(transform.forward * speedInput);
            }
             if (Input.GetAxisRaw("Drift")==1 )
            {
                
                Drift();
                drifting = true;
            }
            else if (Input.GetAxisRaw("Drift") == 0 && speedInput != 0 && drifting)
            {
                stopDrifting();
            }
            theRB.drag = AirResistanceAuSol;

        }
        else 
        {
            theRB.drag = AirResistanceEnChute;
            theRB.AddForce(Vector3.down * Gravity);
        
        }
        
        


    }

    void Drift() 
    {
        //desynchroniser la camera
        //changeDrag(1,1);
        print("Lance Le Drift");
        // rotate le véhicule
        print(turnInput);
        forwardAccel = 0;
        float turn = (turnInput / Mathf.Abs(turnInput));
        Vector3 calculateDirection = transform.forward + new Vector3(Mathf.Sin((AngleDrift * turnInput * Mathf.Deg2Rad)), 0, Mathf.Cos((AngleDrift * turnInput * Mathf.Deg2Rad)));
        //Debug.DrawRay(transform.position, calculateDirection.normalized * 10, Color.red);
        
        Vector3 CalculateRotation = new Vector3(0, AngleDrift * turn, 0) + transform.rotation.eulerAngles;
        
        //revoir rotation
        transform.rotation *= Quaternion.AngleAxis(AngleDrift * turn * Time.deltaTime *rotationDriftSpeed, transform.up);
        
        
        //Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(CalculateRotation) * transform.rotation,rotationDriftSpeed*Time.deltaTime);
        //Quaternion.Lerp(transform.rotation, Quaternion.Euler(CalculateRotation) * transform.rotation, rotationDriftSpeed);
        //Quaternion.FromToRotation(transform.forward,calculateDirection );
        //Quaternion.Euler(CalculateRotation)*transform.rotation

        
        // Ajouter une force sur le coté 
        Vector3 DirectionForceDrift = transform.right*Input.GetAxis("HorizontalManette");//revoir la direction de la force 
        Debug.DrawRay(transform.position, DirectionForceDrift * 5, Color.cyan);
        theRB.AddForce(DirectionForceDrift.normalized*forceDrift, ForceMode.Force);



        // Ajouter une force vers l'avans
        theRB.AddForce(transform.forward * speedInput);
        Debug.DrawRay(transform.position, transform.forward * 5, Color.blue);

    }
    void stopDrifting() 
    {
        print("Stop Le Drift");
        forwardAccel = 8000;
        drifting = false;
    }


    #region fonction Gagne Temps
    void changeDrag(float NewDrag) 
    {
        theRB.drag = NewDrag;
    }
    void changeDrag(float NewDrag,float NewAngularDrag) 
    {
        theRB.drag = NewDrag;
        theRB.angularDrag = NewAngularDrag;
    
    }

    #endregion
}
