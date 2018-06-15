using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WorldCapBetService.Models;

namespace WorldCapBetService.BLL
{
    public class ApiFootballDataClient
    {

        //In this case we are encapsulating the HttpClient,
        //but it could be exposed if that is desired.
        private readonly HttpClient _client;


        public ApiFootballDataClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<ApiFootballData> GetFixtures()
        {
            try
            {
                //Here we are making the assumption that our HttpClient instance
                //has already had its base address set.
                var response = await _client.GetAsync("competitions/467/fixtures");
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                return ApiFootballData.FromJson(jsonString);
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }

    }
}
