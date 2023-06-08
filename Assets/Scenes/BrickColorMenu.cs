using System.Collections.Generic;
using UnityEngine;

namespace Scenes {
	public class BrickColorMenu : MonoBehaviour {
		[SerializeField]
		private List<Material> materials;
		[SerializeField]
		private GameObject buttonsContainer;
		[SerializeField]
		private GameObject buttonPrefab;
		[SerializeField]
		private LegoBrick preview;

		private void Start() {
			foreach (Material material in this.materials) {
				GameObject instance = Instantiate(this.buttonPrefab, this.buttonsContainer.transform);
				BrickColorButton button = instance.GetComponent<BrickColorButton>();
				button.Material = material;
				button.MaterialSelectedCallback = this.UpdateCurrentMaterial;
			}
		}

		private void UpdateCurrentMaterial(Material material) {
			this.preview.SetMaterial(material);
		}
	}
}
