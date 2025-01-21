using FacilityLeasing.API.Models;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace FacilityLeasing.API.Tests
{
    public class Tests : IClassFixture<DBFixture>
    {
        private readonly DBFixture _dbFixture;

        public Tests(DBFixture dbFixture)
        {
            _dbFixture = dbFixture;
        }

        [Fact]
        public async Task CreateContract_ReturnContractsList_ContractExists()
        {
            // create contract
            var request = new HttpRequestMessage(HttpMethod.Post, "/contracts")
            {
                Content = JsonContent.Create(new PlacementContractDTO
                {
                    FacilityCode = "FAC001",
                    EquipmentCode = "EQP001",
                    EquipmentQuantity = 18
                })
            };
            request.Headers.Add("X-AUTH", Environment.GetEnvironmentVariable("X_AUTH_HEADER"));
            var response = await _dbFixture.Client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // get contracts list
            request = new HttpRequestMessage(HttpMethod.Get, "/contracts");
            request.Headers.Add("X-AUTH", Environment.GetEnvironmentVariable("X_AUTH_HEADER"));
            response = await _dbFixture.Client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // ensure the created contract exists
            var contracts = await response.Content.ReadFromJsonAsync<List<PlacementContractDTO>>();
            contracts!.Should().Contain(c => c.FacilityCode == "FAC001" &&
                                             c.EquipmentCode == "EQP001" &&
                                             c.EquipmentQuantity == 18);
        }

        [Fact]
        public async Task CreateContractWithExceedingArea_ShouldFail()
        {
            // create contract
            var request = new HttpRequestMessage(HttpMethod.Post, "/contracts")
            {
                Content = JsonContent.Create(new PlacementContractDTO
                {
                    FacilityCode = "FAC001",
                    EquipmentCode = "EQP001",
                    EquipmentQuantity = 1000 // the quantity exceeds the allowed area
                })
            };
            request.Headers.Add("X-AUTH", Environment.GetEnvironmentVariable("X_AUTH_HEADER"));
            var response = await _dbFixture.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); // should return bad request

            // error message check
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("Contract is not feasible", responseContent);
        }

        [Fact]
        public async Task NonAuthorizedRequest_ShouldFail()
        {
            // get contracts list without auth header
            var request = new HttpRequestMessage(HttpMethod.Get, "/contracts");
            var response = await _dbFixture.Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode); // should be unauthorized

            // try get contracts with randomly generated auth header
            request.Headers.Add("X-AUTH", Guid.NewGuid().ToString());
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode); // should stay unauthorized
        }
    }
}

