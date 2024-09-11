using System.IO;
using System.Xml;
using UnityEngine;
using UnityEditor;

namespace Isostopy.Translation.Editor
{
	/// <summary> Ventana del editor que permite convertir un archivo de traducciones de un formato a otro. </summary>
	public class TranslationFileConverter : EditorWindow
	{
		const string prefsKey_originPath = "TranslationFileConverter.OriginPath";
		const string prefsKey_targetPath = "TranslationFileConverter.TargetPath";


		// --------------------------------------------------------------------------
		#region Window

		[MenuItem("Isostopy/Translation/Translation File Converter")]
		public static void OpenWindow()
		{
			var window = EditorWindow.GetWindow<TranslationFileConverter>(true, "Translation File Converter");
			window.minSize = window.maxSize = new Vector2(500, 170);
		}

		private void OnGUI()
		{
			EditorGUILayout.LabelField("Para convertir un archivo de traducciones de un formato a otro:");
			EditorGUILayout.LabelField("\t > Pulsa el botón.");
			EditorGUILayout.LabelField("\t > Selecciona el archivo que quieres convertir.");
			EditorGUILayout.LabelField("\t > Selecciona la carpeta en la que se va a guardar el nuevo archivo.");
			EditorGUILayout.LabelField("\t > ¡Listo!");

			GUILayout.FlexibleSpace();

			if (GUILayout.Button("CSV to XML", GUILayout.Height(EditorGUIUtility.singleLineHeight * 2)))
			{
				ConvertCsvFileToXmlFile();
			}
		}

		#endregion


		// --------------------------------------------------------------------------
		#region CSV to XML

		/// Convierte un archivo de traducciones en CSV a un arhivo XML.
		private void ConvertCsvFileToXmlFile()
		{
			// Abrir el selector de archivos por la ultima ruta guardada.
			string csvPath = EditorPrefs.GetString(prefsKey_originPath);
			csvPath = string.IsNullOrEmpty(csvPath) ? "Assets/" : csvPath;
			csvPath = EditorUtility.OpenFilePanel("Selecciona un archivo CSV de traducciones", csvPath, "csv");
			if (string.IsNullOrEmpty(csvPath))
				return;
			// Guardar la ruta seleccionada.
			EditorPrefs.SetString(prefsKey_originPath, csvPath);

			// Sacar el contenido del archivo y meterlo en un xml.
			string csvText = File.ReadAllText(csvPath);
			XmlDocument xmlDoc = CsvTextToXmlDocument(csvText);

			// Abrir la ventana para seleccionar donde guardar el archivo.
			string csvFileName = Path.GetFileNameWithoutExtension(csvPath);
			string xmlPath = EditorPrefs.GetString(prefsKey_targetPath);
			xmlPath = string.IsNullOrEmpty(xmlPath) ? "Assets/" : xmlPath;
			xmlPath = EditorUtility.SaveFilePanel("Selecciona donde guardar el nuevo achivo XML", xmlPath, csvFileName, "xml");
			if (string.IsNullOrEmpty(xmlPath))
				return;

			EditorPrefs.SetString(prefsKey_targetPath, xmlPath);

			// Guardar el archivo.
			xmlDoc.Save(xmlPath);
			AssetDatabase.Refresh();
		}

		/// Convierte el texto extraido de un archivo de traducciones en CSV a un objeto XmlDocument.
		private XmlDocument CsvTextToXmlDocument(string csvText)
		{
			// Crear el documento de xml.
			XmlDocument doc = new XmlDocument();

			XmlElement root = doc.CreateElement(string.Empty, "translations", string.Empty);
			doc.AppendChild(root);

			XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
			doc.InsertBefore(declaration, root);

			// Meter el csv en el xml.
			var csvTable = CsvParser.ParseList(csvText);
			foreach (var line in csvTable)
			{
				string id = line[0];
				if (string.IsNullOrEmpty(id))
					continue;

				// Cada frase en el csv es un elemento dentro del root de xml.
				XmlElement element = doc.CreateElement(id);
				root.AppendChild(element);
				// Por cada idioma, le metemos un elemento hijo con la traduccion.
				for (int columnIndex = 1; columnIndex < line.Count; columnIndex++)
				{
					string language = csvTable[0][columnIndex];
					string translation = line[columnIndex];

					if (string.IsNullOrEmpty(language))
						continue;

					XmlElement childElement = doc.CreateElement(language);
					childElement.InnerText = translation;
					element.AppendChild(childElement);
				}
			}

			return doc;
		}

		#endregion
	}
}
