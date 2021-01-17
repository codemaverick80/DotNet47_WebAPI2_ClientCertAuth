using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.Models
{
    public class ApiResponse<T>
    {

        public System.Net.HttpStatusCode Status { get; set; }
        public T Data { get; set; }
        public ApiRequestErrorModel Error { get; set; }

    }


    public class ApiRequestErrorModel
    {
        public string ErrotType { get; set; }
        public string ErrorMessage { get; set; }

    }




}