using UnityEngine;
using TMPro;

namespace Isostopy.Translation
{
	/// <summary> Muestra un texto traducido en un componente TextMeshPro. </summary>
	[AddComponentMenu("Isostopy/Translation/Translated Text (TMP)")]
	public class TranslatedTextMeshPro : TranslatedText
	{
		/// <summary> Texto en el que se muestra la traduccion. </summary>
		[SerializeField] TextMeshProUGUI uiText = null;


		// ---------------------------------------------------------------------

		private void Reset()
		{
			uiText = GetComponent<TextMeshProUGUI>();
			id = uiText != null ? uiText.gameObject.name : id;
		}

		protected override void SetText(string text)
		{
			uiText.text = text;
		}
	}
}