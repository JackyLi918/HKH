/*******************************************************
 * Filename: WebPartScripts.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	7/16/2016 5:35:54 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System.Web;
using System.Web.Optimization;

namespace MEF.Mvc.WebParts
{
    /// <summary>
    /// WebPartScripts
    /// </summary>
    public static class WebPartScripts
    {
        public static IHtmlString Render(IWebPart webPart, params string[] paths)
        {
            if (webPart == null)
                return Scripts.Render(paths);

            string[] tmp = new string[paths.Length];
            for (int i = 0; i < paths.Length; i++)
            {
                tmp[i] = webPart.MapPath(paths[i]);
            }
            return Scripts.Render(tmp);
        }
    }
}