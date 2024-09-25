using UnityEngine;

[RequireComponent(typeof(HxNutAndBolt))]
public class NutAndBoltLogic : MonoBehaviour {

  //! The layer to use for the bolt when it's attached.
  [Tooltip("The layer to use for the bolt when it's attached.")]
  public string onAttachLayer = "CarComponents";

  //! The driving nut and bolt interaction.
  private HxNutAndBolt _hxNutAndBolt = null;

  //! The layer the bolt had before it attached.
  private int _cachedLayer = 0;

  //! Called when the script is being loaded.
  private void Awake() {
    _hxNutAndBolt = GetComponent<HxNutAndBolt>();

    if (_hxNutAndBolt == null) {
      return;
    }

    _hxNutAndBolt.OnAttach += OnAttach;
    _hxNutAndBolt.OnRelease += OnRelease;
  }

  //! Execute when the bolt attaches.
  private void OnAttach() {
    if (_hxNutAndBolt == null || _hxNutAndBolt.bolt == null) {
      return;
    }

    _cachedLayer = _hxNutAndBolt.bolt.gameObject.layer;
    HxShared.SetLayerRecursively(_hxNutAndBolt.bolt.gameObject,
        LayerMask.NameToLayer(onAttachLayer));
  }

  //! Execute when the bolt releases.
  private void OnRelease() {
    if (_hxNutAndBolt == null || _hxNutAndBolt.bolt == null) {
      return;
    }

    HxShared.SetLayerRecursively(_hxNutAndBolt.bolt.gameObject, _cachedLayer);
  }
}
