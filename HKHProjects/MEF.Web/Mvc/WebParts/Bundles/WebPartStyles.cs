/*******************************************************
 * Filename: WebPartStyles.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	7/16/2016 5:36:28 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Optimization;

namespace MEF.Mvc.WebParts
{
    /// <summary>
    /// WebPartStyles
    /// </summary>
    public static class WebPartStyles
    {
        public static IHtmlString Render(IWebPart webPart, params string[] paths)
        {
            if (webPart == null)
                return Styles.Render(paths);

            string[] tmp = new string[paths.Length];
            for (int i = 0; i < paths.Length; i++)
            {
                tmp[i] = webPart.MapPath(paths[i]);
            }
            return Styles.Render(tmp);
        }
    }
}