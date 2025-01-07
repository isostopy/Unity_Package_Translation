using UnityEngine;
using UnityEngine.Events;

/* README */
// >> Los idiomas y los id son case-insentive. Español, ESPAÑOL y español serán considerados el mismo idioma.
//		Ojo, si son sensibles a tildes y otros signos. Ingles e Inglés son idiomas distintos. Mejor todos los idiomas e ids sin tildes.
// >> No se pueden repetir ids.

namespace Isostopy.Translation
{
	/// <summary>
	/// Clase estatica que contiene todos los textos en todos los idiomas. </summary>
	public static class TranslationManager
	{
		/// <summary> Diccionario de traducciones. </summary>
		private static TranslationDictionary translations = new();

		/// <summary> Idioma guardado como el idioma actual. </summary>
		private static string _currentLanguage = "";
		/// <summary> Evento disparado cuando se cambia de idioma. </summary>
		private static UnityAction<string> onLanguageChange;


		// ---------------------------------------------------------------------

		/// <summary> Añade una traduccion al diccionario. </summary>
		public static void AddEntry(string language, string id, string translation)
		{
			language = language.ToLower();
			id = id.ToLower();

			translations.AddEntry(language, id, translation);

			if (string.IsNullOrEmpty(CurrentLanguage))
				CurrentLanguage = language;
		}

		// ---------

		/// <summary>
		/// Devuelve la traduccion con el id indicado en el idioma indicado. <para></para>
		/// Si el idioma se deja en null, se devuelve la traduccion en el idioma preseleccionado. </summary>
		public static string GetTranslation(string id, string language = null)
		{
			if (language == null)
				language = CurrentLanguage;

			id = id.ToLower();
			language = language.ToLower();

			if (translations.ContainsKey(language) == false)
				return "missing language [" + language + "]";
			if (translations[language].ContainsKey(id) == false)
				return "missing translation [" + id + "]";

			return translations[language][id];
		}

		// ---------

		/// <summary> Idioma actual. </summary>
		public static string CurrentLanguage
		{
			get
			{
				return _currentLanguage;
			}

			set
			{
				value = value.ToLower();

				if (value == _currentLanguage)
					return;

				if (translations.ContainsKey(value))
				{
					_currentLanguage = value;
					onLanguageChange?.Invoke(_currentLanguage);
				}
				else
				{
					Debug.LogError("El diccionario de traducciones no contiene el idioma [" + value + "]");
				}
			}
		}

		/// <summary> Añade un listener para saber cuando se cambia de idioma. </summary>
		public static void AddListenerToLanguageChange(UnityAction<string> listner) => onLanguageChange += listner;

		/// <summary> Elimina un listener del evento que notifica cuando se cambia de idioma. </summary>
		public static void RemoveListenerFromLanguageChange(UnityAction<string> listner) => onLanguageChange -= listner;
	}
}
