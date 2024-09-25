using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearSelectorButtonLock : MonoBehaviour {

  // Use this for initialization.
  void Start() {
    if (buttonJoint != null & selectorJoint != null) {
      HxDof buttonDof = buttonJoint.GetOperatingDof();
      if (buttonDof != null) {
        HxStateFunction stateFunction;
        if (buttonDof.TryGetStateFunctionByName(buttonStateFunction, out stateFunction)) {
          stateFunction.OnStateChange += OnStateChanged;
        }
      }

      HxDof selectorDof = selectorJoint.GetOperatingDof();
      if (selectorDof != null) {
        HxDofBehavior behavior;
        selectorDof.TryGetBehaviorByName(selectorBehavior, out behavior);
        _detentBehavior = (HxDofDetentBehavior)behavior;
      }

      // Make sure the selector dof starts frozen in its initial position.
      selectorJoint.TeleportAnchor1AlongOperatingDof(selectorJoint.initialPosition);
      selectorJoint.Freeze();
    }
  }

  private void OnStateChanged(int newState) {
    if (newState == freezeState && _detentBehavior != null) {
      if (_detentBehavior.DetentIndex > -1) {
        selectorJoint.TeleportAnchor1AlongOperatingDof(_detentBehavior.Detent);
      }
      selectorJoint.Freeze();
    } else if (newState == unfreezeState) {
      selectorJoint.Unfreeze();
    }
  }

  [Tooltip("The button joint we listen to for state changes.")]
  public Hx1DTranslator buttonJoint = null;

  [Tooltip("The name of the state function to listen to on Button Joint.")]
  public string buttonStateFunction = "Function0";

  [Tooltip("The selector joint we freeze or unfreeze.")]
  public Hx1DTranslator selectorJoint = null;

  [Tooltip("The name of the behavior on Selector Joint to watch for when we're close to a detent.")]
  public string selectorBehavior = "Behavior0";

  [Tooltip("The state when we want Selector Joint frozen.")]
  public int freezeState = 0;

  [Tooltip("The state when we don't want Selector Joint frozen.")]
  public int unfreezeState = 1;

  private HxDofDetentBehavior _detentBehavior = null;
}
