using System;

namespace WebAPI2.Models
{
    public class CertificateInfo
    {
        public string Issuer { get; set; }
        public string Subject { get; set; }
        public string Thumbprint { get; set; }

        public DateTime ExpireDate { get; set; }
        public DateTime ValidDateFrom { get; set; }

    }
}
