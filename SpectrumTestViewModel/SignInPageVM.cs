using XamarinAccountSetupDemoModel;
using System.Threading.Tasks;

namespace XamarinAccountSetupDemoViewModel
{
    public class SignInPageVM : NotifyBase
    {

        private string _userName;
        private string _password;


        public async Task SignIn()
        {
            var accounts = await AccountsDatabase.Instance;
            Account account = accounts.GetItemAsync(UserName).Result;
            if (account == null)
            {
                ErrorMessage = "User does not exist";
            }
            else if (account.Password != Password)
            {
                ErrorMessage = "Password is Incorrect";
            }
            else
            {
                ErrorMessage = string.Empty;
            }
        }
        public string ErrorMessage { get; set; }

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                SetSignInState();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                SetSignInState();
            }
        }
        public bool SignInEnabled { get; set; }

        private void SetSignInState()
        {
            if (string.IsNullOrEmpty(_userName) || string.IsNullOrEmpty(_password))
            {
                SignInEnabled = false;
            }
            else
            {
                SignInEnabled = true;
            }
            OnPropertyChanged(nameof(SignInEnabled));
        }
    }
}
