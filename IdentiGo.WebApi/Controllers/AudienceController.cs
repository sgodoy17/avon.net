using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IdentiGo.WebApi.Entities;
using IdentiGo.WebApi.Models;

namespace IdentiGo.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/audience")]
    public class AudienceController : ApiController
    {

        [Route("")]
        public IHttpActionResult Post(AudienceModel audienceModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Audience newAudience = AudiencesStore.AddAudience(audienceModel.Data);

            return Ok(newAudience);
        }
    }
}