using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Security;
using Microsoft.Web.Services3.Security.Tokens;
using System;

namespace Component.Transversal.Utilities
{
    class CustomSecurityServerInputFilter : ReceiveSecurityFilter
    {
        public CustomSecurityServerInputFilter(CustomPolicyAssertions parentAssertion)
            : base(parentAssertion.ServiceActor, false)
        {
        }

        public override void ValidateMessageSecurity(SoapEnvelope envelope, Security security)
        {
            SecurityToken clientToken = null;
            SecurityToken serverToken = null;

            foreach (ISecurityElement elem in security.Elements)
            {
                if (elem is MessageSignature sig)
                {
                    clientToken = sig.SigningToken;
                }

                if (elem is EncryptedData enc)
                {
                    serverToken = enc.SecurityToken;
                }
            }

            if (clientToken == null || serverToken == null)
                throw new Exception("El mensaje entrante no cumple con los requisitos de seguridad");

            RequestState state = new RequestState(clientToken, serverToken);
            envelope.Context.OperationState.Set(state);
        }
    }
}
