using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Isostopy.Translation
{
	/// <summary>
	/// Componente que hace de intermediario entre la escena y el TranslationManager para cambiar el idioma seleccionado. </summary>
	/// Como clase estatica, el TranslationManager no se puede referenciar desde el inspector.
	/// Este componente permite modificar el idioma desde el inspector.

	[AddComponentMenu("Isostopy/Translation/Language Selector")]
	public class LanguageSelector : MonoBehaviour
    {
        /// <summary> Lista de botones de la UI asociados al idioma que va a poner cada uno. </summary>
        [Space][SerializeField] List<LanguageButtom> buttons = new List<LanguageButtom>();


		// ---------------------------------------------------------------------

		private void Start()
		{
            // Cuando se pulse alguno de los botones, cambiar el idioma.
			foreach(var languageButton in buttons)
            {
                languageButton.button.onClick.AddListener(() =>
                {
					SetLanguage(languageButton.language);
                });
            }
		}


		// ---------------------------------------------------------------------

		/// <summary> Idioma seleccionado en el Translation Manager. </summary>
		public string CurrentLanguage
        {
            get { return TranslationManager.CurrentLanguage; }
        }

		/// <summary> Cambia el idioma seleccionado en el Translation Manager. </summary>
		public void SetLanguage(string language)
		{
			TranslationManager.SetLanguage(language);
		}


		// ---------------------------------------------------------------------

		[System.Serializable]
        private class LanguageButtom
        {
            public string language;
            public Button button;
        }
    }
}
