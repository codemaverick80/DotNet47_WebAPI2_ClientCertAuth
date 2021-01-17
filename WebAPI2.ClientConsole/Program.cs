using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace WebAPI2.ClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            GetApiDataAndSendClientCertToApi();
        }



        public static void GetApiDataAndSendClientCertToApi()
        {
            X509Certificate2 clientCert = GetClientCertificate();
            WebRequestHandler requestHandler = new WebRequestHandler();

            requestHandler.ClientCertificates.Add(clientCert);

            HttpClient client = new HttpClient(requestHandler)
            {
                BaseAddress = new Uri("https://geetsangeet.com/")
            };
            client.DefaultRequestHeaders.Add("API-SenderMessageId", Guid.NewGuid().ToString());
            client.DefaultRequestHeaders.Add("API-SenderApplicationId", "ConsoleApp");
            client.DefaultRequestHeaders.Add("API-SenderHostName", "localhost");
            client.DefaultRequestHeaders.Add("API-CreationTimeStamp", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffK"));
            client.DefaultRequestHeaders.Add("API-TransactionId", "Client TransactionId");


            HttpResponseMessage response = client.GetAsync("api/values").Result;
            string responseContent = response.Content.ReadAsStringAsync().Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine(responseContent);
            Console.ReadKey();

        }
        public static void GetApiData()
        {

            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri("https://geetsangeet.com/")

            };

            HttpResponseMessage response = client.GetAsync("api/values").Result;
            string responseContent = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(responseContent);
            Console.ReadKey();

        }


        private static X509Certificate2 GetClientCertificate()
        {
            X509Store userCaStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            try
            {
                userCaStore.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certificatesInStore = userCaStore.Certificates;
                X509Certificate2Collection findResult = certificatesInStore.Find(X509FindType.FindBySubjectName, "geetsangeet.com", true);
                X509Certificate2 clientCertificate = null;
                if (findResult.Count == 1)
                {
                    clientCertificate = findResult[0];
                }
                else
                {
                    throw new Exception("Unable to locate the correct client certificate.");
                }
                return clientCertificate;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                userCaStore.Close();
            }


        }


        public static void GetAllCertificatesFromRoot()
        {
            X509Store userCaStore = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            try
            {
                userCaStore.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certificate2Collection = userCaStore.Certificates;
                foreach (var cert in certificate2Collection)
                {
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine(cert.GetExpirationDateString());
                    Console.WriteLine(cert.Issuer);
                    Console.WriteLine(cert.HasPrivateKey);
                    Console.WriteLine(cert.SubjectName.Name);
                    Console.WriteLine("-----------------------------");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                userCaStore.Close();
            }
        }

        public static void GetAllCertificatesFromCurrentUserStore()
        {
            X509Store userCaStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            try
            {
                userCaStore.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certificate2Collection = userCaStore.Certificates;
                foreach (var cert in certificate2Collection)
                {
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine(cert.GetExpirationDateString());
                    Console.WriteLine(cert.Issuer);
                    Console.WriteLine(cert.HasPrivateKey);
                    Console.WriteLine(cert.SubjectName.Name);
                    Console.WriteLine("-----------------------------");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                userCaStore.Close();
            }
        }

    }
}
