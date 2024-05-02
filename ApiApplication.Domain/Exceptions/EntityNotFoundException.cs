using System;

namespace ApiApplication.Domain.Exceptions {
    [Serializable]
    public class EntityNotFoundException : Exception {
        private const string Template = "Entity {0} not found. Entity ID: {1}";
        public EntityNotFoundException(string entityId, string entityName) : base(string.Format(Template, entityName, entityId)) {
        }

        public EntityNotFoundException(int entityId, string entityName) : base(string.Format(Template, entityName, entityId)) {
        }
    }
}
