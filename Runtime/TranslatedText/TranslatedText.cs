using UnityEngine;

namespace Isostopy.Translation
{
	/// <summary>
	/// Componente que muestra un texto traducido en base a la informacion del TranslationManager.</summary>
	public abstract class TranslatedText : MonoBehaviour
	{
		/// <summary> Id de la traduccion. </summary>
		[Space][SerializeField] protected string id = "translation-id";

		/// <summary> Id de la traduccion. </summary>
		public string TranslationId
		{
			get => id;
			set
			{
				id = value;
				Translate();
			}
		}


		// ---------------------------------------------------------------------

		protected virtual void Start()
		{
			Translate();

			// Añadir listener al cambio de idioma.
			TranslationManager.AddListenerToLanguageChange(OnLanguageChanged);
		}

		protected virtual void OnDestroy()
		{
			// Al destruir el game object elimina el listener.
			TranslationManager.RemoveListenerFromLanguageChange(OnLanguageChanged);
		}

		/// <summary> Listener del evento que lanza el manager cuando se cambia el idioma. </summary>
		protected virtual void OnLanguageChanged(string newLanguage)
		{
			Translate();
		}


		// ---------------------------------------------------------------------

		protected void Translate()
		{
			if (string.IsNullOrEmpty(id))
				return;

			var translation = TranslationManager.GetTranslation(id);
			SetText(translation);
		}

		/// <summary> Cambia el texto que se esta mostrando. </summary>
		protected abstract void SetText(string text);
	}
}