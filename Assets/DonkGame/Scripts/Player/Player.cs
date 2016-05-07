using UnityEngine;
using System.Collections;
using InControl;

public class Player : MonoBehaviour
{
 
  public float fireSensitivity = 0.1f;
    
  public float speed = 100f;
  public int playerNumber = 1;
  public int gold = 0;

  private Vector3 aimDirection = Vector3.zero;

  private const int MAX_ABILITIES = 4;

  [Header ("Object Links")]
  [SerializeField]
  private GameObject model;
  [SerializeField]
  private GameObject turret;

  [SerializeField]
  Transform abilityContainer;

  [SerializeField]
  Ability[] Abilities = new Ability[MAX_ABILITIES + 1];
  [SerializeField]
  private GameObject defaultAbility;

  public LayerMask attackMask;
  [HideInInspector]
  public Team team = null;

  [SerializeField]
  public Renderer[] primaryMaterials;

  [HideInInspector]
  public InputDevice device = null;


  private int currentAbilityIndex = 0;

  void Start ()
  {
    for (int i = 0; i < primaryMaterials.Length; i++) {
      Renderer r = primaryMaterials [i];
      r.material = Instantiate<Material> (r.material);
      r.material.color = Game.Config.colors.playerColors [playerNumber - 1];
    }
    this.AddAbility (this.defaultAbility);
  }
	
  // Update is called once per frame
  void Update ()
  {
    if (device == null && InputManager.Devices.Count >= playerNumber) {
      //FIXME pass in device properly when drop in exists
      Debug.LogError ("Defaulted device for player " + playerNumber);
      device = InputManager.Devices [playerNumber - 1];
    }

    if (device) {
      UpdateActions ();
    }

    if (this.aimDirection != Vector3.zero) {
      this.model.transform.LookAt (this.transform.position + this.aimDirection);
    }
  }

  void FixedUpdate ()
  {
    if (device) {
      UpdateDirection ();
    }
  }
    
  public Rigidbody latchedObject = null;
  public float latchedMoveSpeed = 0.2f;
  void UpdateDirection ()
  {
    //Convert Incontrol vectors to movement directions in 3d space
    Vector3 moveDirection = new Vector3 (device.Direction.Vector.x, 0, device.Direction.Vector.y).normalized;
    this.aimDirection = new Vector3 (device.RightStick.Vector.x, 0f, device.RightStick.Vector.y);

    if (this.turret && aimDirection.sqrMagnitude > this.fireSensitivity) {                
      Quaternion quat = Quaternion.LookRotation (aimDirection, Vector3.up);
      this.turret.transform.rotation = quat;
    }

    if (latchedObject != null) {
		this.latchedObject.MovePosition(this.latchedObject.transform.position + moveDirection * Time.deltaTime * this.speed * this.latchedMoveSpeed);
    } else {
      //Move self
      this.GetComponent<Rigidbody> ().MovePosition(this.transform.position + moveDirection * Time.deltaTime * this.speed);
    }
  }

  void UpdateActions ()
  {
    //The following comments assume xbox 360 controls, but it should work cross platform

    if (device.Action1.WasPressed) {
      //A button          
      PrimaryAction ();
    }
    if (device.Action2.WasPressed) {
      //B button
		if (this.latchedObject) {
			this.UnlatchFromObject();
		}
			//TODO spit chicken, fuck this spacing
    }
    if (device.Action3.WasPressed || device.RightTrigger.WasPressed) {
      //X button
      this.Attack ();
    }
    if (device.Action4.WasPressed) {
      //Y Button
      Debug.LogError ("Player " + playerNumber + " Y");
    }  
  }


  private int AddAbility (GameObject abilityPrefab)
  {
    GameObject abilityInstance = (GameObject)GameObject.Instantiate (abilityPrefab);

    Ability a = abilityInstance.GetComponent<Ability> ();
    a.transform.SetParent (this.abilityContainer);
    a.Init (this.transform, this.attackMask, this);

    int openIndex = -1;
    for (int i = 0; i < this.Abilities.Length; i++) {
      if (this.Abilities [i] == null) {
        openIndex = i;
        break;
      }
    }
    if (openIndex < 0) {
      GameObject.Destroy (this.Abilities [currentAbilityIndex]);
      openIndex = currentAbilityIndex;
    }
    this.Abilities [openIndex] = a;

    return openIndex;
  }

  void Attack ()
  {
    if (this.Abilities [currentAbilityIndex] != null) {
      this.Abilities [currentAbilityIndex].Activate ();
    } else {
      this.Abilities [0].Activate ();
    }
  }


  public float tempActivateRadius = 3f;
  public Vector3 activateBoxCenter = new Vector3 (0f, 0f, 1.5f);
  public Vector3 activateBoxSize = Vector3.one * 3;

  void PrimaryAction ()
  {
    //TODO make this properly rotate
    Collider[] possibleObjects = Physics.OverlapBox (this.transform.position, activateBoxSize, this.transform.rotation);

    for (int i = 0; i < possibleObjects.Length; i++) {
      BigEgg egg = possibleObjects [i].GetComponent<BigEgg> ();
      if (egg != null) {
        LatchOnto(egg.GetComponent<Rigidbody>());
        break;
      }

      Chicken chicken = possibleObjects [i].GetComponent<Chicken> ();
      if (chicken != null) {
        Debug.LogError ("eating chicken");
        break;
      }
    }
  }

  void LatchOnto(Rigidbody obj) {
	this.latchedObject = obj;
	this.GetComponent<Rigidbody>().isKinematic = true;

    this.transform.SetParent(obj.transform);
  }

  void UnlatchFromObject() {
	this.latchedObject = null;
	this.GetComponent<Rigidbody>().isKinematic = false;
    this.transform.SetParent (this.team.transform);
  }

  void OnDrawGizmosSelected ()
  {
    Gizmos.color = Color.magenta;
    Gizmos.DrawSphere (this.transform.position + this.aimDirection, 0.3f);
    Gizmos.DrawWireCube (this.transform.position + this.activateBoxCenter, activateBoxSize);
  }
}
