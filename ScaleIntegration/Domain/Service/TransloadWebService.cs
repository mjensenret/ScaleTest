using Domain.Models;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;

namespace Domain.Service
{
    class TransloadWebService
    {
        string baseUrl = "http://transload-test2.savageservices.com:9595/api/scale/";
        string user = "api";
        string pass = "api";
        IsoDateTimeConverter dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/YYYY HH:mm:ss" };

        public TransloadWebService() { }

        public RestClient getUrl(int transferOrderId)
        {
            var client = new RestClient(baseUrl + transferOrderId)
            {
                Authenticator = new HttpBasicAuthenticator(user, pass)
            };
            return client;
        }

        public bool checkAuditNumber(int transferOrderId)
        {
            var client = getUrl(transferOrderId);
            var request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;
            //request.AddHeader("Cache-Control", "no-cache");
            //request.AddHeader("Content-Type", "application/json");

            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;

            return false;
        }

        public TransferOrderModel getTransferOrder(int Id)
        {
            var client = getUrl(Id);
            var request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;

            var response = client.Execute(request);

            

            var model = JsonConvert.DeserializeObject<TransferOrderModel>(response.Content, dateTimeConverter);

            //model.Id = response.Data


            return model;
        }


        public bool UpdateInboundScaleData(int transferOrderId, int sequenceNumber, int driverId, int truckNumber, int trailerNumber, decimal weight, DateTime scaleInDate)
        {
            TransferOrderModel transferOrder = getTransferOrder(transferOrderId);

            return false;
        }
    }
}
