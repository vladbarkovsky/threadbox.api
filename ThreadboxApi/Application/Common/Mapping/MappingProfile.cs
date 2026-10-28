using AutoMapper;
using System.Reflection;

namespace ThreadboxApi.Application.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            List<Type> types = assembly
                .GetExportedTypes()
                .Where(bool (Type type) => type.GetInterfaces().Contains(typeof(IMapped)))
                .ToList();

            foreach (Type type in types)
            {
                object instance = Activator.CreateInstance(type);
                MethodInfo methodInfo = type.GetMethod(nameof(IMapped.Mapping));
                methodInfo.Invoke(instance, new object[] { this });
            }
        }
    }
}
