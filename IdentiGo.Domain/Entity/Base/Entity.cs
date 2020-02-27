using System;
using System.Runtime.Serialization;

namespace IdentiGo.Domain.Entity.Base
{


    /// <summary>
    /// Clase base para las entidades
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public abstract class Entity
    {
        #region Members

        int? _requestedHashCode;
        Guid _id;

        #endregion

        #region Properties

        /// <summary>
        /// Obtiene o establece el identificador de persistencia
        /// </summary>
        [DataMember]
        public virtual Guid Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;

                OnIdChanged();
            }
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Cuan el ID de la entidad es cambiado
        /// </summary>
        protected virtual void OnIdChanged()
        {

        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Verifica si se trata de una entidad transitoria, es decir, sin identidad
        /// </summary>
        /// <returns>Verdadero en caso de ser trensitoria, de lo contrario False</returns>
        public bool IsTransient()
        {
            return this.Id == Guid.Empty;
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// <see cref="M:System.Object.Equals"/>
        /// </summary>
        /// <param name="obj"><see cref="M:System.Object.Equals"/></param>
        /// <returns><see cref="M:System.Object.Equals"/></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            Entity item = (Entity)obj;

            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return item.Id == this.Id;
        }

        /// <summary>
        /// <see cref="M:System.Object.GetHashCode"/>
        /// </summary>
        /// <returns><see cref="M:System.Object.GetHashCode"/></returns>
        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = this.Id.GetHashCode() ^ 31;

                return _requestedHashCode.Value;
            }
            else
                return base.GetHashCode();

        }

        public static bool operator ==(Entity left, Entity right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }

        #endregion
    }
}
