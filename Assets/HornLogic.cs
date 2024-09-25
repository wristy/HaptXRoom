using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Hx1DJoint)), RequireComponent(typeof(AudioSource))]
public class HornLogic : MonoBehaviour {

  void Start() {
    _hornAudio = GetComponent<AudioSource>();

    Hx1DJoint joint = GetComponent<Hx1DJoint>();
    HxStateFunction stateFunction;
    if (joint != null && joint.GetOperatingDof().TryGetStateFunctionByName(hornStateFunctionName,
        out stateFunction)) {
      stateFunction.OnStateChange += OnHornStateChange;
    } else {
      HxDebug.LogError(string.Format(
          "State function with name %s not found.", hornStateFunctionName), this);
    }
  }

  void OnHornStateChange(int newState) {
    if (_hornAudio == null) {
      return;
    }

    if (_hornVolumeFadeCoroutine != null) {
      StopCoroutine(_hornVolumeFadeCoroutine);
    }

    if (newState == hornOnState) {
      _hornVolumeFadeCoroutine = StartCoroutine(FadeHornVolume(maxVolume));
    } else {
      _hornVolumeFadeCoroutine = StartCoroutine(FadeHornVolume(0.0f));
    }
  }

  IEnumerator FadeHornVolume(float targetVolume) {
    if (_hornAudio == null) {
      yield return null;
    }

    while (_hornAudio.volume != targetVolume) {
      float diff = targetVolume - _hornAudio.volume;
      float delta = fadeTime > 0.0f ?
          Mathf.Sign(diff) * maxVolume * Time.deltaTime / fadeTime : diff;
      if (delta * delta > diff * diff) {
        delta = diff;
      }
      _hornAudio.volume += delta;

      yield return null;
    }
  }

  [Tooltip("The name of the state function to bind for the horn.")]
  public string hornStateFunctionName = "Function1";

  [Tooltip("The state that corresponds to the horn being on.")]
  public int hornOnState = 1;

  [Tooltip("The max volume of the horn."), Range(0.0f, 1.0f)]
  public float maxVolume = 0.75f;

  [Tooltip("The fade time of the horn."), Range(0.0f, float.MaxValue)]
  public float fadeTime = 0.01f;

  private AudioSource _hornAudio = null;

  private Coroutine _hornVolumeFadeCoroutine = null;
}
