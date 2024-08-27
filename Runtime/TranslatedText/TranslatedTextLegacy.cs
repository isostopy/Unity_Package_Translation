using UnityEngine;
using UnityEngine.UI;

namespace Isostopy.Translation
{
	/// <summary> Muestra un texto traducido en un componente Text de la UI. </summary>
	[AddComponentMenu("Isostopy/Translation/Translated Text (Legacy)")]
	public class TranslatedTextLegacy : TranslatedText
	{
        /// <summary> Texto en el que se muestra la traduccion. </summary>
        [SerializeField] Text uiText = null;


		// ---------------------------------------------------------------------

		private void Reset()
		{
			uiText = GetComponent<Text>();
			id = uiText != null ? uiText.gameObject.name : id;
		}

		protected override void SetText(string text)
		{
			uiText.text = text;
		}
	}
}
