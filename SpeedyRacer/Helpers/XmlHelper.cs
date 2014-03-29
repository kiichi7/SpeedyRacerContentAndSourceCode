// Only required for TrackImporter, which is only used in Debug mode
// and in conjunction with unit tests to import tracks and combimodels.
#if DEBUG
// Project: SpeedyRacer, File: XmlHelper.cs
// Namespace: SpeedyRacer.Helpers, Class: XmlHelper
// Path: C:\code\SpeedyRacer\Helpers, Author: Abi
// Code lines: 669, Size of file: 19,98 KB
// Creation date: 02.09.2006 04:17
// Last modified: 02.09.2006 06:14
// Generated with Commenter by abi.exDream.com

#region Using directives
#if DEBUG
//using NUnit.Framework;
#endif
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
#endregion

namespace SpeedyRacer.Helpers
{
	/// <summary>
	/// Xml helper
	/// </summary>
	internal class XmlHelper
	{
		#region Constructor
		/// <summary>
		/// Private constructor to prevent instantiation.
		/// </summary>
		private XmlHelper()
		{
		} // XmlHelper()
		#endregion

		#region Get attributes with XmlReader
		/// <summary>
		/// Get xml attribute with help of currently open xml reader and
		/// attributeName. If attribute does not exists an empty string is
		/// returned.
		/// </summary>
		/// <param name="reader">Reader</param>
		/// <param name="attributeName">Attribute name</param>
		/// <returns>String</returns>
		public static string GetXmlAttribute(
			XmlReader reader,
			string attributeName)
		{
			string ret = reader.GetAttribute(attributeName);
			return ret == null ? "" : ret;
		} // GetXmlAttribute(reader, attributeName)

		/// <summary>
		/// Get xml int value, will return 0 if it does not exists or
		/// isn't an int. Will use the "value" attribute.
		/// </summary>
		/// <param name="reader">Reader</param>
		/// <returns>Float</returns>
		public static int GetXmlIntValue(XmlReader reader)
		{
			string str = reader.GetAttribute("value");

			int ret = 0;
			//int.TryParse(str, out ret);
			if (StringHelper.IsNumericInt(str))
				ret = Convert.ToInt32(str);

			return ret;
		} // GetXmlIntValue(reader)

		/// <summary>
		/// Get xml float value, will return 0 if it does not exists or
		/// isn't a float. Will use the "value" attribute.
		/// </summary>
		/// <param name="reader">Reader</param>
		/// <returns>Float</returns>
		public static float GetXmlFloatValue(XmlReader reader)
		{
			string str = reader.GetAttribute("value");

			float ret = 0;
			//float.TryParse(str, out ret);
			if (StringHelper.IsNumericFloat(str))
				ret = Convert.ToSingle(str, NumberFormatInfo.InvariantInfo);

			return ret;
		} // GetXmlFloatValue(reader)
		#endregion

		#region XmlDocument find and get nodes helpers
		/// <summary>
		/// Find node in the root level of the xml document (main nodes)
		/// </summary>
		/// <param name="xmlDoc">Xml doc</param>
		/// <param name="xmlNodeName">Xml node name</param>
		/// <returns>Xml node</returns>
		public static XmlNode FindNodeInRoot(XmlDocument xmlDoc,
			string xmlNodeName)
		{
			if (xmlDoc == null)
				return null;

			foreach (XmlNode node in xmlDoc.ChildNodes)
				if (StringHelper.Compare(node.Name, xmlNodeName))
					return node;

			// Not found
			return null;
		} // FindNodeInRoot(xmlDoc, xmlNodeName)

		/// <summary>
		/// Find node recursively in a xml document by its name.
		/// </summary>
		/// <param name="xmlDoc">Xml document</param>
		/// <param name="xmlNodeName">Xml node name</param>
		/// <returns>Xml node or null if not found</returns>
		public static XmlNode FindNode(XmlNode xmlDoc, string xmlNodeName)
		{
			if (xmlDoc == null)
				return null;

			foreach (XmlNode node in xmlDoc.ChildNodes)
			{
				if (String.Compare(node.Name, xmlNodeName, true) == 0)
					return node;

				XmlNode foundChildNode = FindNode(node, xmlNodeName);
				if (foundChildNode != null)
					return foundChildNode;
			} // foreach (node)

			// Not found at all.
			return null;
		} // FindNode(xmlDoc, xmlNodeName)

