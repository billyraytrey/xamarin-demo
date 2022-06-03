using XamarinAccountSetupDemoViewModel;
using System;
using Xamarin.Forms;

namespace XamarinAccountSetupDemo
{
    public partial class SignInPage : ContentPage
    {
        private SignInPageVM _viewModel;
        public SignInPage()
        {
            InitializeComponent();
            _viewModel = new SignInPageVM();
            BindingContext = _viewModel;
        }

        private async void SingInButtonClicked(object sender, EventArgs e)
        {
            await _viewModel.SignIn();
            if (_viewModel.ErrorMessage.Contains("User"))
            {
                await DisplayAlert(_viewModel.ErrorMessage, "Please enter the correct username.", "OK");
            }
            else if (_viewModel.ErrorMessage.Contains("Password"))
            {
                await DisplayAlert(_viewModel.ErrorMessage, "Please enter the correct password.", "OK");
            }
            else
            {
                await DisplayAlert("Congratulations!", "The username and password was found.", "Cool");
            }
        }

        private async void NewUserButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewAccountPage());
        }
    }
}
