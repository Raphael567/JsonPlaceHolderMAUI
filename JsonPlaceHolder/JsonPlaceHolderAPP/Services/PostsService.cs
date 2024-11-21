using JsonPlaceHolderAPP.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
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

                    var postsList = JsonSerializer.Deserialize<List<Post>>(json);

                    if (postsList != null)
                    {
                        return new ObservableCollection<Post>(postsList);
                    }

                    throw new Exception($"Erro ao carregar posts: {response.StatusCode}");
                }

                throw new Exception($"Erro ao carregar posts. Status Code: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao fazer a requisição GET", ex);
            }
        }

        public async Task<Post> GetPostById(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"posts/{id}");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();

                    var post = JsonSerializer.Deserialize<Post>(json);

                    if (post != null)
                    {
                        return post;
                    }

                    throw new Exception($"O post retornado está vazio");
                }

                throw new Exception($"Erro ao carregar post: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao fazer a requisição GET por ID", ex);
            }
        }

        public async Task<Post> CreatePost(Post newPost)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("posts", newPost);

                if (response.IsSuccessStatusCode)
                {
                    var createdPost = await response.Content.ReadFromJsonAsync<Post>();
                    return createdPost;
                }

                throw new Exception($"Erro ao criar post: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao fazer a requisição POST", ex);
            }
        }

        public async Task<bool> UpdatePost(Post post)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"posts/{post.id}", post);

                Debug.WriteLine($"Status Code: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Erro: {responseBody}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao atualizar post: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeletePost(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"posts/{id}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao deletar post: {ex.Message}");
                return false;
            }
        }
    }
}
