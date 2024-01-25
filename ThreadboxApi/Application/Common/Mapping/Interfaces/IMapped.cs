using AutoMapper;

namespace ThreadboxApi.Application.Common.Mapping.Interfaces
{
    /// <summary>
    /// Allows to configure mappings inside implementing class
    /// </summary>
    public interface IMapped
    {
        void Mapping(Profile profile);
    }
}