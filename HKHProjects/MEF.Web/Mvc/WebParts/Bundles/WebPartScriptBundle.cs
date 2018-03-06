/*******************************************************
 * Filename: WebPartScriptBundle.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	7/16/2016 5:29:56 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System.Web.Optimization;

namespace MEF.Mvc.WebParts
{
    /// <summary>
    /// WebPartScriptBundle
    /// </summary>
    public class WebPartScriptBundle : ScriptBundle
    {
        protected IWebPart webPart = null;

        public WebPartScriptBundle(IWebPart webPart, string virtualPath)
            : base(webPart == null ? virtualPath : webPart.MapPath(virtualPath))
        {
            this.webPart = webPart;
        }

        public WebPartScriptBundle(IWebPart webPart, string virtualPath, string cdnPath)
            : base(webPart == null ? virtualPath : webPart.MapPath(virtualPath), cdnPath)
        {
            this.webPart = webPart;
        }

        public override Bundle Include(params string[] virtualPaths)
        {
            return base.Include(MapPaths(virtualPaths));
        }

        public override Bundle Include(string virtualPath, params IItemTransform[] transforms)
        {
            if (webPart == null)
                return base.Include(virtualPath, transforms);

            return base.Include(webPart.MapPath(virtualPath), transforms);
        }

        private string[] MapPaths(string[] virtualPaths)
        {
            if (webPart == null)
                return virtualPaths;

            string[] tmp = new string[virtualPaths.Length];
            for (int i = 0; i < virtualPaths.Length; i++)
            {
                tmp[i] = webPart.MapPath(virtualPaths[i]);
            }
            return tmp;
        }
    }
}