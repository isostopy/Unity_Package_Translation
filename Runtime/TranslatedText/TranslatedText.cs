using UnityEngine;

namespace Isostopy.Translation
{
	/// <summary>
	/// Componente que muestra un texto traducido en base a la informacion del componente persistente TranslationManager.</summary>
	public abstract class TranslatedText : MonoBehaviour
	{
		/// <summary> Id de la traduccion. </summary>
		[Space][SerializeField] protected string id = "translation-id";


		// ---------------------------------------------------------------------

		/// <summary> Id de la traduccion que se muestra. </summary>
		public string TranslationId
		{
			get => id;
			set
			{
				id = value;

				string translation = GetTranslation();
				SetText(translation);
			}
		}

		/// <summary> Devuelve el texto traducido que tiene que mostrar este componente. </summary>
		public string GetTranslation()
		{
			return TranslationManager.GetTranslation(id);
		}


		// ---------------------------------------------------------------------

		protected virtual void Start()
		{
			// Actualizar el texto y añadir listener al cambio de idioma.
			SetText(TranslationManager.GetTranslation(id));
			TranslationManager.AddLanguageListener(UpdateText);
		}

		protected virtual void OnDestroy()
		{
			// Al destruir el game object elimina el listener.
			TranslationManager.RemoveLanguageListener(UpdateText);
		}

		/// <summary> Listener del evento que lanza el manager cuando se cambia el idioma. </summary>
		protected virtual void UpdateText(string newLanguage)
		{
			if (string.IsNullOrEmpty(id))
				return;

			var translation = GetTranslation();
			SetText(translation);
		}


		// ---------------------------------------------------------------------

		/// <summary> Cambia el texto que se esta mostrando. </summary>
		protected abstract void SetText(string text);
	}
}