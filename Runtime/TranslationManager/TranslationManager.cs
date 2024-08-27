using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* README */
// En nombre del idioma en la primera fila tiene que ser el mismo en todos los archivos, pero puede estar en columnas diferentes.
// Los idiomas y los id son case-insentive. Da igual como los escribas. ESPAÑOL y español son lo mismo.
// Ojo, si son sensibles a tildes y otros signos. Ingles e Inglés son idiomas distintos, mejor todos los nombres de idiomas e ids sin tildes.
// No se pueden repetir ids, ni si quiera entre distintos archivos csv.

namespace Isostopy.Translation
{
	/// <summary>
	/// Clase persistente que tiene almacenado un diccionario con todas las frases en todos los idiomas, extraidas de los archivos csv de traducciones. </summary>
	[AddComponentMenu("Isostopy/Translation/Translation Manager")]
	public class TranslationManager : MonoBehaviour
	{
		static TranslationManager instance = null;

		/// <summary> Array con todos los archivos csv de los que se extraen las traducciones. </summary>
		[Space][SerializeField] TextAsset[] csvFiles = null;

		/// <summary> Diccionario de traducciones. </summary>
		/// Cada key del diccionario es un idioma, cuyo value es su propio diccionario con el key = id, y el value = la traduccion correspondiente a ese idioma.
		Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>();

		/// <summary> Idioma guardado como el idioma actual. </summary>
		private static string _currentLanguage = null;
		/// <summary> Evento disparado cuando se cambia de idioma. </summary>
		private static UnityAction<string> onLanguageChange;


		// ---------------------------------------------------------------------
		#region Initialization

		private void Awake()
		{
			PersistentSetup();
			FillDictionaries();
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
				Destroy(this);
		}

		// ------------------------------------------

		/// <summary>
		/// Llena el diccionario de traducciones con el contenido de los csv. </summary>
		private void FillDictionaries()
		{
			translations.Clear();
			foreach (TextAsset file in csvFiles)
			{
				// Extraer el conenido del csv en una tabla 2D.
				List<List<string>> csvTable = CsvParser.ParseList(file.text);

				// La primera linea son los nombres de los idiomas.
				List<string> languages = csvTable[0];
				AddLanguages(languages);
				// Cada una de las demas lineas es una traduccion.
				for (int y = 1; y < csvTable.Count; y++)
				{
					List<string> line = csvTable[y];
					AddTranslations(line, languages);
				}
			}
		}

		/// <summary>
		/// Añade al diccionario de traducciones una entrada vacia por cada idioma que no aparece ya. </summary>
		private void AddLanguages(List<string> languages)
		{
			// El idioma por defecto es el primer idioma del primer csv.
			if (_currentLanguage == null && languages.Count > 1)
				_currentLanguage = languages[1].ToLower().Trim();

			for (int x = 1; x < languages.Count; x++)
			{
				languages[x] = languages[x].ToLower().Trim();
				if (!translations.ContainsKey(languages[x]))
					translations.Add(languages[x], new Dictionary<string, string>());
			}
		}

		/// <summary>
		/// Añade una nueva traduccion al diccionario de traducciones. </summary>
		/// line[0] es el id de la traduccion. line[1] es el texto en el idioma languages[1]. line[2], en lenguages [2]. Etc.
		private void AddTranslations(List<string> line, List<string> languages)
		{
			string id = line[0].ToLower().Trim();
			 
			int x = 1;
			while (x < line.Count && x < languages.Count)
			{
				string language = languages[x];
				string cell = line[x];

				// Si en las traducciones ya aparece esa id, avisar de que hay un duplicado.
				if (translations[language].ContainsKey(id))
				{
					if (id != "") Debug.LogWarning("Los CSV contienen duplicados del id [" + id + "]");
					break;
				}

				// Añadir al diccionario.
				translations[language].Add(id, cell);
				x++;
			}
		}

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
				if (instance != null)
					return _currentLanguage;
				return "";
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
		public static void AddLanguageListener(UnityAction<string> listner) => onLanguageChange += listner;

		/// <summary> Elimina un listener del evento que notifica cuando se cambia de idioma. </summary>
		public static void RemoveLanguageListener(UnityAction<string> listner) => onLanguageChange -= listner;

		#endregion
	}
}
