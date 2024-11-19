using JsonPlaceHolderAPP.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;

namespace JsonPlaceHolderAPP.Services
{
    public class PostService
    {
        private readonly HttpClient _httpClient;

        public PostService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
        }

        public async Task<ObservableCollection<Post>> GetAllPosts()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("posts");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("Posts carregados: " + json);

                    var postsList = JsonSerializer.Deserialize<ObservableCollection<Post>>(json);
                    if (postsList != null) 
                    { 
                        return new ObservableCollection<Post>(postsList);
                    }
                }
            } 
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                Debug.WriteLine(ex.ToString());
            }

            return null;
        }
    }
}
