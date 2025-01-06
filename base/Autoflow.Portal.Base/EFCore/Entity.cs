﻿using System.ComponentModel.DataAnnotations;
using Autoflow.Portal.Base.EFCore.Utils;

namespace Autoflow.Portal.Base.EFCore
{
    [Serializable]
    public abstract class Entity : IEntity
    {
        protected Entity() { }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"[ENTITY: {GetType().Name}] Keys = {GetKeys().JoinAsString(", ")}";
        }

        public abstract object?[] GetKeys();
    }

    [Serializable]
    public abstract class Entity<TKey> : Entity, IEntity<TKey>
    {
        [Key]
        public TKey Id { get; protected set; } = default!;

        protected Entity() { }

        public Entity(TKey id)
        {
            Id = id;
        }

        public override object?[] GetKeys()
        {
            return [Id];
        }

        public override string ToString()
        {
            return $"[ENTITY: {GetType().Name}] Id = {Id}";
        }
    }
}
