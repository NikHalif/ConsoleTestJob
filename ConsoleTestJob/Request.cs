using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTestJob
{

    public static class Request
    {

        static readonly Uri ResurceURI = new Uri("https://www.lesegais.ru/open-area/graphql");
        static readonly string UserAgent = "Mozilla/5.0 (Parser B.O.T)";

        public static async Task<List<Deal>> PostDealAsync(int size, int number, string filtr = null, string orders = null)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = ResurceURI;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);


                var deal = new List<Deal>();

                var json = JsonConvert.SerializeObject(new
                {
                    query = "query SearchReportWoodDeal($size: Int!, $number: Int!, $filter: Filter, $orders: [Order!]) {\n  searchReportWoodDeal(filter: $filter, pageable: {number: $number, size: $size}, orders: $orders) {\n    content {\n      sellerName\n      sellerInn\n      buyerName\n      buyerInn\n      woodVolumeBuyer\n      woodVolumeSeller\n      dealDate\n      dealNumber\n      __typename\n    }\n    __typename\n  }\n}\n",
                    variables = new
                    {
                        size = size,
                        number = number,
                        filter = filtr,
                        orders = orders
                    },
                    operationName = "SearchReportWoodDeal"
                });

                try
                {
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(ResurceURI, content);
                    response.EnsureSuccessStatusCode();

                    deal = JsonConvert.DeserializeObject<DatDeals>(response.Content.ReadAsStringAsync().Result).data.searchReportWoodDeal.content;

                }
                catch (Exception e)
                {
                    throw e;
                }

                return deal;

            }
        }

        public static async Task<DealsCount> PostDealsCountAsync(int size, int number, string filtr = null)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = ResurceURI;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);


                var deal = new DealsCount();

                var json = JsonConvert.SerializeObject(new
                {
                    query = "query SearchReportWoodDealCount($size: Int!, $number: Int!, $filter: Filter, $orders: [Order!]) {\n  searchReportWoodDeal(filter: $filter, pageable: {number: $number, size: $size}, orders: $orders) {\n    total\n    number\n    size\n    overallBuyerVolume\n    overallSellerVolume\n    __typename\n  }\n}\n",
                    variables = new
                    {
                        size = size,
                        number = number,
                        filter = filtr,
                    },
                    operationName = "SearchReportWoodDealCount"
                });

                try
                {
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(ResurceURI, content);
                    response.EnsureSuccessStatusCode();

                    deal = JsonConvert.DeserializeObject<DatDealsCount>(
                        response.Content.ReadAsStringAsync().Result
                        )
                        .data
                        .searchReportWoodDeal;
                }
                catch (Exception e)
                {
                    throw e;
                }

                return deal;
            }
        }

    }

}
