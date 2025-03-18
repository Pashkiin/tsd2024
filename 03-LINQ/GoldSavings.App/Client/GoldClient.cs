using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GoldSavings.App.Model;

namespace GoldSavings.App.Client;

public class GoldClient
{
    private HttpClient _client;
    public GoldClient()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("http://api.nbp.pl/api/");
        _client.DefaultRequestHeaders.Accept.Clear();

    }
    public async Task<GoldPrice> GetCurrentGoldPrice()
    {
        HttpResponseMessage responseMsg = _client.GetAsync("cenyzlota/").GetAwaiter().GetResult();
        if (responseMsg.IsSuccessStatusCode)
        {
            string content = await responseMsg.Content.ReadAsStringAsync();
            List<GoldPrice> prices = JsonConvert.DeserializeObject<List<GoldPrice>>(content);
            if (prices != null && prices.Count == 1)
            {
                return prices[0];
            }
        }
        return null;
    }

    public async Task<List<GoldPrice>> GetGoldPrices(DateTime startDate, DateTime endDate)
    {
        const int maxDays = 93;
        string dateFormat = "yyyy-MM-dd";
        List<GoldPrice> allPrices = new List<GoldPrice>();

        DateTime currentStartDate = startDate;

        while (currentStartDate <= endDate)
        {
            DateTime currentEndDate = currentStartDate.AddDays(maxDays - 1);
            if (currentEndDate > endDate)
            {
                currentEndDate = endDate;
            }

            string requestUri = $"cenyzlota/{currentStartDate.ToString(dateFormat)}/{currentEndDate.ToString(dateFormat)}";
            HttpResponseMessage responseMsg = _client.GetAsync(requestUri).GetAwaiter().GetResult();
            if (responseMsg.IsSuccessStatusCode)
            {
                string content = await responseMsg.Content.ReadAsStringAsync();
                List<GoldPrice> prices = JsonConvert.DeserializeObject<List<GoldPrice>>(content);
                if (prices != null)
                {
                    allPrices.AddRange(prices);
                }
            }
            currentStartDate = currentEndDate.AddDays(1);
        }

        return allPrices;
    }

}