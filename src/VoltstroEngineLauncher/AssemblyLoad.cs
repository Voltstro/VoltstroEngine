using System.Reflection;
using System.Runtime.Loader;

namespace VoltstroEngineLauncher
{
	internal sealed class AssemblyLoad : AssemblyLoadContext
	{
		private string gamePath;

		public Assembly LoadAssembly(string path, string dllFile)
		{
			gamePath = path;
			Resolving += OnResolving;
			return LoadFromAssemblyPath($"{path}/{dllFile}");
		}

		private Assembly OnResolving(AssemblyLoadContext context, AssemblyName assemblyName)
		{
			//We fail to load it
			Assembly assembly = context.LoadFromAssemblyPath($"{gamePath}/{assemblyName.Name}.dll");
			return assembly;
		}
	}
}