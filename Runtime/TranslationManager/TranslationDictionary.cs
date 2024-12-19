using System.Collections.Generic;
using UnityEngine;

/* README */
// >> Cada key del diccionario es un idioma, cuyo value es otro diccionario con el key = id, y el value = el texto correspondiente a ese idioma.
// >> No se pueden repetir ids.
// >> Los idiomas y los id son case-insentive. Espa�ol, ESPA�OL y espa�ol ser�n considerados el mismo idioma.
//		Ojo, si son sensibles a tildes y otros signos. Ingles e Ingl�s son idiomas distintos. Mejor todos los idiomas e ids sin tildes.

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
		/// <summary> A�ade una nueva traduccion al diccionario. </summary>
		public void AddEntry(string language, string id, string translation)
		{
			// Asegurar que las id estan en minuscula.
			id = id.ToLower();
			language = language.ToLower();
			// A�adir el idioma al diccionario.
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

			// A�adir al diccionario la traduccion.
			this[language].Add(id, translation);
		}
	}
}
