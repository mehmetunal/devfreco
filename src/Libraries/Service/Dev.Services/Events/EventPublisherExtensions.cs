﻿using Dev.Data;
using Dev.Data.Events;
using System.Collections.Generic;

namespace Dev.Services.Events
{
    /// <summary>
    /// Event publisher extensions
    /// </summary>
    public static class EventPublisherExtensions
    {
        /// <summary>
        /// Entity inserted
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="entity">Entity</param>
        public static void EntityInserted<T>(this IEventPublisher eventPublisher, T entity) where T : BaseEntity
        {
            eventPublisher.Publish(new EntityInsertedEvent<T>(entity));
        }

        /// <summary>
        /// Entity updated
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="entity">Entity</param>
        public static void EntityUpdated<T>(this IEventPublisher eventPublisher, T entity) where T : BaseEntity
        {
            eventPublisher.Publish(new EntityUpdatedEvent<T>(entity));
        }
        public static void EntityUpdated<T>(this IEventPublisher eventPublisher, List<T> entities) where T : BaseEntity
        {
            eventPublisher.Publish(new EntityUpdatedEvent<T>(entities));
        }

        /// <summary>
        /// Entity deleted
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="entity">Entity</param>
        public static void EntityDeleted<T>(this IEventPublisher eventPublisher, T entity) where T : BaseEntity
        {
            eventPublisher.Publish(new EntityDeletedEvent<T>(entity));
        }
    }
}