		/// <summary>
		/// Get xml attribute, will return "" if not found.
		/// </summary>
		/// <param name="node">Node</param>
		/// <param name="attributeName">Attribute name</param>
		/// <returns>String</returns>
		public static string GetXmlAttribute(
			XmlNode node, string attributeName)
		{
			if (node == null || node.Attributes == null)
				return "";

			foreach (XmlNode attribute in node.Attributes)
				if (StringHelper.Compare(attribute.Name, attributeName))
					return attribute.Value == null ? "" : attribute.Value;

			// Not found, just return empty string
			return "";
		} // GetXmlAttribute(node, attributeName)

		/// <summary>
		/// Get xml int value, will return 0 if it does not exists or
		/// isn't an int. Will use the "value" attribute.
		/// </summary>
		/// <param name="node">Node</param>
		/// <returns>Float</returns>
		public static int GetXmlIntValue(XmlNode node)
		{
			string str = GetXmlAttribute(node, "value");

			int ret = 0;
			//int.TryParse(str, out ret);
			if (StringHelper.IsNumericInt(str))
				ret = Convert.ToInt32(str);

			return ret;
		} // GetXmlIntValue(node)

		/// <summary>
		/// Get xml float value, will return 0 if it does not exists or
		/// isn't a float. Will use the "value" attribute.
		/// </summary>
		/// <param name="node">Node</param>
		/// <returns>Float</returns>
		public static float GetXmlFloatValue(XmlNode node)
		{
			string str = GetXmlAttribute(node, "value");

			float ret = 0;
			//float.TryParse(str, out ret);
			if (StringHelper.IsNumericFloat(str))
				ret = Convert.ToSingle(str, NumberFormatInfo.InvariantInfo);

			return ret;
		} // GetXmlFloatValue(node)
		#endregion

		#region XmlDocument create node helpers
		/// <summary>
		/// Create xml document with declaration (version 1.0, utf-8).
		/// </summary>
		/// <returns>Created xml document</returns>
		public static XmlDocument CreateXmlDocumentWithDeclaration()
		{
			XmlDocument xmlDoc = new XmlDocument();
			// Insert declaration as first element
			xmlDoc.InsertBefore(
				xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null),
				xmlDoc.DocumentElement);
			return xmlDoc;
		} // CreateXmlDocumentWithDeclaration()

		/// <summary>
		/// Create parent node
		/// </summary>
		/// <param name="xmlDoc">Xml doc</param>
		/// <param name="nodeName">Node name</param>
		/// <returns>Xml node</returns>
		public static XmlNode CreateParentNode(XmlDocument xmlDoc, string nodeName)
		{
			XmlElement parentNode = xmlDoc.CreateElement(nodeName);
			xmlDoc.AppendChild(parentNode);
			return parentNode;
		} // CreateParentNode(xmlDoc, nodeName)

		/// <summary>
		/// Create empty parent node named "ParentNode" on an empty xml document.
		/// </summary>
		/// <returns>Xml node</returns>
		public static XmlNode CreateEmptyParentNode()
		{
			// Create empty xml document
			XmlDocument xmlDoc = CreateXmlDocumentWithDeclaration();

			// Create parent node, don't add it do xmlDoc, we dispose it anyway.
			return xmlDoc.CreateElement("ParentNode");
		} // CreateEmptyParentNode()

		/// <summary>
		/// Get xml document from any node, even the xml document itself.
		/// </summary>
		/// <param name="someNode">Some node</param>
		/// <returns>Xml document</returns>
		public static XmlDocument GetXmlDocument(XmlNode someNode)
		{
			if (someNode == null)
				throw new ArgumentNullException("someNode",
					"Can't get xml document without valid node");

			// Check if someNode is our XmlDocument
			XmlDocument xmlDoc = someNode as XmlDocument;

			// If not, use the OwnerDocument property
			if (xmlDoc == null)
				xmlDoc = someNode.OwnerDocument;

			return xmlDoc;
		} // GetXmlDocument(someNode)

		/// <summary>
		/// Create child node, will get xml document from parentNode.
		/// </summary>
		/// <param name="parentNode">Parent node</param>
		/// <param name="childNodeName">Child node name</param>
		/// <returns>Xml node</returns>
		public static XmlNode CreateChildNode(
			XmlNode parentNode,
			string childNodeName)
		{
			// Create child node
			XmlNode childNode =
				GetXmlDocument(parentNode).CreateElement(childNodeName);

			// Add it to parent
			parentNode.AppendChild(childNode);

			// And return it
			return childNode;
		} // CreateChildNode(parentNode, childNodeName)

