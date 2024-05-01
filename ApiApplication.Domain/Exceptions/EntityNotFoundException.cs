using System;

namespace ApiApplication.Domain.Exceptions {
    [Serializable]
    public class EntityNotFoundException : DomainException {
        private const string Template = "Entity not found. Entity ID: {0}";
        public EntityNotFoundException(string entityId) : base(string.Format(Template, entityId)) {
        }
    }
}
