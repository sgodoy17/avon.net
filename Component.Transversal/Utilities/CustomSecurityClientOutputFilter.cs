using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Design;
using Microsoft.Web.Services3.Security;
using Microsoft.Web.Services3.Security.Tokens;
using System.Security.Cryptography.X509Certificates;

namespace Component.Transversal.Utilities
{
    class CustomSecurityClientOutputFilter : SendSecurityFilter
    {
        SecurityToken clientToken;
        SecurityToken serverToken;

        public CustomSecurityClientOutputFilter(CustomPolicyAssertions parentAssertion)
            : base(parentAssertion.ServiceActor, true)
        {
            clientToken = X509TokenProvider.CreateToken(StoreLocation.CurrentUser, StoreName.My, "CN=WSE2QuickStartClient");
            serverToken = X509TokenProvider.CreateToken(StoreLocation.LocalMachine, StoreName.My, "CN=WSE2QuickStartServer");
        }

        public override void SecureMessage(SoapEnvelope envelope, Security security)
        {
            security.Tokens.Add(clientToken);
            security.Elements.Add(new MessageSignature(clientToken));
            security.Elements.Add(new EncryptedData(serverToken));
            security.Elements.Add(new EncryptedData(serverToken, "#" + clientToken.Id));
            RequestState state = new RequestState(clientToken, serverToken);
            envelope.Context.OperationState.Set(state);
        }
    }
}
