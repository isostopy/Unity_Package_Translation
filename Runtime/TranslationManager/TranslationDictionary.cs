using System.Collections.Generic;
using UnityEngine;

namespace Isostopy.Translation
{ 
	/// <summary>
	/// Diccionario de traducciones.
	/// <para></para>
	/// Accede a una traduccion haciendo: <br/>
	/// <code> Dictionary[language][id] </code>
	/// </summary>
	public class TranslationDictionary : Dictionary<string, Dictionary<string, string>>
	{
		/// <summary> Añade una nueva traduccion al diccionario. </summary>
		public void AddEntry(string language, string id, string translation)
		{
			// Añadir el idioma al diccionario.
			if (this.ContainsKey(language) == false)
			{
				this.Add(language, new Dictionary<string, string>());
			}
			// Checkear que no este la id duplicada.
			if (this[language].ContainsKey(id) == true)
			{
				Debug.LogWarning("La traduccion con id [" + id + "] ya existe para el idioma [" + language + "] en el diccionario de traducciones.");
				return;
			}

			// Añadir al diccionario la traduccion.
			this[language].Add(id, translation);
		}
	}
}
