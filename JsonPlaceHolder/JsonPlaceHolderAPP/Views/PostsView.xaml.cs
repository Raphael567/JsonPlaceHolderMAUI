using JsonPlaceHolderAPP.ViewModels;

namespace JsonPlaceHolderAPP.Views;

public partial class PostsView : ContentPage
{
	public PostsView()
	{
		InitializeComponent();
		BindingContext = new PostsViewModel();
    }
}