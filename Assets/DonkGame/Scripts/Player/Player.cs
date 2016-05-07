using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class Player : MonoBehaviour
{
 
  public float fireSensitivity = 0.1f;
    
  public float currentSpeed = 100f;
  public float maxSpeed = 100f;
  public float minSpeed = 1f;
  public int playerNumber = 1;
  public int gold = 0;
  public List<Chicken> chickensInStomach = new List<Chicken>();
  public int maxChickensInStomach;

  private Vector3 aimDirection = Vector3.zero;

  private const int MAX_ABILITIES = 4;

	private int currentAbilityIndex = 0;
	[HideInInspector]
	public bool dead = false;

  [Header ("Object Links")]
  [SerializeField]
  private GameObject model;
  [SerializeField]
  private GameObject turret;

  [SerializeField]
  Transform abilityContainer;

	[SerializeField]
	public Image healthImage;

  [SerializeField]
  Ability[] Abilities = new Ability[MAX_ABILITIES + 1];
  [SerializeField]
  private GameObject defaultAbility;
  [SerializeField]
  private GameObject crossbow;

  public LayerMask attackMask;
  [HideInInspector]
  public Team team = null;

  [SerializeField]
  public Renderer[] primaryMaterials;
	[SerializeField]
	private SkinnedMeshRenderer fatSuit;

  [SerializeField]
  public BoxCollider InteractTrigger;

  [HideInInspector]
  public InputDevice device = null;

	public Vector3 spawnPoint;

    private Color _playerColor;
    public Color PlayerColor {
        get {
            return _playerColor;
        }
        set {
            _playerColor = value;
            for (int i = 0; i < primaryMaterials.Length; i++) {
                Renderer r = primaryMaterials[i];
                r.material = Instantiate<Material>(r.material);
                r.material.color = _playerColor;
				this.healthImage.color = _playerColor;
            }
        }
    }

  void Start ()
  {
    this.AddAbility(this.defaultAbility);
    this.AddAbility(this.crossbow);

    this.currentSpeed = this.maxSpeed;
  }
	
  // Update is called once per frame
  void Update ()
  {
    if (device == null && InputManager.Devices.Count >= playerNumber) {
      //FIXME pass in device properly when drop in exists
		Debug.LogWarning ("Defaulted device for player " + playerNumber);
      device = InputManager.Devices [playerNumber - 1];
    }

    if (device && !dead) {
      UpdateActions ();
    }

    if (this.aimDirection != Vector3.zero) {
      this.transform.LookAt (this.transform.position + this.aimDirection);
    }

		if (!dead && this.transform.position.y < -20) {
			this.Die();
		}
			
		if (this.fatSuit && this.fatSuit.sharedMesh.blendShapeCount > 0) {
			this.fatSuit.SetBlendShapeWeight (0, (float)this.chickensInStomach.Count / this.maxChickensInStomach);
		}
  }

  void FixedUpdate ()
  {
    if (device && !dead) {
      UpdateDirection ();
    }
  }
    
  public Rigidbody latchedObject = null;
  public float latchedMoveSpeed = 0.2f;
  void UpdateDirection ()
  {
    //Convert Incontrol vectors to movement directions in 3d space
    Vector3 moveDirection = new Vector3 (device.Direction.Vector.x, 0, device.Direction.Vector.y).normalized;
    this.aimDirection = moveDirection;

    if (this.turret && aimDirection.sqrMagnitude > this.fireSensitivity) {                
      Quaternion quat = Quaternion.LookRotation (aimDirection, Vector3.up);
      this.turret.transform.rotation = quat;
    }

    if (latchedObject != null) {
		float moveScale = latchedMoveSpeed;

		BigEgg egg = latchedObject.GetComponent<BigEgg>();
		if (egg != null) {
			moveScale = moveScale * egg.numLatchers;
		}

		this.latchedObject.MovePosition(this.latchedObject.position + moveDirection * Time.deltaTime * this.currentSpeed * moveScale);
    } else {
      //Move self
	  this.GetComponent<Rigidbody> ().MovePosition(this.transform.position + moveDirection * Time.deltaTime * this.currentSpeed);
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
		} else {
			this.SpitUp();
		}
    }
    if (device.Action3.WasPressed || device.RightTrigger.WasPressed) {
      //X button
      this.Attack ();
    }
    if (device.Action4.WasPressed) {
      //Y Button
      this.SwitchWeapons();
    }  
  }


  private int AddAbility (GameObject abilityPrefab)
  {
    GameObject abilityInstance = (GameObject)GameObject.Instantiate (abilityPrefab);

    Ability a = abilityInstance.GetComponent<Ability> ();
    a.transform.SetParent (this.abilityContainer, false);
    a.transform.position = this.abilityContainer.transform.position;
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

	this.currentAbilityIndex = openIndex;

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

  void PrimaryAction ()
  {

    Collider[] possibleObjects = Physics.OverlapBox(this.InteractTrigger.bounds.center, this.InteractTrigger.bounds.extents);

    for (int i = 0; i < possibleObjects.Length; i++) {
      BigEgg egg = possibleObjects [i].gameObject.GetComponent<BigEgg> ();
      if (egg != null) {
        LatchOnto(egg.GetComponent<Rigidbody>());
        ++egg.numLatchers;
        break;
      }

      Chicken chicken = possibleObjects [i].gameObject.GetComponent<Chicken> ();
      if (chicken != null && (this.chickensInStomach.Count < this.maxChickensInStomach)) {
        this.chickensInStomach.Add(chicken);
        chicken.transform.SetParent(this.transform, false);
        chicken.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
        chicken.gameObject.SetActive(false);
        this.UpdateSpeed();
        break;
      }

			Shop shop = possibleObjects [i].gameObject.GetComponent<Shop> ();
			if (shop != null && shop.open && this.gold >= shop.goldCost) {
				this.gold -= shop.goldCost;
				this.AddAbility (shop.abilityPrefab);
				shop.ItemPurchased();
			}
    }
  }

	public void UpdateHealthUI() {
		Damagable d = this.GetComponent<Damagable> ();
		this.healthImage.fillAmount = d.HealthPercent;
	}

	public float spawnDelay = 4f;
	public void Die() {
		this.UnlatchFromObject();
		this.dead = true;
		this.UpdateHealthUI ();
		StartCoroutine (RespawnDelay());
	}

	IEnumerator RespawnDelay() {
		yield return new WaitForSeconds (spawnDelay);

		Respawn ();

		yield return null;
	}

	public void Respawn() {
		this.dead = false;
		this.transform.position = spawnPoint;
		this.GetComponent<Damagable>().Reset();
		this.UpdateHealthUI();
	}
		
  private void SpitUp() {
    if (this.chickensInStomach.Count >= 1) {
      Chicken chickenToRemove = this.chickensInStomach[this.chickensInStomach.Count-1];
      this.chickensInStomach.Remove(chickenToRemove);
      StartCoroutine(chickenToRemove.SpatOut());
      this.UpdateSpeed();
    }
  }

  private void UpdateSpeed() {
    this.currentSpeed = Mathf.Clamp(this.maxSpeed - (2f * this.chickensInStomach.Count), this.minSpeed, this.maxSpeed);
  }

  private void SwitchWeapons() {
    ++this.currentAbilityIndex;
    if((this.currentAbilityIndex > (this.Abilities.Length - 1)) || (this.Abilities[this.currentAbilityIndex] == null)) {
      this.currentAbilityIndex = 0;
    }
  }

  void LatchOnto(Rigidbody obj) {
	this.latchedObject = obj;
	this.GetComponent<Rigidbody>().isKinematic = true;

    this.transform.SetParent(obj.transform);
  }

	void UnlatchFromObject () {	  
		if (this.latchedObject != null && this.latchedObject.GetComponent<BigEgg> ()) {
			--this.latchedObject.GetComponent<BigEgg> ().numLatchers;
		}
		this.GetComponent<Rigidbody> ().isKinematic = false;
		this.transform.SetParent (this.team.transform);

		this.latchedObject = null;
	}

  void OnDrawGizmosSelected ()
  {
    Gizmos.color = Color.magenta;
    Gizmos.DrawSphere (this.transform.position + this.aimDirection, 0.3f);
  }
}
