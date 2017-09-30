//                              _oo0oo_
//                             o8888888o
//                             88" . "88
//                             (| -_- |)
//                             0\  =  /0
//                           ___/'---'\___
//                        .' \\\|     |// '.
//                       / \\\|||  :  |||// \\
//                      / _ ||||| -:- |||||- \\
//                      | |  \\\\  -  /// |   |
//                      | \_|  ''\---/''  |_/ |
//                      \  .-\__  '-'  __/-.  /
//                    ___'. .'  /--.--\  '. .'___
//                 ."" '<  '.___\_<|>_/___.' >'  "".
//                | | : '-  \'.;'\ _ /';.'/ - ' : | |
//                \  \ '_.   \_ __\ /__ _/   .-' /  /
//            ====='-.____'.___ \_____/___.-'____.-'=====
//                              '=---='
//
//          ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
//
//                  佛祖保佑                 永无BUG
using System;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace HKH.Common
{
	/// <summary>
	/// Eval Expression
	/// </summary>
	/// <remarks>
	/// Create By jacky on 2007-3-2
	/// Make sure the expression is calculatable
	/// </remarks>
	public class XExpression
	{
		private Assembly assembly = null;			        //dynamic assembly
		private const string className = "XExpression";		//dyanmic class
		private const string methodName = "Eval";		    //dynamic method

		/// <summary>
		/// 
		/// </summary>
		/// <param name="expression">the expression to calculate</param>
		public XExpression(string expression)
		{
			//it's difficult to validate the expression if math function included
			//0.5+Math.Log(10,2)+100   Math.Log(8,2)*100   100/Math.Log(12,2)
			//				if (!Regex.IsMatch(expression,@"") )
			//				{
			//					throw new Exception("Expression isn't well format");
			//				}

			//keep result is double for division, even though int divided by int.
			expression = expression.Replace("/", "*1.0/");

			CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp");
			CompilerParameters paras = new CompilerParameters();
			paras.GenerateExecutable = false;
			paras.GenerateInMemory = true;

			StringBuilder classSource = new StringBuilder();
			classSource.Append("using System;");
			classSource.Append("public class " + className);
			classSource.Append("{");
			classSource.Append("	public object " + methodName + "( )");
			classSource.Append("    {");
			classSource.Append("		return " + expression + ";");
			classSource.Append("    }");
			classSource.Append("}");

			CompilerResults result = codeProvider.CompileAssemblyFromSource(paras, classSource.ToString());

			assembly = result.CompiledAssembly;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public object Eval()
		{
			object oClass = assembly.CreateInstance(className);
			MethodInfo method = oClass.GetType().GetMethod(methodName);

			return method.Invoke(oClass, null);
		}
	}
}
