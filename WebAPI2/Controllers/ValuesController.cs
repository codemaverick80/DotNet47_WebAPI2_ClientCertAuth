using System;
using System.Collections.Generic;
using System.Web.Http;
using WebAPI2.Filters;
using WebAPI2.Models;

namespace WebAPI2.Controllers
{
    public class ValuesController : ApiController
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
                Id = new Random().Next(105, 200),
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
                new Product{ Id=100, Name="Laptop", Description="Dell XPS 15 9500"},
                new Product{ Id=101, Name="Mobile", Description="Apple iPhone 12 Pro"},
                new Product{ Id=102, Name="Tablet", Description="Apple ipad pro 12 Inch"},
                new Product{ Id=103, Name="Monitor", Description="Benq 27 Inch Photographer Monitor"}
            };
            return plist;
        }
    }
}
