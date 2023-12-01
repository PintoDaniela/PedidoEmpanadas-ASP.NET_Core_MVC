﻿using PedidoEmpanadasApp.Interfaces;
using Shared.DTOs;
using System.Text;
using System.Text.Json;

namespace PedidoEmpanadasApp.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public PedidoRepository(HttpClient httpClient, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSetting:baseUrl"];
        }


        public async Task<string> RealizarPedido(string userName, PedidoDto pedido, string token)
        {
            var pedidoUrl = $"{_baseUrl}/api/Pedido";

            // Agregar el token al header de la solicitud
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Puedes serializar el objeto PedidoDto directamente
            var jsonPedido = JsonSerializer.Serialize(pedido);
            var dataPedido = new StringContent(jsonPedido, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(pedidoUrl, dataPedido);

            if (response.IsSuccessStatusCode)
            {                
                return "Ok";
            }
            if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var respuestaPedido = await response.Content.ReadAsStringAsync();
                if(respuestaPedido.Contains("Ya hiciste tu pedido"))
                {
                    return "400";
                }                
            }            
            return null;
        }


        public Task<PedidoDto> MostrarPedido(PedidoDto pedido)
        {
            throw new NotImplementedException();
        }
    }
}
