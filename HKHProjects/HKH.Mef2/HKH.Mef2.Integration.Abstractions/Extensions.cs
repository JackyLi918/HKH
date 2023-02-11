using System.Composition.Convention;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyInjection;

namespace HKH.Mef2.Integration
{
    public static class Extensions
    {
        /// <summary>
        //  Add part types from a list of assemblies in the specific Directory to the container. If a part type does
        //  not have any exports it will be ignored.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="path">directory path</param>
        /// <param name="searchPattern">The search string to match against the names of files in path. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions. default value is "*.dll". </param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include all subdirectories or only the current directory. default value is SearchOption.TopDirectoryOnly. </param>
        /// <param name="conventions">Conventions represented by a System.Composition.Convention.AttributedModelProvider, or null. </param>
        /// <returns>A configuration object allowing configuration to continue.</returns>
        public static ContainerConfiguration WithAssemblies(this ContainerConfiguration configuration, string path, string searchPattern = "*.dll", SearchOption searchOption = SearchOption.AllDirectories, AttributedModelProvider conventions = null)
        {
            var assemblies = Directory
                .GetFiles(path, searchPattern, searchOption)
                .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath);

            return configuration.WithAssemblies(assemblies, conventions);
        }

        public static void EnableMef2(this IServiceCollection builder, ContainerConfiguration mefContainer)
        {
            mefContainer.WithAssembly(typeof(IResolver).Assembly);  //register DIContainerWrapper
            builder.AddSingleton(typeof(ContainerConfiguration), mefContainer);
            builder.AddScoped<IResolver, DefaultResolver>();
            builder.AddScoped<CompositionContainer, CompositionContainer>();
        }
    }
}