		/// <summary>
		/// Create node with attribute
		/// </summary>
		/// <param name="xmlDoc">Xml document</param>
		/// <param name="nodeName">Node name</param>
		/// <param name="attributeName">Attribute name</param>
		/// <param name="attributeValue">Attribute value</param>
		/// <returns>Xml node</returns>
		public static XmlNode CreateNodeWithAttribute(XmlDocument xmlDoc,
			string nodeName, string attributeName, string attributeValue)
		{
			XmlElement node = xmlDoc.CreateElement(nodeName);
			XmlAttribute attribute = xmlDoc.CreateAttribute(attributeName);
			attribute.Value = attributeValue;
			node.Attributes.Append(attribute);
			return node;
		} // CreateNodeWithAttribute(xmlDoc, nodeName, attributeName)
		#endregion

		#region Load xml
		/// <summary>
		/// Load xml from text and returns the first useable root (not the xml
		/// document).
		/// </summary>
		/// <param name="xmlText">Xml text</param>
		public static XmlNode LoadXmlFromText(string xmlText)
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xmlText);
			return GetRootXmlNode(xmlDoc);
		} // LoadXmlFromText(xmlText)

		/// <summary>
		/// Load xml from file
		/// </summary>
		/// <param name="xmlFilename">Xml filename</param>
		/// <returns>Xml node</returns>
		public static XmlNode LoadXmlFromFile(string xmlFilename)
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(FileHelper.LoadGameContentFile(xmlFilename));
			return GetRootXmlNode(xmlDoc);
		} // LoadXmlFromFile(xmlFilename)
		#endregion

		#region Get child node
		/// <summary>
		/// Get child node
		/// </summary>
		/// <param name="rootNode">Root node</param>
		/// <param name="childName">Child name</param>
		/// <returns>Xml node</returns>
		public static XmlNode GetChildNode(XmlNode rootNode, string childName)
		{
			if (rootNode == null)
				throw new ArgumentNullException("rootNode",
					"Need valid rootNode for GetChildNode.");

			foreach (XmlNode node in rootNode.ChildNodes)
			{
				if (node.Name == childName)
					return node;

				if (node.ChildNodes != null &&
					node.ChildNodes.Count > 0)
				{
					XmlNode result = GetChildNode(node, childName);
					if (result != null)
						return result;
				} // if (node.ChildNodes)
			} // foreach (node)

			// Not found, return null
			return null;
		} // GetChildNode(rootNode, childName)

		/// <summary>
		/// Get child node. This version does not search for a child node name,
		/// but instead we check if we can find any child with the
		/// attribute name and value as specified.
		/// </summary>
		/// <param name="rootNode">Root node</param>
		/// <param name="childAttributeName">Child attribute name</param>
		/// <param name="childAttributeValue">Child attribute value</param>
		/// <returns>Xml node</returns>
		public static XmlNode GetChildNode(XmlNode rootNode,
			string childAttributeName, string childAttributeValue)
		{
			if (rootNode == null)
				throw new ArgumentNullException("rootNode",
					"Need valid rootNode for GetChildNode.");

			foreach (XmlNode node in rootNode.ChildNodes)
			{
				if (GetXmlAttribute(node, childAttributeName) == childAttributeValue)
					return node;

				if (node.ChildNodes != null &&
					node.ChildNodes.Count > 0)
				{
					XmlNode result = GetChildNode(node,
						childAttributeName, childAttributeValue);
					if (result != null)
						return result;
				} // if (node.ChildNodes)
			} // foreach (node)

			// Not found, return null
			return null;
		} // GetChildNode(rootNode, childAttributeName, childAttributeValue)
		#endregion

		#region Get root xml node
		/// <summary>
		/// Get root xml node (not the xml document, but the first useable root)
		/// </summary>
		/// <returns>Xml node</returns>
		public static XmlNode GetRootXmlNode(XmlNode xmlNode)
		{
			if (xmlNode == null)
				throw new ArgumentNullException("xmlNode",
					"Need valid xmlNode for GetRootXmlNode.");

			// Not a document type? Then jump back to document type.
			if (xmlNode.NodeType != XmlNodeType.Document)
			{
				return GetRootXmlNode(xmlNode.OwnerDocument);
			} // if (xmlNode.NodeType)

			foreach (XmlNode node in xmlNode.ChildNodes)
			{
				if (node.NodeType != XmlNodeType.XmlDeclaration &&
					node.NodeType != XmlNodeType.ProcessingInstruction)
				{
					return node;
				} // if (node.NodeType)
			} // foreach (node)

			// Nothing found? Then return original node, there isn't any real node
			// available yet.
			return xmlNode;
		} // GetRootXmlNode(xmlNode)
		#endregion

		#region Get directory attribute
		private const string DefaultDirectoryAttributeName = "Directory";

		/// <summary>
		/// Get directory attribute
		/// </summary>
		/// <param name="node">Node</param>
		/// <returns>String</returns>
		public static string GetDirectoryAttribute(XmlNode node)
		{
			string ret = GetXmlAttribute(node, DefaultDirectoryAttributeName);
			if (String.IsNullOrEmpty(ret))
			{
				foreach (XmlNode childNode in node.ChildNodes)
					if (StringHelper.Compare(childNode.Name,
						DefaultDirectoryAttributeName))
						return childNode.Value;
			} // if (String.IsNullOrEmpty)

			return ret;
		} // GetDirectoryAttribute(node)
		#endregion

		#region Write Xml to string
		/// <summary>
		/// Write xml to string. Will return the whole xml document content
		/// as a string. Declaration stuff is cut out, but everything else
		/// is the same as written to the file when calling xmlDoc.Save.
		/// </summary>
		/// <param name="xmlDoc">Xml doc</param>
		/// <returns>String</returns>
		public static string WriteXmlToString(XmlDocument xmlDoc)
		{
			if (xmlDoc == null)
				throw new ArgumentNullException("xmlDoc",
					"WriteXmlToString requires a valid xml document.");
			
			// Save xmlDoc to a memory stream
			MemoryStream memStream = new MemoryStream();
			xmlDoc.Save(memStream);

			// Put it into a text reader
			memStream.Seek(0, SeekOrigin.Begin);
			TextReader textReader = new StreamReader(memStream);

			// And read everything
			int lineNumber = 0;
			string ret = "";
			while (textReader.Peek() >= 0)
			{
				string line = textReader.ReadLine();
				lineNumber++;
				// Skip first line (xml version and stuff)
				if (lineNumber == 1 &&
					line.Length < 200 &&
					(line.StartsWith("<?xml") ||
					 line.Substring(1).StartsWith("<?xml")))
					continue;

				// Replace &lt; to < and &gt; to >
				line = line.Replace("&lt;", "<").Replace("&gt;", ">");

				// Else just add to return string
				ret += (ret.Length == 0 ? "" : StringHelper.NewLine) + line;
			} // while (textReader.Peek)

			return ret;
		} // WriteXmlToString(xmlDoc)
		#endregion

		#region Save a system-specific type to a xml-value
		/// <summary>
		/// Convert xml type to string
		/// </summary>
		/// <param name="typeValue">Type value</param>
		/// <returns>String</returns>
		public static string ConvertXmlTypeToString(object typeValue)
		{
			// Special save handling only for the date (at the moment)
			if (typeValue.GetType() == new DateTime().GetType())
			{
				return StringHelper.WriteIsoDate((DateTime)typeValue);
			} // if (typeValue.GetType)
			else
			{
				return typeValue.ToString();
			} // else
		} // ConvertXmlTypeToString(typeValue)
		#endregion

		#region Unit Testing
