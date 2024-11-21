using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JsonPlaceHolderAPP.Models;
using JsonPlaceHolderAPP.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace JsonPlaceHolderAPP.ViewModels
{
    public partial class PostsViewModel : ObservableObject
    {
        private readonly PostService _postsService;

        public PostsViewModel()
        {
            _postsService = new PostService();
            GetAllPostsCommand = new AsyncRelayCommand(LoadPosts);
            UpdatePostAsyncCommand = new AsyncRelayCommand(UpdatePostAsync);
            DeletePostAsyncCommand = new AsyncRelayCommand<int>(DeletePostAsync);
        }

        [ObservableProperty]
        private ObservableCollection<Post> _posts;

        [ObservableProperty]
        private int postId;

        [ObservableProperty]
        private int deletePostId;

        [ObservableProperty]
        private int updatePostId;

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string body;

        [ObservableProperty]
        private string userId;

        [ObservableProperty]
        private string createPostTitle;

        [ObservableProperty]
        private string createPostBody;

        public ICommand GetAllPostsCommand { get; }
        public ICommand UpdatePostAsyncCommand { get; }
        public ICommand DeletePostAsyncCommand { get; }

        private async Task LoadPosts()
        {
            Posts = await _postsService.GetAllPosts();
        }

        [RelayCommand]
        public async Task LoadPostById(int id)
        {
            try
            {
                var post = await _postsService.GetPostById(id);

                if (post != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Post", $"Post: {post.title}\n\n{post.body}", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Post", "Post não encontrado", "OK");
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Erro ao carregar post", ex);
            }
        }

        [RelayCommand]
        public async Task CreatePost()
        {
            var newPost = new Post
            {
                userId = Convert.ToInt32(userId),
                title = createPostTitle,
                body = createPostBody
            };

            try
            {
                var createdPost = await _postsService.CreatePost(newPost);

                await Application.Current.MainPage.DisplayAlert("Post criado", $"Post criado com sucesso! Id: {createdPost.id}", "OK");

                userId = string.Empty;
                title = string.Empty;
                body = string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar post", ex);
            }
        }

        public async Task UpdatePostAsync()
        {
            var postToUpdate = new Post
            {
                id = UpdatePostId,
                title = Title,
                body = Body
            };

            try
            {
                var success = await _postsService.UpdatePost(postToUpdate);

                if (success)
                {
                    var updatedPost = Posts.FirstOrDefault(p => p.id == UpdatePostId);
                    if (updatedPost != null)
                    {
                        updatedPost.title = Title;
                        updatedPost.body = Body;

                        await Application.Current.MainPage.DisplayAlert("Post atualizado com sucesso.",
                            $"Post: {updatedPost.title}\n\n{updatedPost.body}", "OK");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Erro", "Post não encontrado após atualização.", "OK");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erro", "Não foi possível atualizar o post.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Erro ao atualizar post: {ex.Message}", "OK");
            }
        }


        private async Task DeletePostAsync(int id)
        {
            bool success = await _postsService.DeletePost(id);

            if (success)
            {
                var postToRemove = Posts.FirstOrDefault(p => p.id == id);
                if (postToRemove != null)
                {
                    Posts.Remove(postToRemove);
                }

                await Application.Current.MainPage.DisplayAlert("Sucesso", "Post deletado com sucesso.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Não foi possível deletar o post.", "OK");
            }
        }
    }
}
