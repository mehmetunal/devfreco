using System.Collections.Generic;

namespace Dev.Data.Events
{
    /// <summary>
    /// A container for entities that are updated.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityUpdatedEvent<T> where T : BaseEntity
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="entity">Entity</param>
        public EntityUpdatedEvent(T entity)
        {
            Entity = entity;
        }

        public EntityUpdatedEvent(List<T> entities)
        {
            Entities = entities;
        }

        /// <summary>
        /// Entity
        /// </summary>
        public T Entity { get; }
        public List<T> Entities { get; }
    }
}
