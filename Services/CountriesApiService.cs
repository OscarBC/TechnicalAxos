using Newtonsoft.Json;
using TechnicalAxos_OscarBarrera.Models;

namespace TechnicalAxos_OscarBarrera.Services
{

    public class CountriesApiService
    {
        private readonly HttpClient _httpClient;

        public CountriesApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://restcountries.com/v3.1/")
            };
        }

        public async Task<List<CountryInfo>> GetAllCountriesAsync()
        {
            try{
                var response = await _httpClient.GetAsync("all?fields=name,capital,region,subregion,population,languages,flags");
                Console.WriteLine("response:"+response.StatusCode);
                if(response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var countries = JsonConvert.DeserializeObject<List<CountryInfo>>(result);
                    return countries ?? new List<CountryInfo>();
                }else{
                    return new List<CountryInfo>();
                }
            }catch(Exception e){
                Console.WriteLine(e.Message);
                return new List<CountryInfo>();
            }
        }
    }
    
}