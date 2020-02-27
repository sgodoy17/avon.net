using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Security;
using System;

namespace Component.Transversal.Utilities
{
    class CustomSecurityClientInputFilter : ReceiveSecurityFilter
    {
        public CustomSecurityClientInputFilter(CustomPolicyAssertions parentAssertion)
            : base(parentAssertion.ServiceActor, true)
        {
        }

        public override void ValidateMessageSecurity(SoapEnvelope envelope, Security security)
        {
            RequestState state;
            bool signed = false;
            bool encrypted = false;
            state = envelope.Context.OperationState.Get<RequestState>();

            foreach (ISecurityElement elem in security.Elements)
            {
                if (elem is MessageSignature sig)
                {
                    if (sig.SigningToken.Equals(state.ClientToken))
                        signed = true;
                }

                if (elem is EncryptedData enc)
                {
                    if (enc.SecurityToken.Equals(state.ClientToken))
                        encrypted = true;
                }
            }

            if (!signed || !encrypted)
                throw new Exception("Mensaje de respuesta no cumple con los requisitos de seguridad");
        }
    }
}
