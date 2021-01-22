using System;
using System.Collections.Generic;
using System.Web.Http;
using WebAPI2.Filters;
using WebAPI2.Models;

namespace WebAPI2.Controllers
{
    public class ProductController : ApiController
    {
        [RequireHttps]
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok<List<Product>>(GetProducts());
        }

        [RequireHttps]
        [HttpPost]
        public ApiResponse<Product> SaveProduct([FromBody] ProductPostRequest postRequest)
        {

            Product prod = new Product()
            {
                Id = Guid.NewGuid(),
                Name = postRequest.ProductName,
                Description = postRequest.Description
            };

            var response = new ApiResponse<Product>()
            {
                Data = prod,
                Error = null,
                Status = System.Net.HttpStatusCode.OK
            };
            return response;
        }

        private List<Product> GetProducts()
        {
            List<Product> plist = new List<Product>
            {
                new Product{ Id=Guid.Parse("eb4950d9-48b4-40cc-8c73-7e29e52b52fd"), Name="Laptop", Description="Dell XPS 15 9500"},
                new Product{ Id=Guid.Parse("abbecc95-836e-4598-91c9-60f62ebc7997"), Name="Mobile", Description="Apple iPhone 12 Pro"},
                new Product{ Id=Guid.Parse("e5c3918e-39a7-4068-a55b-d74fe9bff9f5"), Name="Tablet", Description="Apple ipad pro 12 Inch"},
                new Product{ Id=Guid.Parse("ff4d5e9e-8076-4111-a819-dc8e653b40fa"), Name="Monitor", Description="Benq 27 Inch Photographer Monitor"}
            };
            return plist;
        }
    }
}
