﻿using AutoMapper;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Models
{
	/// <summary>
	/// File with data in byte array
	/// </summary>
	public class ThreadboxFile : IMapped
	{
		/// <summary>
		/// File name with extension ('file.ext')
		/// </summary>
		public string Name { get; set; } = null!;

		/// <summary>
		/// File extension ('.ext')
		/// </summary>
		public string Extension { get; set; } = null!;

		/// <summary>
		/// Content-Type HTTP header value
		/// </summary>
		public string ContentType { get; set; } = null!;

		public byte[] Data { get; set; } = null!;

		public void Mapping(Profile profile)
		{
			// Mapping from multipart/form-data (file sending to server)
			profile.CreateMap<IFormFile, ThreadboxFile>()
				.ForMember(d => d.Name, o => o.MapFrom(s => s.FileName))
				.ForMember(d => d.Extension, o => o.MapFrom(s => Path.GetExtension(s.FileName)))
				.ForMember(d => d.Data, o => o.MapFrom<FileDataResolver>());
		}

		/// <summary>
		/// Converts multipart/form-data file to byte array
		/// </summary>
		public class FileDataResolver : IValueResolver<IFormFile, ThreadboxFile, byte[]>
		{
			public byte[] Resolve(IFormFile source, ThreadboxFile destination, byte[] destMember, ResolutionContext context)
			{
				using var memoryStream = new MemoryStream();
				source.CopyTo(memoryStream);
				return memoryStream.ToArray();
			}
		}
	}
}