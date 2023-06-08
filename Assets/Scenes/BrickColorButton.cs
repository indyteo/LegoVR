using UnityEngine;
using UnityEngine.UI;

namespace Scenes {
	public class BrickColorButton : MonoBehaviour {
		private LegoBrick brick;
		private Button button;
		private Material material;

		public Material Material {
			get => this.material;
			set {
				this.material = value;
				this.brick.SetMaterial(value);
			}
		}

		private void Awake() {
			this.brick = this.GetComponentInChildren<LegoBrick>();
			this.button = this.GetComponent<Button>();
		}

		private void OnEnable() {
			this.button.onClick.AddListener(this.OnClick);
		}

		private void OnDisable() {
			this.button.onClick.RemoveListener(this.OnClick);
		}

		private void OnClick() {
			LegoManager.Instance.CurrentMaterial = this.material;
		}
	}
}
