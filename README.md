Capture the Flag


```

=============
Class Model
=============


GameObjects
=============

PlayerObject <PlayerController, RigidBody, Collider>
	+ direction
	+ actualDirection (smoothing)	
	- HealthBar<HealthBar, Canvas>
	- Audio
		- MoveSound
		- DeathSound
		- AttackSound		

Objective (Chicken)
	+ captureValue
	+ valuePerMinute
	- Model <Animator>


GameMaster
 + GameRound (Reference)

TeamBase
	+ Team (reference)
	- Model

Pickup
|- HealthPickup
|- WeaponPickup
|- BuffPickup




Static Classes
==============
Game
 - Config
 - State<enum>
 - LoadScene()

GameRound
 - Team[]

Team
 + Player[]


```