#if DEBUG
		/*include NUnit.Framework for this
		/// <summary>
		/// Test xml helper class
		/// </summary>
		//[TestFixture]
		public class XmlTests
		{
			#region Test xml create and save nodes
			/// <summary>
			/// Test xml create and save nodes
			/// </summary>
			//[Test]
			public void TestXmlCreateAndSaveNodes()
			{
				// Create test xml document
				XmlDocument xmlDoc = new XmlDocument();
				XmlNode parentNode = XmlHelper.CreateParentNode(xmlDoc, "Parent");
				parentNode.AppendChild(XmlHelper.CreateNodeWithAttribute(
					xmlDoc, "Test1", "SomeNumber", "1"));

				Assert.AreEqual(
					"<Parent>" + StringHelper.NewLine +
					"  <Test1 SomeNumber=\"1\" />" + StringHelper.NewLine +
					"</Parent>",
					XmlHelper.WriteXmlToString(xmlDoc));
				/*old version
				// Save it to a memory stream
				MemoryStream memStream = new MemoryStream();
				xmlDoc.Save(memStream);

				//tst, works:
				//Console.WriteLine(
				//	StringHelper.ConvertByteArrayToString(memStream.ToArray()));
				Assert.AreEqual(
					"<Parent>" + StringHelper.NewLine +
					"  <Test1 SomeNumber=\"1\" />" + StringHelper.NewLine +
					"  <SomeVector x=\"1\" y=\"0\" z=\"0\" />" + StringHelper.NewLine +
					"</Parent>",
					StringHelper.ConvertByteArrayToString(memStream.ToArray()));
				*
			} // TestXmlCreateAndSaveNodes()
			#endregion

			#region Test write xml to string
			/// <summary>
			/// Test write xml to string
			/// </summary>
			//[Test]
			public void TestWriteXmlToString()
			{
				XmlDocument xmlDocument = XmlHelper.CreateXmlDocumentWithDeclaration();
				XmlNode parentNode = XmlHelper.CreateParentNode(xmlDocument, "Parent");
				XmlNode childNode = XmlHelper.CreateChildNode(parentNode, "Child");

				Assert.AreEqual(
					"<Parent>" + StringHelper.NewLine +
					"  <Child />" + StringHelper.NewLine +
					"</Parent>",
					XmlHelper.WriteXmlToString(xmlDocument));
			} // TestWriteXmlToString()
			#endregion

			#region Test get root xml node
			/// <summary>
			/// Test get root xml node
			/// </summary>
			//[Test]
			public void TestGetRootXmlNode()
			{
				XmlDocument xmlDoc = XmlHelper.CreateXmlDocumentWithDeclaration();
				XmlNode someNode = XmlHelper.CreateChildNode(xmlDoc, "blub");
				XmlNode someChildNode = XmlHelper.CreateChildNode(someNode, "bla");
				Assert.AreEqual(someNode, GetRootXmlNode(xmlDoc));
				Assert.AreEqual(someNode, GetRootXmlNode(someNode));
				Assert.AreEqual(someNode, GetRootXmlNode(someChildNode));
			} // TestGetRootXmlNode()
			#endregion

			#region Test get xml attribute
			/// <summary>
			/// Test get xml attribute
			/// </summary>
			//[Test]
			public void TestGetXmlAttribute()
			{
				string testDirectoryName = @"C:\Windows";
				XmlDocument xmlDoc = new XmlDocument();
				XmlNode parentNode = XmlHelper.CreateNodeWithAttribute(
					xmlDoc, "Parent", DefaultDirectoryAttributeName, testDirectoryName);
				xmlDoc.AppendChild(parentNode);
				parentNode.AppendChild(XmlHelper.CreateNodeWithAttribute(
					xmlDoc, "LittleChild", "SomeCrazyDude", "yaya"));

				Assert.AreEqual(
					"<Parent Directory=\"" + testDirectoryName + "\">" +
					StringHelper.NewLine +
					"  <LittleChild SomeCrazyDude=\"yaya\" />" + StringHelper.NewLine +
					"</Parent>",
					XmlHelper.WriteXmlToString(xmlDoc));

				Assert.AreEqual(testDirectoryName,
					GetXmlAttribute(parentNode, DefaultDirectoryAttributeName));
				// Same test with lower casing
				Assert.AreEqual(testDirectoryName,
					GetXmlAttribute(parentNode, "directory"));
			} // TestGetXmlAttribute()
			#endregion

			#region Test read xml file
			/// <summary>
			/// Test read xml file
			/// </summary>
			//[Test]
			public void TestReadXmlFile()
			{
				// Couple of examples of ShaderModule xml code ...
				string[] xmlFileContents = new string[]
				{
@"<priority value=""2"" />
<ps>
return ambientColor;
</ps>",

@"<Module value=""Texturing"" Operation=""*"">
	<Module value=""Ambient"" />
	<Module value=""Diffuse"" />
	<Module value=""Specular"" />
</Module>",

@"float2 texCoord = In.texCoord;",

@"// Project: <projectname/>, File: <filename/>
// Creation date: <creationdate/>
// Last modified: <lastmodified/>
// Author: <author/>
// Automatically generated by Shader Designer exDream (c) 2006
// Fx file mode: <fxfileexportmode/>
// Used modules: <usedmodules/>",

@"float extraValue1
<
	string UIName = ""Extra value 1"";
	string UIWidget = ""slider"";
	float UIMin = 0.0;
	float UIMax = 100.0;
	float UIStep = 0.01;
> = 0.0f;",

@"float3 GetCameraPos()
{
	return viewInverse[3].xyz;
} // GetCameraPos()",

@"string description = ""<usermodules/> shader"";",
				};

				foreach (string xmlText in xmlFileContents)
				{
					// Make sure everything is surrounded by xml (else we get errors)
					XmlNode xmlRoot = XmlHelper.LoadXmlFromText(
						"<ShaderModule>\n" +
						xmlText +
						//XmlHelper.ReplaceXmlBracketsInText(xmlText) + "\n" +
						"</ShaderModule>");
					//tst:
					//Console.WriteLine(XmlHelper.WriteXmlToString(
					//	XmlHelper.GetXmlDocument(xmlRoot)) + "\n");
				} // foreach (xmlText)
			} // TestReadXmlFile()
			#endregion
		} // class XmlTests
		*/
#endif
		#endregion
	} // class XmlHelper
} // namespace SpeedyRacer.Helpers
#endif