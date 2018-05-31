﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace OAuthSample.API.Operations
{
    public class Security : IOperationFilter
    {
        public void Apply(Operation operation,
                          SchemaRegistry schemaRegistry,
                          ApiDescription apiDescription)
        {
            var actFilters = apiDescription.ActionDescriptor.GetFilterPipeline();
            var allowsAnonymous = actFilters.Select(f => f.Instance).OfType<OverrideAuthorizationAttribute>().Any();
            if (allowsAnonymous)
                return;

            if (operation.security == null)
                operation.security = new List<IDictionary<string, IEnumerable<string>>>();

            var oAuthRequirements =
                new Dictionary<string, IEnumerable<string>>
                {
                    {
                        "oauth2",
                        new List<string> { "all", "user", "order" }
                    }
                };

            operation.security.Add(oAuthRequirements);
        }
    }
}