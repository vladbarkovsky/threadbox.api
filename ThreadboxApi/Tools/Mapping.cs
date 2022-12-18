using AutoMapper;
using System.Reflection;

namespace ThreadboxApi.Tools
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }

    public interface IMapped
    {
        void Mapping(Profile profile);
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            var assembly = Assembly.GetExecutingAssembly();

            ApplyMappingsFromAssembly(assembly);
            ApplyManualMappingsFromAssembly(assembly);
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly
                .GetExportedTypes()
                .Where(x => x
                    .GetInterfaces()
                    .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);

                var methodInfo = type.GetMethod("Mapping") ?? type
                    .GetInterface("IMapFrom`1")!
                    .GetMethod("Mapping");

                methodInfo!.Invoke(instance, new object[] { this });
            }
        }

        private void ApplyManualMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly
                .GetExportedTypes()
                .Where(x => x
                    .GetInterfaces()
                    .Contains(typeof(IMapped)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                methodInfo!.Invoke(instance, new object[] { this });
            }
        }
    }
}