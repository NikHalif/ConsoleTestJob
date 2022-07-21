using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ConsoleTestJob
{
    public class SearchReportWoodDeal<T>
    {
        public T searchReportWoodDeal { get; set; }
    }

    public class DatDeals
    {
        public SearchReportWoodDeal<DealsContent> data { get; set; }
    }

    public class DealsContent
    {
        public List<Deal> content { get; set; }
    }

    public class Deal
    {
        public string buyerInn { get; set; }
        public string buyerName { get; set; }
        public DateTime? dealDate { get; set; }
        public string dealNumber { get; set; }
        public string sellerInn { get; set; }
        public string sellerName { get; set; }
        public double woodVolumeBuyer { get; set; }
        public double woodVolumeSeller { get; set; }

        public Deal() { }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this).ToString();
        }
    }



    public class DatDealsCount
    {
        public SearchReportWoodDeal<DealsCount> data { get; set; }
    }

    public class DealsCount
    {
        public int number { get; set; }
        public double overallBuyerVolume { get; set; }
        public double overallSellerVolume { get; set; }
        public int size { get; set; }
        public int total { get; set; }

        public DealsCount() { }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this).ToString();
        }

    }
}
