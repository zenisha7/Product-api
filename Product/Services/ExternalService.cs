using Microsoft.Extensions.Logging;
using Product.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace Product.Services
{
    //public class ExternalService : ICustomerService
    //{
    //    private HttpClient client;
    //    public ExternalService(ILogger<ExternalService> logger,HttpClient _client)
    //    {
    //        _client = client;
    //    }
    //    public async Task<List<Customer>> GetCustomers()
    //    {
          
    //            HttpResponseMessage response = await client.GetAsync("api/account/GetCustomers?Id");
    //            List<Customer> responseBody;
    //        if (response.IsSuccessStatusCode)
    //        {
    //            responseBody = await response.Content.ReadAsAsync<Customer>();
    //            return await Task.FromResult(responseBody);
    //        }

    //        else
    //        {
    //            return null;
    //        }
           
           
    //    }

    //    public async Task<Customer> GetCustomerByID(Guid id)
    //    {
    //       try
    //        {
    //            HttpResponseMessage response = await client.GetAsync("api/account/GetCustomers?Id");
    //            response.EnsureSuccessStatusCode();
    //            string responseBody = await response.Content.ReadAsStringAsync();
    //            Console.WriteLine(responseBody);
    //        }
    //        catch
    //        {
    //            Console.WriteLine("\nException Caught!");
    //        }
           
    //    }

  

    //    public Task<Customer> SetPurchaseProductAbility(Guid id, bool purchase)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
