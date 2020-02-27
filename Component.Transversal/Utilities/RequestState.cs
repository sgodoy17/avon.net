using Microsoft.Web.Services3.Security.Tokens;

namespace Component.Transversal.Utilities
{
    class RequestState
    {
        SecurityToken clientToken;
        SecurityToken serverToken;

        public RequestState(SecurityToken cToken, SecurityToken sToken)
        {
            clientToken = cToken;
            serverToken = sToken;
        }

        public SecurityToken ClientToken
        {
            get { return clientToken; }
        }

        public SecurityToken ServerToken
        {
            get { return serverToken; }
        }
    }
}