using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logging;
using Domain.Service;
using Domain.Models;

namespace Domain.Repositories
{
    public class TransferOrderRepository
    {
        TestTransferContext db = new TestTransferContext();
        TransloadWebService svc = new TransloadWebService();
        

        public TransferOrderRepository()
        {
            ServiceLog.Init(null, null, null, @"C:\Logs\ScaleIntegration\RepositoryLog.log", @"C:\Logs\ScaleIntegration\RepositoryErrorLog.log", true, 10);
        }

        public bool TransferOrderValid(int transferOrderId)
        {
            try
            {
                var model = svc.getTransferOrder(transferOrderId);

                if (svc.checkAuditNumber(transferOrderId))
                {
                    ServiceLog.Default.Trace("Transfer order Id: {0} was found and in scheduled status.", transferOrderId.ToString());
                    return true;
                }
                //var query = from t in db.TransferOrders
                //            where t.transferOrderId == transferOrderId
                //            where t.status == "SCHEDULED"
                //            select t;
                //if (query.Count() >= 1)
                //{
                //    ServiceLog.Default.Trace("Transfer order Id: {0} was found and in scheduled status.", transferOrderId.ToString());
                //    return true;
                //}
                
            }
            catch (Exception e)
            {
                ServiceLog.Default.Log("Error parsing the transfer order Id: {0}", transferOrderId.ToString());
                ServiceLog.Default.LogError(e);
            }


            ServiceLog.Default.Trace("Transfer order Id: {0} was either not in scheduled status, or not found.", transferOrderId.ToString());

            return false;

        }

        public bool UpdateInboundScaleData(int transferOrderId, int sequenceNumber, int driverId, int truckNumber, int trailerNumber, decimal weight, DateTime scaleInDate)
        {
            ServiceLog.Default.Trace(@"Attempting to update Inbound data:
                                        Transfer Order: {0}
                                        Sequence Number: {1}
                                        Driver Id: {2}
                                        Truck: {3}
                                        Trailer: {4}
                                        Weight: {5}
                                        Scale Date: {6}",
                                        transferOrderId.ToString(),
                                        sequenceNumber.ToString(),
                                        driverId.ToString(),
                                        truckNumber.ToString(),
                                        trailerNumber.ToString(),
                                        weight.ToString(),
                                        scaleInDate.ToString()
                                        );
            //var transferOrder = getTransferOrder(transferOrderId);

            TransferOrderModel transferOrder = svc.getTransferOrder(transferOrderId);

            bool splitLoad = transferOrder.isSplit;

            if (transferOrder != null)
            {
                if (transferOrder.isSplit)
                {
                    //update the load start only if sequence = 1
                }
                else
                {
                    //update TO and TOA
                    svc.UpdateInboundScaleData(transferOrderId, sequenceNumber, driverId, truckNumber, trailerNumber, weight, scaleInDate);
                }

                //    if (transferOrderArrival.scaleInWeight == null)
                //    {

                //        TransferOrder updTO = transferOrder;

                //        updTO.departureEquipmentTypeId = 2;
                //          var transferOrderArrival = getTransferOrderArrivals(transferOrderId, sequenceNumber);
                //  updTO.departureEquipmentName = truckNumber.ToString();
                //        if (splitLoad && sequenceNumber == 1)
                //        {
                //            updTO.loadStartDate = scaleInDate;
                //        }
                //        else if (!splitLoad)
                //        {
                //            updTO.loadStartDate = scaleInDate;
                //        }

                //        updTO.modifiedBy = "ScaleInProcess";
                //        updTO.modifiedDate = DateTime.Now;

                //        TransferOrderArrival updTOA = transferOrderArrival;

                //        updTOA.scaleInWeight = weight;
                //        updTOA.scaleInDate = scaleInDate;
                //        updTOA.modifiedBy = "ScaleInProcess";
                //        updTOA.modifiedDate = DateTime.Now;

                //        try
                //        {
                //            db.SaveChanges();
                //            ServiceLog.Default.Trace("Transfer order Id {0} and transfer order arrival id {1} have been successfully updated.", updTO.transferOrderId.ToString(), updTOA.transferOrderArrivalId.ToString());
                //            return true;
                //        }
                //        catch (Exception e)
                //        {
                //            Console.WriteLine("We have a failure...");
                //            Console.WriteLine(e.Message);
                //            ServiceLog.Default.Log("Error occurred while attempting to update transferOrderArrivalId {0}", updTOA.transferOrderArrivalId);
                //            ServiceLog.Default.LogError(e);
                //            return false;
                //        }
                //    }
                //    else
                //    {
                //        ServiceLog.Default.Trace("Transfer order id {0} with transfer order arrival Id {1} has already been updated.", transferOrder.transferOrderId.ToString(), transferOrderArrival.transferOrderArrivalId.ToString());
                //    }

            }
            else
            {
                ServiceLog.Default.Trace("Transfer order object was null for id {0}", transferOrderId.ToString());
            }

            return false;
        }

