using System.Collections.Generic;

namespace Isostopy.Translation
{
	/// <summary> Clase estatica para convertir un texto en formato CSV a una tabla con la que poder trabajar. </summary>
	public static class CsvParser
	{
		// ---------------------------------------------------------------------

		/// <summary>
		/// Devuelve el texto de un csv como una lista de strings de dos dimensiones. </summary>
		public static List<List<string>> ParseList(string csvContent)
		{
			List<List<string>> csvTable = new List<List<string>>();             /// Lista 2D para guardar el contenido del csv.
			csvTable.Add(new List<string>());
			int currentLine = 0;

			string cell;
			int i = 0;

			// Mirar char a char el contenido del csv.
			csvContent = csvContent.Trim();
			while (i < csvContent.Length)
			{
				cell = "";

				// Si hay unas comillas, guardar como casilla todo lo que hay hasta las siguientes comillas unicas.
				if (csvContent[i] == '"')
					cell += GetQuotedCell(csvContent, ref i);
				// Si no, guardar como casilla todo lo que hay hasta la siguiente coma o \n.
				else
					cell += GetSimpleCell(csvContent, ref i);

				// Añadir la casilla a la fila actual.
				csvTable[currentLine].Add(cell);

				// Si estamos en un salto de linea, añadir una fila a la tabla.
				if (i < csvContent.Length && csvContent[i] == '\n')
				{
					csvTable.Add(new List<string>());
					currentLine++;
				}
				i++;    /// Sumar uno para pasar de la coma o salto de linea en el que estamos.
			}

			return csvTable;
		}

		/// <summary>
		/// Devuelve el texto de un csv como una array de strings de dos dimensiones. </summary>
		public static string[][] ParseArray(string csvContent)
		{
			List<string>[] listArray = ParseList(csvContent).ToArray();

			string[][] csvTable = new string[listArray.Length][];
			for (int i = 0; i < csvTable.Length; i++)
			{
				csvTable[i] = listArray[i].ToArray();
			}

			return csvTable;
		}


		// ---------------------------------------------------------------------

		/// <summary>
		/// Devuelve un string que va desde csvContent[i] hasta la siguiente coma o salto de linea. <para></para>
		/// Moviendo i hasta la posicion de esa coma o \n. </summary>
		static string GetSimpleCell(string csvContent, ref int i)
		{
			string cell = "";

			while (i < csvContent.Length && csvContent[i] != ',' && csvContent[i] != '\n')
			{
				cell += csvContent[i];
				i++;
			}

			return cell;
		}

		/// <summary>
		/// Devuelve un string que va desde csvContent[i + 1] hasta las siguiente aparicion de unas comillas unicas. <para></para>
		/// Moviendo i hasta la posicion de la siguiente coma o \n. </summary>
		static string GetQuotedCell(string csvContent, ref int i)
		{
			string cell = "";

			i++;
			while (i < csvContent.Length - 1)
			{
				// En los csv las comillas que aparecen dentro de las casillas se duplican.
				// Si encontramos comillas y no hay otras detras, por tanto, ha terminado la casilla.
				if (csvContent[i] == '"' && csvContent[i + 1] != '"')
				{
					i++;
					break;
				}
				// Si hay comillas y hay otras detras, avanzar uno para guardar una sola comilla.
				else if (csvContent[i] == '"')
					i++;
				cell += csvContent[i];
				i++;
			}

			// Ir hasta la siguiente ',' o \n.
			while (i < csvContent.Length && csvContent[i] != ',' && csvContent[i] != '\n')
				i++;

			return cell;
		}
	}
}
