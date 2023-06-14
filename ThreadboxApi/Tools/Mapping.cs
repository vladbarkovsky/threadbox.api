using AutoMapper;
using System.Reflection;
using ThreadboxApi.Domain.Common;
using ThreadboxApi.Domain.Entities;

namespace ThreadboxApi.Tools
{
    /// <summary>
    /// Allows to configure mappings inside implementing class
    /// </summary>
    public interface IMapped
    {
        void Mapping(Profile profile);
    }

    /// <summary>
    /// Creates simple mapping from <typeparamref name="TSource"/> without any configuration
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public interface IMappedFrom<TSource>
    {
        void Mapping(Profile profile)
        {
            profile.CreateMap(typeof(TSource), GetType());
        }
    }

    /// <summary>
    /// Creates simple mapping from <typeparamref name="TSource"/> without any configuration
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public interface IMapFromFormFile
    {
        void Mapping(Profile profile)
        {
            profile.CreateMap(typeof(IFormFile), GetType())
                .ForMember("File", o => o.MapFrom(s => s));
        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            var assembly = Assembly.GetExecutingAssembly();

            ApplyAutomaticMappingsFromAssembly(assembly);
            ApplyManualMappingsFromAssembly(assembly);
        }

        private void ApplyAutomaticMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly
                .GetExportedTypes()
                .Where(x => x.GetInterfaces().Any(x => IsIMappedFrom(x)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);

                var methodInfos = type
                    .GetInterfaces()
                    .Where(x => IsIMappedFrom(x))
                    .Select(x => x.GetMethod("Mapping"));

                foreach (var methodInfo in methodInfos)
                {
                    methodInfo.Invoke(instance, new object[] { this });
                }
            }
        }

        private static bool IsIMappedFrom(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IMappedFrom<>);
        }

        private void ApplyManualMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly
                .GetExportedTypes()
                .Where(x => x.GetInterfaces().Contains(typeof(IMapped)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                methodInfo.Invoke(instance, new object[] { this });
            }
        }
    }
}