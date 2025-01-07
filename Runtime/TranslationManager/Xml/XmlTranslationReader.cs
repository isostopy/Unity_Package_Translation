using System.Xml;
using UnityEngine;

// >> Estructura del xml:
/*		<?xml ...?>
 *		<root>
 *			<id1>
 *				<language1> (...) </language1>
 *				<language2> (...) </language2>
 *			</id1> 
 *			<id2>
 *				<language1> (...) </language1>
 *				<language2> (...) </language2>
 *			</id2>
 *		</root>		*/

namespace Isostopy.Translation
{
	/// <summary>
	/// Extrae las traducciones de archivos XML. </summary>
	[AddComponentMenu("Isostopy/Translation/XML Translation Reader")]
	public class XmlTranslationReader : MonoBehaviour
	{
		/// Array con los archivos de los que se extraen las traducciones.
		[Space][SerializeField] TextAsset[] xmlFiles = { };


		// ---------------------------------------------------------------------

		private void Awake()		
		{
			foreach (var file in xmlFiles)
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(file.text);

				foreach (XmlNode node in xmlDoc.DocumentElement)
				{
					foreach (XmlNode childNode in node.ChildNodes)
					{
						var id =			node.Name;
						var language =		childNode.Name;
						var translation =	childNode.InnerText;

						TranslationManager.AddEntry(language, id, translation);
					}
				}
			}
		}

		// ---------------------------------------------------------------------
	}
}
