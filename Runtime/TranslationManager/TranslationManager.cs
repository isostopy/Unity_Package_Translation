using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Isostopy.Translation
{
	/// <summary>
	/// Clase persistente que tiene un diccionario almacenando todas los textos en todos los idiomas. </summary>
	public abstract class TranslationManager : MonoBehaviour
	{
		public static TranslationManager instance;

		/// <summary> Diccionario de traducciones. </summary>
		protected TranslationDictionary translations = new TranslationDictionary();

		/// <summary> Idioma guardado como el idioma actual. </summary>
		private static string _currentLanguage = "";
		/// <summary> Evento disparado cuando se cambia de idioma. </summary>
		private static UnityAction<string> onLanguageChange;


		// ---------------------------------------------------------------------
		#region Initialization

		protected virtual void Awake()
		{
			PersistentSetup();
			translations = GenerateTranslationDictionary();

			// El primer idioma del diccionario es el idioma por defecto.
			var firstLanguage = translations.Keys.ElementAt(0);
			CurrentLanguage = firstLanguage;
		}

		/// <summary> Asegurarse de que solo hay una instancia de este componente.
		/// Y de que no se destruye al cambiar de escenas. </summary>
		private void PersistentSetup()
		{
			if (instance == null)
			{
				instance = this;
				DontDestroyOnLoad(this);
			}
			else
			{
				Debug.LogWarning("Hay multiples instancias del Translation Manager.", gameObject);
				Destroy(this);
			}
		}

		// ----------------------------------

		/// <summary>
		/// Genera el diccionario de traducciones. <br/>
		/// Cada key del diccionario es un idioma, cuyo value es otro diccionario con el key = id, y el value = el texto correspondiente a ese idioma. </summary>
		public abstract TranslationDictionary GenerateTranslationDictionary();

		#endregion


		// ---------------------------------------------------------------------
		#region Functionality

		/// <summary>
		/// Devuelve la traduccion con el id indicado en el idioma indicado. <para></para>
		/// Si el idioma se deja en null, se devuelve la traduccion en el idioma preseleccionado. </summary>
		public static string GetTranslation(string id, string language = null)
		{
			if (instance == null)
				return "missing TranslationManager";

			if (language == null)
				language = _currentLanguage;

			id = id.ToLower();
			language = language.ToLower();

			if (instance.translations.ContainsKey(language) == false)
				return "missing language [" + language + "]";
			if (instance.translations[language].ContainsKey(id) == false)
				return "missing translation [" + id + "]";

			return instance.translations[language][id];
		}

		/// <summary> Idioma actual. </summary>
		public static string CurrentLanguage
		{
			get
			{
				if (instance == null)
					return "";
				return _currentLanguage;
			}

			set
			{
				if (instance == null)
					return;

				value = value.ToLower();

				if (value == _currentLanguage)
					return;

				if (instance.translations.ContainsKey(value))
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

		#endregion
	}
}
