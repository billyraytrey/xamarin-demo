using XamarinAccountSetupDemoViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinAccountSetupDemo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewAccountPage : ContentPage
    {
        private NewAccountPageVM _viewModel;

        public NewAccountPage()
        {
            InitializeComponent();
            BindingContext = new NewAccountPageVM();
            _viewModel = new NewAccountPageVM();
            BindingContext = _viewModel;

            firstName.Completed += FirstNameCompleted;
            lastName.Completed += LastNameCompleted;
            username.Completed += UsernameCompleted;
            password.Completed += Password_Completed;
            number.Completed += Number_Completed;
            date.Completed += DateCompleted;
        }

        private void FirstNameCompleted(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lastName.Text))
            {
                lastName.Focus();
            }
        }
        private void LastNameCompleted(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(username.Text))
            {
                username.Focus();
            }
        }
        private void UsernameCompleted(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(password.Text))
            {
                password.Focus();
            }
        }
        private void Password_Completed(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(number.Text))
            {
                number.Focus();
            }
        }
        private void Number_Completed(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(date.Text))
            {
                date.Focus();
            }
        }

        private void DateCompleted(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lastName.Text) ||
                string.IsNullOrEmpty(username.Text) ||
                string.IsNullOrEmpty(password.Text) ||
                string.IsNullOrEmpty(number.Text) ||
                string.IsNullOrEmpty(date.Text))
                return;
            CreateAccount();
        }
        private void CreateAccountClicked(object sender, EventArgs e)
        {
            CreateAccount();
        }
        private async void CreateAccount()
        {
            await _viewModel.CreateAccount();
            if (_viewModel.ErrorMessage.Contains("Date"))
            {
                await DisplayAlert(_viewModel.ErrorMessage, "Please try again in the future.", "OK");
            }
            else if (_viewModel.ErrorMessage.Contains("Password"))
            {
                await DisplayAlert(_viewModel.ErrorMessage, "Please choose a new password.", "OK");
            }
            else if (_viewModel.ErrorMessage.Contains("Username"))
            {
                await DisplayAlert(_viewModel.ErrorMessage, "Please enter a diffrent username.", "OK");
            }
            else
            {
                await Navigation.PushAsync(new SuccessPage());
            }
        }      
    }
}