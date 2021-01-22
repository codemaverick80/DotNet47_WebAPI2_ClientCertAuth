using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Net;
using System.Threading;
using Newtonsoft.Json;

namespace WebAPI2.ClientConsole
{   

    class Program
    {
        static void Main(string[] args)
        {

            //GetData();
            PostData();

        }

        #region "HttpWebRequest"
        public static void GetData()
        {
            string apiUrl = "https://geetsangeet.com/api/product";
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Accept = "application/json";
                httpWebRequest.Headers.Add("API-SenderMessageId", Guid.NewGuid().ToString());
                httpWebRequest.Headers.Add("API-SenderApplicationId", "ConsoleApp");
                httpWebRequest.Headers.Add("API-SenderHostName", "localhost");
                httpWebRequest.Headers.Add("API-CreationTimeStamp", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffK"));
                httpWebRequest.Headers.Add("API-TransactionId", "Client TransactionId");
                httpWebRequest.Method = "GET";
                httpWebRequest.Timeout = Timeout.Infinite;

                X509Certificate2 clientCert = GetClientCertificateFromStore();
                if (clientCert != null)
                {
                    httpWebRequest.ClientCertificates.Add(clientCert);
                }
                else
                {
                    Console.WriteLine("Client certificate is null");
                    return;
                }

                //Assign the response object of HttpWebRequest to a HttpWebResponse variable
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                //Read API response
                var readApiResponse = ReadApiResponse(httpWebResponse);
                httpWebResponse.Close();


                //Deserialize API response
                var resultToReturn = JsonConvert.DeserializeObject<object>(readApiResponse);

                Console.WriteLine(resultToReturn);

            }
            catch(ArgumentException ex)
            {
                throw ex;
            }
            catch(WebException ex)
            {
                Stream dataStream = ex.Response.GetResponseStream();
                var reader = new StreamReader(dataStream);
                string content =reader.ReadToEnd().ToString();
                reader.Close();
                dataStream.Close();
                Console.WriteLine(content);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public static string GetPayloadForPostRequest()
        {
            var postRequest = new ProductPostRequestModel()
            {
                ProductName="Samsung OLED TV",
                 Description="Samsung OLED 65inch TV"
            };
            var payload = JsonConvert.SerializeObject(postRequest);
            return payload;

        }
        public static void PostData()
        {         

            System.Text.Encoding encoding = new System.Text.UTF8Encoding();
            byte[] data = encoding.GetBytes(GetPayloadForPostRequest());

            string apiUrl = "https://geetsangeet.com/api/product";
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Accept = "application/json";
                httpWebRequest.Headers.Add("API-SenderMessageId", Guid.NewGuid().ToString());
                httpWebRequest.Headers.Add("API-SenderApplicationId", "ConsoleApp");
                httpWebRequest.Headers.Add("API-SenderHostName", "localhost");
                httpWebRequest.Headers.Add("API-CreationTimeStamp", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffK"));
                httpWebRequest.Headers.Add("API-TransactionId", "Client TransactionId");
                httpWebRequest.Method = "POST";
                httpWebRequest.Timeout = Timeout.Infinite;

                X509Certificate2 clientCert = GetClientCertificateFromStore();
                if (clientCert != null)
                {
                    httpWebRequest.ClientCertificates.Add(clientCert);
                }
                else
                {
                    Console.WriteLine("Client certificate is null");
                    return;
                }


                Stream dataStream = httpWebRequest.GetRequestStream();
                dataStream.Write(data, 0, data.Length);


                //Assign the response object of HttpWebRequest to a HttpWebResponse variable
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                //Read API response
                var readApiResponse = ReadApiResponse(httpWebResponse);
                httpWebResponse.Close();


                //Deserialize API response
                var resultToReturn = JsonConvert.DeserializeObject<object>(readApiResponse);

                Console.WriteLine(resultToReturn);

            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            catch (WebException ex)
            {
                Stream dataStream = ex.Response.GetResponseStream();
                var reader = new StreamReader(dataStream);
                string content = reader.ReadToEnd().ToString();
                reader.Close();
                dataStream.Close();
                Console.WriteLine(content);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private static string ReadApiResponse(HttpWebResponse httpWebResponse)
        {
            
            if(httpWebResponse.StatusCode==HttpStatusCode.OK)
            {
                Stream dataStream = httpWebResponse.GetResponseStream();
                var reader = new StreamReader(dataStream);
                string dataResponse = reader.ReadToEnd().ToString();
                reader.Close();
                dataStream.Close();
                return dataResponse;

            }
            else
            {
                return "Something went wrong...";
            }

        }

        #endregion
                    

        #region "Certificate from File"
        private static X509Certificate2 GetCertificateFromFile()
        {
            string pfxFilePath = @"C:\Client.pfx";
            string pfxPassword = "XXXXX";

            X509Certificate2 x509 = new X509Certificate2();
            byte[] rawData = ReadFile(pfxFilePath);
            x509.Import(rawData,pfxPassword, X509KeyStorageFlags.DefaultKeySet);
            //Console.WriteLine("{0}Subject: {1}{0}", Environment.NewLine, x509.Subject);
            //Console.WriteLine("{0}Issuer: {1}{0}", Environment.NewLine, x509.Issuer);
            //Console.WriteLine("{0}Version: {1}{0}", Environment.NewLine, x509.Version);
            //Console.WriteLine("{0}Valid Date: {1}{0}", Environment.NewLine, x509.NotBefore);
            //Console.WriteLine("{0}Expiry Date: {1}{0}", Environment.NewLine, x509.NotAfter);
            //Console.WriteLine("{0}Thumbprint: {1}{0}", Environment.NewLine, x509.Thumbprint);
            //Console.WriteLine("{0}Serial Number: {1}{0}", Environment.NewLine, x509.SerialNumber);
            //Console.WriteLine("{0}Friendly Name: {1}{0}", Environment.NewLine, x509.PublicKey.Oid.FriendlyName);
            //Console.WriteLine("{0}Public Key Format: {1}{0}", Environment.NewLine, x509.PublicKey.EncodedKeyValue.Format(true));
            //Console.WriteLine("{0}Raw Data Length: {1}{0}", Environment.NewLine, x509.RawData.Length);
            //Console.WriteLine("{0}Certificate to string: {1}{0}", Environment.NewLine, x509.ToString(true));
            //Console.WriteLine("{0}Certificate to XML String: {1}{0}", Environment.NewLine, x509.PublicKey.Key.ToXmlString(false));

            return x509;
        }
        internal static byte[] ReadFile(string fileName)
        {
            FileStream f = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            int size = (int)f.Length;
            byte[] data = new byte[size];
            size = f.Read(data, 0, size);
            f.Close();
            return data;
        }
        #endregion


        #region "Certificate from store"

        private static X509Certificate2 GetClientCertificateFromStore()
        {
            try
            {
               // X509Store userCaStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                X509Store userCaStore = new X509Store(StoreLocation.LocalMachine);
                userCaStore.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certificatesInStore = userCaStore.Certificates;
                X509Certificate2 clientCertificate = null;
                foreach (var cert in certificatesInStore)
                {
                    if (cert.Subject.Contains("OU=TESTAPP"))
                    {
                        clientCertificate = cert;
                        break;
                    }
                }
                return clientCertificate;
            }
            catch (Exception ex)
            {
                throw ex;
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


        #endregion



        #region "WebRequestHandler & HttpClient"

        public static void GetApiDataAndSendClientCertToApi()
        {
            X509Certificate2 clientCert = GetClientCertificateFromStore();
            //X509Certificate2 clientCert = GetCertificateFromFile();
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

            HttpResponseMessage response = client.GetAsync("api/product").Result;
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
            HttpResponseMessage response = client.GetAsync("api/product").Result;
            string responseContent = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(responseContent);
            Console.ReadKey();

        }

        #endregion





    }

    public class ProductPostRequestModel
    {
        public string ProductName { get; set; }

        public string Description { get; set; }

    }


}
