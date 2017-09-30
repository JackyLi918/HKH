using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Compilation;
using System.Web.Hosting;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

[assembly: PreApplicationStartMethod(typeof(MEF.Mvc.PreApplicationInit), "Initialize")]
namespace MEF.Mvc
{
    public class PreApplicationInit
    {
        private static bool _isStarting;

        /// <summary>
        /// The source WebPart folder from which to shadow copy from
        /// </summary>
        private static readonly DirectoryInfo WebPartsFolder;

        /// <summary>
        /// The folder to shadow copy the WebPart DLLs to use for running the app
        /// </summary>
        private static readonly DirectoryInfo ShadowCopyFolder;

        static PreApplicationInit()
        {
            WebPartsFolder = new DirectoryInfo(HostingEnvironment.MapPath("~/" + MEF.Mvc.WebParts.WebPartsManager.WebPartFolderName));
            ShadowCopyFolder = new DirectoryInfo(HostingEnvironment.MapPath("~/" + MEF.Mvc.WebParts.WebPartsManager.WebPartFolderName + "/bin"));
        }

        public static void Initialize()
        {
            if (!_isStarting)
            {
                _isStarting = true;
                DynamicModuleUtility.RegisterModule(typeof(RequestLifetimeHttpModule));

                if (!Directory.Exists(ShadowCopyFolder.FullName))
                    Directory.CreateDirectory(ShadowCopyFolder.FullName);

                //clear out plugins)
                foreach (var f in ShadowCopyFolder.GetFiles("*.dll", SearchOption.AllDirectories))
                {
                    f.Delete();
                }

                //shadow copy files
                foreach (var plug in WebPartsFolder.GetFiles("*.dll", SearchOption.AllDirectories))
                {
                    File.Copy(plug.FullName, Path.Combine(ShadowCopyFolder.FullName, plug.Name), true);
                }

                foreach (var a in
                    ShadowCopyFolder
                    .GetFiles("*.dll", SearchOption.AllDirectories)
                    .Select(x => AssemblyName.GetAssemblyName(x.FullName))
                    .Select(x => Assembly.Load(x.FullName)))
                {
                    BuildManager.AddReferencedAssembly(a);
                }
            }
        }
    }
}
