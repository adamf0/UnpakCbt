using System.Reflection;
using System.Runtime.Loader;

namespace UnpakCbt.Api.Security
{
    public static class SecurityConfig
    {
        public static void PreventDynamicCodeExecution()
        {
            // 🔹 Blokir Assembly.Load (hanya mendeteksi, tidak bisa mencegah di .NET Core)
            AppDomain.CurrentDomain.AssemblyLoad += (sender, args) =>
            {
                throw new UnauthorizedAccessException($"Dynamic assembly loading is not allowed: {args.LoadedAssembly.FullName}");
            };

            // 🔹 Blokir Resolving Assembly (paling efektif di .NET Core)
            AssemblyLoadContext.Default.Resolving += (context, assemblyName) =>
            {
                throw new UnauthorizedAccessException($"Dynamic assembly loading is blocked: {assemblyName}");
            };

            // 🔹 Blokir Kompilasi Runtime (Reflection.Emit & CodeDom)
            var fields = typeof(Type).GetFields(BindingFlags.NonPublic | BindingFlags.Static);
            foreach (var field in fields)
            {
                if (field.Name.Contains("ReflectionEmit") || field.Name.Contains("CodeDom"))
                {
                    try
                    {
                        field.SetValue(null, null);
                    }
                    catch
                    {
                        // Ignore error jika field tidak bisa diubah
                    }
                }
            }

        }
    }
}
