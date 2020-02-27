using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Security;

namespace Component.Transversal.Utilities
{
    class CustomSecurityServerOutputFilter : SendSecurityFilter
    {
        public CustomSecurityServerOutputFilter(CustomPolicyAssertions parentAssertion)
            : base(parentAssertion.ServiceActor, false)
        {
        }

        public override void SecureMessage(SoapEnvelope envelope, Security security)
        {
            RequestState state = envelope.Context.OperationState.Get<RequestState>();
            security.Tokens.Add(state.ServerToken);
            security.Elements.Add(new MessageSignature(state.ServerToken));
            security.Elements.Add(new EncryptedData(state.ClientToken));
        }
    }
}
