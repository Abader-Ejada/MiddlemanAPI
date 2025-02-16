﻿using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace MiddlemanAPI.Controllers
{
    public class StatusController : Controller
    {
        private readonly HttpClient _httpClient;

        public StatusController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("update")]
        public async Task<IActionResult> UpdateStatus([FromQuery] string accountId, [FromQuery] string status)
        {
            if (string.IsNullOrEmpty(accountId) || string.IsNullOrEmpty(status))
            {
                return BadRequest("Missing accountId or status.");
            }

            // Convert status string to corresponding integer
            int statusValue;
            switch (status.ToLower())
            {
                case "accept":
                    statusValue = 1;
                    break;
                case "reject":
                    statusValue = 2;
                    break;
                case "pending":
                    statusValue = 0;
                    break;
                default:
                    return BadRequest("Invalid status value.");
            }

            // Prepare the request body for Power Automate
            var requestBody = new
            {
                accountId = accountId,
                status = statusValue
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            // Power Automate endpoint
            var powerAutomateUrl = "https://prod-43.northeurope.logic.azure.com/workflows/ecffceb1cdec4335aa30129d5b408535/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=i2TtKey9qigiHFXMjDou7cPTrwP5hqm1CLZW0F3Vim8";

            try
            {
                // Send a POST request to Power Automate
                var response = await _httpClient.PostAsync(powerAutomateUrl, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return Ok("Status updated successfully.");
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "Failed to update status.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


    }
}