        public bool UpdateOutboundScaleData(int transferOrderId, int sequenceNumber, int loaderId, decimal weight, DateTime scaleOutDate)
        {
            ServiceLog.Default.Trace(@"Attempting to update outbound data:
                                        Transfer Order: {0}
                                        Sequence Number: {1}
                                        Loader Id: {2}
                                        Weight: {3}
                                        Scale Out Date: {4}",
                            transferOrderId.ToString(),
                            sequenceNumber.ToString(),
                            loaderId.ToString(),
                            weight.ToString(),
                            scaleOutDate.ToString()
                            );

            var transferOrder = getTransferOrder(transferOrderId);

            bool splitLoad = isSplit(transferOrderId);

            if (transferOrder != null)
            {
                var transferOrderArrival = getTransferOrderArrivals(transferOrderId, sequenceNumber);

                if (transferOrderArrival.scaleOutWeight == null)
                {
                    TransferOrder updTO = transferOrder;

                    if (splitLoad && sequenceNumber > 1)
                    {
                        updTO.loadEndDate = scaleOutDate;
                    }
                    else if (!splitLoad)
                    {
                        updTO.loadEndDate = scaleOutDate;
                    }

                    updTO.transferOrderLoaderId = loaderId;
                    updTO.modifiedBy = "ScaleOutProcess";
                    updTO.modifiedDate = DateTime.Now;

                    TransferOrderArrival updTOA = transferOrderArrival;
                    decimal netWt = Convert.ToDecimal(updTOA.scaleInWeight) - weight;

                
                    updTOA.scaleOutWeight = weight;
                    updTOA.netTransferWeight = Math.Abs(netWt);
                    updTOA.scaleOutDate = scaleOutDate;
                    updTOA.modifiedBy = "ScaleOutProcess";
                    updTOA.modifiedDate = DateTime.Now;

                    try
                    {
                        db.SaveChanges();
                        ServiceLog.Default.Trace("Outbound scale data for transfer order id {0} and transfer order arrival id {1} has been successfully updated.", transferOrder.transferOrderId.ToString(), transferOrderArrival.transferOrderArrivalId.ToString());
                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Unable to update scale out information");
                        Console.WriteLine(e.Message);
                        ServiceLog.Default.Trace("Transfer order id {0}, transfer order arrival id {1} did not save correctly", transferOrder.transferOrderId.ToString(), transferOrderArrival.transferOrderArrivalId.ToString());
                        return false;
                    }
                }
                else
                {
                    ServiceLog.Default.Trace("Transfer order id {0}, transfer order arrival id {1} has already been updated.", transferOrder.transferOrderId.ToString(), transferOrderArrival.transferOrderArrivalId.ToString());
                }
            }
            else
            {
                ServiceLog.Default.Trace("transferOrder was null");
            }

            return false;
        }

        private TransferOrder getTransferOrder(int id)
        {
            var transferOrder = (from t in db.TransferOrders
                                 where t.transferOrderId == id
                                 where t.status == "SCHEDULED"
                                 where !t.isCompleted
                                 where !t.isVoided
                                 select t);

            return transferOrder.Single();
        }

        private TransferOrderArrival getTransferOrderArrivals(int transferOrderId, int sequenceNumber)
        {
            var transferOrderArrivals = (from toa in db.TransferOrderArrivals
                                         where toa.transferOrderId == transferOrderId
                                         select toa);
            
            if (transferOrderArrivals.Count() > 1)
                transferOrderArrivals = transferOrderArrivals.Where(x => x.sequenceNumber == sequenceNumber);


            return transferOrderArrivals.FirstOrDefault();

        }

        private bool isSplit(int transferOrderId)
        {
            var transferOrderArrivals = (from toa in db.TransferOrderArrivals
                                         where toa.transferOrderId == transferOrderId
                                         select toa);

            if (transferOrderArrivals.Count() > 1)
                return true;

            return false;
        }

    }
}
