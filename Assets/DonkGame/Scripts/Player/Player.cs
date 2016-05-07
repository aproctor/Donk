﻿using UnityEngine;
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

  [SerializeField]
  public BoxCollider InteractTrigger;

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

    this.currentSpeed = this.maxSpeed;
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
      this.transform.LookAt (this.transform.position + this.aimDirection);
    }
  }

  void FixedUpdate ()
  {
    if (device) {
      UpdateDirection ();
    }
  }

  void UpdateDirection ()
  {        
    //Convert Incontrol vectors to movement directions in 3d space
    Vector3 moveDirection = new Vector3 (device.Direction.Vector.x, 0, device.Direction.Vector.y).normalized;
    this.aimDirection = new Vector3 (device.RightStick.Vector.x, 0f, device.RightStick.Vector.y);

    this.GetComponent<Rigidbody> ().MovePosition (this.transform.position + moveDirection * Time.deltaTime * this.currentSpeed);
    if (this.turret && aimDirection.sqrMagnitude > this.fireSensitivity) {                
      Quaternion quat = Quaternion.LookRotation (aimDirection, Vector3.up);
      this.turret.transform.rotation = quat;
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
      this.SpitUp();
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

  void PrimaryAction ()
  {

    Collider[] possibleObjects = Physics.OverlapBox(this.InteractTrigger.bounds.center, this.InteractTrigger.bounds.extents);

    for (int i = 0; i < possibleObjects.Length; i++) {
      BigEgg egg = possibleObjects [i].gameObject.GetComponent<BigEgg> ();
      if (egg != null) {
        LatchOntoEgg(egg);
        break;
      }

      Chicken chicken = possibleObjects [i].gameObject.GetComponent<Chicken> ();
      if (chicken != null && (this.chickensInStomach.Count < this.maxChickensInStomach)) {
        this.chickensInStomach.Add(chicken);
        chicken.transform.SetParent(this.transform, true);
        chicken.gameObject.SetActive(false);
        this.UpdateSpeed();
        break;
      }
    }
  }

  private void SpitUp() {
    if (this.chickensInStomach.Count > 1) {
      Chicken chickenToRemove = this.chickensInStomach[this.chickensInStomach.Count-1];
      chickenToRemove.transform.parent = null;
      this.chickensInStomach.Remove(chickenToRemove);
      chickenToRemove.gameObject.SetActive(true);
      this.UpdateSpeed();
    }
  }

  private void UpdateSpeed() {
    this.currentSpeed = Mathf.Clamp(this.maxSpeed - (2f * this.chickensInStomach.Count), this.minSpeed, this.maxSpeed);
  }

  void LatchOntoEgg(BigEgg egg) {
    Debug.LogError ("Latching onto egg",egg.gameObject);

  }

  void OnDrawGizmosSelected ()
  {
    Gizmos.color = Color.magenta;
    Gizmos.DrawSphere (this.transform.position + this.aimDirection, 0.3f);
  }
}
