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
	- Camera (Maybe later spawned above with a follow script)
	- Player Model <Animator>
	- HealthBar<HealthBar, Canvas>
	- Audio
		- MoveSound
		- DeathSound
		- AttackSound		

Resource
	+ value
	- Model <Animator>

TeamBase
	+ Team (reference)
	- Model

GameMaster
 + GameRound (Reference)

Pickup
|- HealthPickup
|- WeaponPickup
|- BuffPickup


UI <Canvas>
 - PlayerCameras <PlayerCameraRenderer>
 - HUD
 - Overlays
 - Curtain


Static Classes
==============
Game
 - Config
 - State<enum>
 - LoadScene()

GameRound
 - Team[]


Team
 + color
 + Player[]


ScriptableObjects
=================

GameConfig
 - based on environment
 - holds other configs

```
