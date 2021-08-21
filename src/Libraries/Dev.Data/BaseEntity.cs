﻿using Dev.Core.Entities;
using System;

namespace Dev.Data
{
    [Serializable]
    public class BaseEntity : BaseEntity<Guid>
    {
    }

    [Serializable]
    public abstract class BaseEntity<TKey> : IBaseEntity<TKey>, IEntity
    {
        public BaseEntity()
        {
            ObjectId = Guid.NewGuid();
            CreatedDate = DateTime.UtcNow;
            CreatorIP = "";
            CreatorUserId = Guid.NewGuid();
        }
        public TKey Id { get; set; }
        public Guid ObjectId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatorIP { get; set; }
        public Guid CreatorUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifierIP { get; set; }
        public Guid? ModifierUserId { get; set; }
    }
}
