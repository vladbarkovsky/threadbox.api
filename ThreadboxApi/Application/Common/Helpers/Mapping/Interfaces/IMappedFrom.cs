using AutoMapper;

namespace ThreadboxApi.Application.Common.Helpers.Mapping.Interfaces
{
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
}