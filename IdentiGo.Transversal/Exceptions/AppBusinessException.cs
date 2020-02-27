using System;

namespace IdentiGo.Transversal.Exceptions
{
    public class AppBusinessException : Exception
    {
        #region Properties

        const string BaseMessage = "Se ha producido una Excepción de Negocio";

        private readonly string _businessMessage;

        public string BusinessMessage
        {
            get
            {
                return _businessMessage;
            }
        }

        #endregion

        #region Constructor

        public AppBusinessException(string businessMessage, Exception innerException)
            : base(BaseMessage, innerException)
        {
            _businessMessage = businessMessage;
        }

        public AppBusinessException(string businessMessage)
            : base(businessMessage)
        {
            _businessMessage = businessMessage;
        }

        public AppBusinessException(string businessMessage, params object[] args)
            : base(BaseMessage)
        {
            _businessMessage = string.Format(businessMessage, args);
        }

        #endregion

        #region Public Functions

        public virtual string ToShortString()
        {
            return string.Format("{0} <> {1}", base.Message, this.BusinessMessage);

        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            string inner = string.Empty;
            Exception ex = this.InnerException;

            while (ex != null)
            {
                inner += ex + Environment.NewLine;
                ex = ex.InnerException;
            }
            if (!string.IsNullOrEmpty(inner))
                return string.Format("{1} {0} Trace: {2} {0} Inner: {3}", Environment.NewLine, this.ToShortString(), this.StackTrace, inner);
            else
                return string.Format("{1} {0} Trace: {2} {0}", Environment.NewLine, this.ToShortString(), this.StackTrace);

        }

        #endregion
    }
}
