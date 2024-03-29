﻿namespace ThreadboxApi.ORM.Entities
{
    public class ThreadImage : BaseEntity
    {
        public Guid FileInfoId { get; set; }
        public FileInfo FileInfo { get; set; }
        public Guid ThreadId { get; set; }
        public Thread Thread { get; set; }
    }
}