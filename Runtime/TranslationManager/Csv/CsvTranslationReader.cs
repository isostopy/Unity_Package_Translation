using System.Collections.Generic;
using UnityEngine;

// >> Estrutura del csv: la primera columna son los ids, la primera fila son los nombres de los idioma.
/*		|     | language1 | language2 |
 *		| id1 | (...)     | (...)     |
 *		| id2 | (...)     | (...)     | */

namespace Isostopy.Translation
{
	/// <summary>
	/// Translation manager que extrae las traducciones de un archivo CSV. </summary>
	[AddComponentMenu("Isostopy/Translation/CSV Translation Reader")]
	public class CsvTranslationReader : MonoBehaviour
	{
		/// Array con todos los archivos csv de los que se extraen las traducciones.
		[Space][SerializeField] TextAsset[] csvFiles = { };


		// ---------------------------------------------------------------------

		private void Awake()
		{
			foreach (TextAsset file in csvFiles)
			{
				// Extraer el contido del CSV a una tabla.
				List<List<string>> csvTable = CsvParser.ParseList(file.text);
				// Revisar cada casilla de la tabla (nos saltamos la 1ª fila y la 1ª columna).
				for (int line = 1; line < csvTable.Count; line++)
				{
					for (int column = 1; column < csvTable[line].Count; column++)
					{
						var id =			csvTable[line][0];          // La primera columna es el id.
						var language =		csvTable[0][column];        // La primera fila es el idioma.
						var translation =	csvTable[line][column];     // Lo que hay en la casilla es la traduccion para ese id en ese idioma.

						TranslationManager.AddEntry(language, id, translation);
					}
				}
			}
		}

		// ---------------------------------------------------------------------
	}
}
