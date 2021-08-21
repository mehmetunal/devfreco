using System;
using MongoDB.Bson;
using Dev.Core.Entities;
using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Dev.Data.Mongo
{

    public abstract class BaseEntity : Data.BaseEntity, IBaseEntity, IEntity
    {
        protected BaseEntity()
        {
            _id = ObjectId.GenerateNewId();
        }

        [BsonId]
        [DataMember]
        private ObjectId _id;

        [DataMember]
        public ObjectId Id
        {
            get => _id;
            set => _id = value;
        }

        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public string CreatedUserID { get; set; }
        [DataMember]
        public string UpdatedUserID { get; set; }
    }
}
