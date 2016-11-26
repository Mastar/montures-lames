
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CharacterDemoController : MonoBehaviour 
{
	Animator animator;
    CharacterController cctrl;
	public int 				WeaponState=0;//unarmed, 1H, 2H, bow, dual, pistol, rifle, spear and ss(sword and shield)

	float				rotateSpeed = 10.0f; //used to smooth out turning
	public bool rightButtonDown=false;//we use this to "skip out" of consecutive right mouse down...

    public List<Collider> weapons;
    
    // Use this for initialization
    void Start () 
	{
        cctrl = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();//need this...
	}

    // Update is called once per frame
    void Update()
    {

        float forward = Input.GetAxis("Vertical");
        float side = Input.GetAxis("Horizontal");
        float rotY = Input.GetAxis("Mouse X");

        transform.Rotate(0, rotY* rotateSpeed, 0);

        // UI goal	
        /*
        if (Input.GetAxis("Horizontal") != 0)
        {
            Vector3 deltaTarget;
            if (Input.GetAxis("Horizontal") < 0)
            {
                deltaTarget = transform.position+ transform.right*transform.rot;
            }
            else {
                deltaTarget = transform.position + new Vector3(0, 0, -1);
            }

            Input.mousePosition

            lookAtPos = deltaTarget.normalized * 2.0f;
            lookAtPos.y = transform.position.y;

            Quaternion tempRot = transform.rotation;    //save current rotation
            transform.LookAt(lookAtPos);
            Quaternion hitRot = transform.rotation;     // store the new rotation
                                                        // now we slerp orientation
            transform.rotation = Quaternion.Slerp(tempRot, hitRot, Time.deltaTime * rotateSpeed);
            Debug.Log(tempRot + "             " + hitRot);

            wasAttacking = false;
        }
        */
        if (Input.GetAxis("Vertical") != 0)
        {
            animator.SetBool("Idling", false);
        }
        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0) {
            animator.SetBool("Idling", true);
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (weapons[WeaponState]) {
                weapons[WeaponState].enabled = false;
            }
            
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                WeaponState = WeaponState == 8 ? 0 : WeaponState+1;
            }
            else
            {
                WeaponState = WeaponState == 0 ? 8 : WeaponState-1;
            }
            if (weapons[WeaponState])
            {
                weapons[WeaponState].enabled = true;
            }
            //animator.applyRootMotion = false;
            animator.SetInteger("WeaponState", WeaponState);// probably would be better to check for change rather than bashing the value in like this
            //animator.applyRootMotion = true;
        }

        if (!cctrl.isGrounded) {
            //transform.position = transform.position - transform.up;
        }

        switch (Input.inputString)//get keyboard input, probably not a good idea to use strings here...Garbage collection problems with regards to local string usage are known to happen
		{						 //the garbage collection memory problem arises from local alloction of memory, and not freeing it up efficiently
			case "0":
				WeaponState = 0;//unarmed
				break;
			case "1":
				WeaponState = 1;//1H one handed weapon
				break;
			case "2":
				WeaponState = 2;//2H two handed weapon(longsword or heavy axe)
				break;
			case "3":
				WeaponState = 3;//bow
				break;
			case "4":
				WeaponState = 4;//dual weild(short swords, light axes)
				break;
			case "5":
				WeaponState = 5;//pistol
				break;
			case "6":
				WeaponState = 6;//rifle
				break;
			case "7":
				WeaponState = 7;//spear
				break;
			case "8":
				WeaponState = 8;//Sword and Shield
				break;
			
			case "p":
				animator.SetTrigger("Pain");//the animator controller will detect the trigger pain and play the pain animation
				break;
			case "a":
				animator.SetInteger("Death", 1);//the animator controller will detect death=1 and play DeathA
				break;
			case "b":
				animator.SetInteger("Death", 2);//the animator controller will detect death=2 and play DeathB
				break;
			case "c":
				animator.SetInteger("Death", 3);//the animator controller will detect death=3 and play DeathC
				break;
			case "n":
				animator.SetBool("NonCombat", true);//the animator controller will detect this non combat bool, and go into a non combat state "in" this weaponstate
				break;
			default:
				break;
		}
		

		if(Input.GetButton("Fire1"))
		{
            if (rightButtonDown != true)// was it previously down? if so we are already using "USE" bailout (we don't want to repeat attacks 800 times a second...just once per press please
            {// RUNGY addin enemy collider "test for nearest hit" here, they need to actually take precednce over the ground
                animator.SetTrigger("Use");//tell mecanim to do the attack animation(trigger)
                animator.SetBool("Idling", true);//stop moving
                rightButtonDown = true;//right button was not down before, mark it as down so we don't attack 800 frames a second 
            }
		}

		
		if (Input.GetButtonUp("Fire1"))//ok, we can clear the right mouse button and use it for the next attack
		{
			if (rightButtonDown == true)
			{
				rightButtonDown = false;
			}
		}
	}
	
	void OnGUI()
	{
		string tempString = "p=pain abc=deaths 12345678 0=change weapons";
		GUI.Label (new Rect (10, 5,1000, 20), tempString);
	}
}
