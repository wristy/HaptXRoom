using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDoorHandleLogic : MonoBehaviour {

  // Use this for initialization
  void Start() {
    if (doorJoint != null) {
      HxDof dof = doorJoint.GetOperatingDof();
      if (dof != null) {
        dof.TryGetStateFunctionByName(doorStateFunction, out _doorStateFunctionRef);
      }
    }

    if (handleJoint != null) {
      HxDof dof = handleJoint.GetOperatingDof();
      if (dof != null) {
        if (dof.TryGetStateFunctionByName(handleStateFunction, out _handleStateFunctionRef)) {
          _handleStateFunctionRef.OnStateChange += OnHandleStateChange;
        }
      }
    }
  }

  void Update() {
    if (_handleStateFunctionRef != null &&
        _handleStateFunctionRef.CurrentState == handleCloseDoorState &&
        _doorStateFunctionRef != null &&
        _doorStateFunctionRef.CurrentState == doorClosableState) {
      if (!doorJoint.Frozen) {
        doorJoint.Freeze();
      }
    } else {
      if (doorJoint.Frozen) {
        doorJoint.Unfreeze();
      }
    }
  }

  void OnHandleStateChange(int newState) {
    if (newState == handleCloseDoorState && doorJoint != null) {
      doorJoint.SetLimits(lowerLimitClosed, upperLimit);
    } else if (newState == handleOpenDoorState) {
      if (doorJoint != null) {
        doorJoint.SetLimits(lowerLimitOpen, upperLimit);
      }
    }
  }

  [Tooltip("The joint constraining the door.")]
  public Hx1DRotator doorJoint = null;

  [Tooltip("The name of the state function to listen to on the door.")]
  public string doorStateFunction = "Function0";
  private HxStateFunction _doorStateFunctionRef = null;

  [Tooltip("The door state that allows the door to be closed.")]
  public int doorClosableState = 1;

  [Tooltip("The lower limit of the door when the handle is open.")]
  public float lowerLimitOpen = 2.0f;

  [Tooltip("The lower limit of the door when the handle is closed.")]
  public float lowerLimitClosed = 0.0f;

  [Tooltip("The upper limit of the door.")]
  public float upperLimit = 80.0f;

  [Tooltip("The joint constraining the handle.")]
  public Hx1DRotator handleJoint = null;

  [Tooltip("The name of the state function to listen to on the handle.")]
  public string handleStateFunction = "Function0";
  private HxStateFunction _handleStateFunctionRef = null;

  [Tooltip("The handle state that closes the door.")]
  public int handleCloseDoorState = 0;

  [Tooltip("The handle state that opens the door.")]
  public int handleOpenDoorState = 1;
}
