using BNG;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LegoVR.Scripts {
	public class LegoManager : MonoBehaviour {
		[field: SerializeField]
		public AudioClip ConnectSound { get; private set; }
		[field: SerializeField]
		public AudioClip DisconnectSound { get; private set; }
		[field: SerializeField]
		public Material CurrentMaterial { get; set; }
		[field: SerializeField]
		public InputActionReference QuitAction { get; private set; }

		public BricksGraph Bricks { get; private set; } = new BricksGraph();

		public static LegoManager Instance { get; private set; }

		private void Awake() {
			if (Instance == null) {
				Instance = this;
				DontDestroyOnLoad(this);
			}
		}

		private void OnEnable() {
			this.QuitAction.action.performed += Quit;
		}

		private void OnDisable() {
			this.QuitAction.action.performed -= Quit;
		}

		public static void Quit(InputAction.CallbackContext context) {
			Application.Quit();
		}

		public static void PlayConnectSound(Vector3 position) {
			Instance.PlaySound(Instance.ConnectSound, position);
		}

		public static void PlayDisconnectSound(Vector3 position) {
			Instance.PlaySound(Instance.DisconnectSound, position);
		}

		public void PlaySound(AudioClip sound, Vector3 position) {
			VRUtils.Instance.PlaySpatialClipAt(sound, position, 1f, 0.5f);
		}

		public static void SelectMaterial() {
			VRUtils.Instance.Log("Select");
		}
	}
}
