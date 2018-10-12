using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public class TransferOrderRepository
    {
        TestTransferContext db = new TestTransferContext();

        public TransferOrderRepository()
        {

        }

        public bool TransferOrderValid(int transferOrderId)
        {
            var query = from t in db.TransferOrders
                        where t.transferOrderId == transferOrderId
                        where t.status == "SCHEDULED"
                        select t;
            if (query.Count() >= 1)
                return true;

            return false;

        }

        public bool UpdateInboundScaleData(int transferOrderId, int sequenceNumber, int driverId, int truckNumber, int trailerNumber, decimal weight, DateTime scaleInDate)
        {
            var transferOrder = getTransferOrder(transferOrderId);

            if (transferOrder != null)
            {
                var transferOrderArrival = getTransferOrderArrivals(transferOrderId, sequenceNumber);

                //if (transferOrderArrival.Count() > 1)
                //    transferOrderArrival.Where(x => x.sequenceNumber == sequenceNumber);

                TransferOrder updTO = transferOrder;

                updTO.departureEquipmentTypeId = 2;
                updTO.departureEquipmentName = truckNumber.ToString();
                updTO.loadStartDate = scaleInDate;
                updTO.modifiedBy = "ScaleInProcess";
                updTO.modifiedDate = DateTime.Now;

                TransferOrderArrival updTOA = transferOrderArrival;

                updTOA.scaleInWeight = weight;
                updTOA.scaleInDate = scaleInDate;
                updTOA.modifiedBy = "ScaleInProcess";
                updTOA.modifiedDate = DateTime.Now;

                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("We have a failure...");
                    Console.WriteLine(e.Message);
                    return false;
                }


            }

            return false;
        }

        public bool UpdateOutboundScaleData(int transferOrderId, int sequenceNumber, int loaderId, decimal weight, DateTime scaleOutDate)
        {
            var transferOrder = getTransferOrder(transferOrderId);

            if (transferOrder != null)
            {
                var transferOrderArrival = getTransferOrderArrivals(transferOrderId, sequenceNumber);

                //if (transferOrderArrival.Count() > 1)
                //    transferOrderArrival.Where(x => x.sequenceNumber == sequenceNumber);

                TransferOrder updTO = transferOrder;

                updTO.loadEndDate = scaleOutDate;
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
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unable to update scale out information");
                    Console.WriteLine(e.Message);
                    return false;
                }
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

    }
}
