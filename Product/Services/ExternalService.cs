﻿using Microsoft.Extensions.Logging;
using Product.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;


namespace Product.Services
{
    public class ExternalService : ICustomerService
    {
        private HttpClient _client;
        public ExternalService(HttpClient client)
        {
            _client = client;
        }
        public async Task<List<CustomerDto>> GetCustomers()
        {
            HttpResponseMessage response = await _client.GetAsync("api/Account/GetCustomers");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<CustomerDto>>();
        }

        public async Task<CustomerDto> GetCustomerByID(Guid id)
        {
            HttpResponseMessage response = await _client.GetAsync("api/Account/GetCustomers?id=" + id);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<CustomerDto>();
        }



        public async Task<CustomerDto> SetPurchaseProductAbility(Guid id, bool purchase)
        {
            HttpResponseMessage response = await _client.GetAsync("api/Account/PurchaseProductAbility?id=" + id + "&purchaseAbility=" + purchase);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<CustomerDto>();
        }
    }
}
    