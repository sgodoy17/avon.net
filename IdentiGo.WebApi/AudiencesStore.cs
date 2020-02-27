using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using IdentiGo.WebApi.Entities;
using Microsoft.Owin.Security.DataHandler.Encoder;

namespace IdentiGo.WebApi
{

    public static class AudiencesStore
    {
        public static ConcurrentDictionary<string, Audience> AudiencesList = new ConcurrentDictionary<string, Audience>();
        public static string ClientId = "099153c2625149bc8ecb3e85e03f0022";
        public static string Base64Secret = "IxrAjDoa2FqElO7IhrSrUJELhUckePEPVpaePlS_Xaw";
        static AudiencesStore()
        {
            AudiencesList.TryAdd("099153c2625149bc8ecb3e85e03f0022",
                                new Audience
                                {
                                    ClientId = ClientId,
                                    Base64Secret = Base64Secret,
                                    Name = "ResourceServer.Api 1"
                                });
        }

        public static Audience AddAudience(string name)
        {
            var clientId = Guid.NewGuid().ToString("N");

            var key = new byte[32];
            RNGCryptoServiceProvider.Create().GetBytes(key);
            var base64Secret = TextEncodings.Base64Url.Encode(key);

            var newAudience = new Audience { ClientId = clientId, Base64Secret = base64Secret, Name = name };
            AudiencesList.TryAdd(clientId, newAudience);
            return newAudience;
        }

        public static Audience FindAudience(string clientId)
        {
            Audience audience = null;
            return AudiencesList.TryGetValue(clientId, out audience) ? audience : null;
        }
    }

}