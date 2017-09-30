using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Net;
using System.Web.Services.Description;
using System.Xml.Serialization;

namespace HKH.WCF
{
	public class WebServiceProxyGenerator
	{
		public static void Generate(string nameSpace, string serviceUri, string srcFile)
		{
			WebClient client = new WebClient();
			Stream stream = client.OpenRead(serviceUri.EndsWith("?wsdl") ? serviceUri : (serviceUri + "?wsdl"));

			ServiceDescription desc = ServiceDescription.Read(stream);
			
			ServiceDescriptionImporter importor = new ServiceDescriptionImporter();
			importor.ProtocolName = "Soap";
			importor.Style = ServiceDescriptionImportStyle.Client;
			importor.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties | CodeGenerationOptions.GenerateNewAsync;
			importor.AddServiceDescription(desc, null, null);

			CodeNamespace nsp = string.IsNullOrEmpty(nameSpace) ? new CodeNamespace() : new CodeNamespace(nameSpace);
			CodeCompileUnit unit = new CodeCompileUnit();
			unit.Namespaces.Add(nsp);

			ServiceDescriptionImportWarnings warnings = importor.Import(nsp, unit);

			CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp");
			TextWriter tWriter = File.CreateText(srcFile);

			codeProvider.GenerateCodeFromCompileUnit(unit, tWriter, null);

			tWriter.Flush();
			tWriter.Close();
		}
	}
}
