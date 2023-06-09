using System.Collections.Generic;
using UnityEngine;

namespace LegoVR.Scripts {
	public class BrickColorMenu : MonoBehaviour {
		[SerializeField]
		private List<Material> materials;
		[SerializeField]
		private GameObject buttonsContainer;
		[SerializeField]
		private GameObject buttonPrefab;
		[SerializeField]
		private LegoBrick preview;
		[SerializeField]
		private BrickInventoryMenu brickMenu;

		private void Start() {
			foreach (Material material in this.materials) {
				GameObject instance = Instantiate(this.buttonPrefab, this.buttonsContainer.transform);
				BrickColorButton button = instance.GetComponent<BrickColorButton>();
				button.Material = material;
				button.MaterialSelectedCallback = this.UpdateCurrentMaterial;
			}
		}

		private void OnEnable() {
			this.brickMenu.gameObject.SetActive(false);
		}

		private void UpdateCurrentMaterial(Material material) {
			this.preview.SetMaterial(material);
			this.brickMenu.RefreshColor(material);
		}
	}
}
