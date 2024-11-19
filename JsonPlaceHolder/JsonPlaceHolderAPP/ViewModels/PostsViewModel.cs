using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JsonPlaceHolderAPP.Models;
using JsonPlaceHolderAPP.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        }

        [ObservableProperty]
        private ObservableCollection<Post> _posts;

        public ICommand GetAllPostsCommand { get; }

        private async Task LoadPosts()
        {
            Posts = await _postsService.GetAllPosts();
        }
    }
}
