using XamarinAccountSetupDemoModel;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XamarinAccountSetupDemoViewModel
{
    public class NewAccountPageVM : NotifyBase
    {
        private string _fName;
        private string _lName;
        private string _uName;
        private string _password;
        private string _number;
        private string _date;
        private string _errorMessage;

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChangedAuto();
            }
        }
        public string FName
        {
            get => _fName;
            set
            {
                _fName = value;
                OnPropertyChanged(nameof(FNameInValid));
                OnPropertyChanged(nameof(CreateAccountEnabled));
            }
        }
        public string LName
        {
            get => _lName;
            set
            {
                _lName = value;
                OnPropertyChanged(nameof(LNameInValid));
                OnPropertyChanged(nameof(CreateAccountEnabled));
            }
        }
        public string UName
        {
            get => _uName;
            set
            {
                _uName = value;
                OnPropertyChanged(nameof(CreateAccountEnabled));
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                PasswordIsValid();
                OnPropertyChanged(nameof(PasswordIncorrectLength));
                OnPropertyChanged(nameof(PasswordContainsNoLowerCase));
                OnPropertyChanged(nameof(PasswordContainsNoUpperCase));
                OnPropertyChanged(nameof(CreateAccountEnabled));
            }
        }
        public string Number
        {
            get => _number;
            set
            {
                _number = value;
                OnPropertyChanged(nameof(NumberInValid));
                OnPropertyChanged(nameof(CreateAccountEnabled));
                OnPropertyChangedAuto();
            }
        }
        public string Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(NumberInValid));
                OnPropertyChanged(nameof(CreateAccountEnabled));
            }
        }
        public bool FNameInValid => NameInvalid(FName);
        public bool LNameInValid => NameInvalid(LName);
        public bool NumberInValid
        {
            get
            {
                if (string.IsNullOrEmpty(Number))
                {
                    return false;
                }
                if (Number.Length < 14 || Number.Length > 14)
                {
                    return true;
                }
                return false;
            }
        }
        public bool DateInValid
        {
            get
            {
                if (string.IsNullOrEmpty(Date))
                {
                    return false;
                }
                if (Date.Length < 10 || Date.Length > 10)
                {
                    return true;
                }
                return false;
            }
        }

        public bool PasswordIncorrectLength
        {
            get
            {
                if (string.IsNullOrEmpty(Password) || Password.Length < 8 || Password.Length > 15)
                {
                    return true;
                }

                return false;
            }
        }
        public bool PasswordContainsNoLowerCase
        {
            get
            {
                if (string.IsNullOrEmpty(Password) || !Regex.IsMatch(Password, "([a-z])"))
                {
                    return true;
                }

                return false;
            }
        }

        public bool PasswordContainsNoUpperCase
        {
            get
            {
                if (string.IsNullOrEmpty(Password) || !Regex.IsMatch(Password, "([A-Z])"))
                {
                    return true;
                }

                return false;
            }
        }

        public bool PasswordIsRepetitive
        {
            get
            {
                var pwArray = Password.ToCharArray();
                for (int i = 0; i < pwArray.Length; i++)
                {
                    string seq = pwArray[i].ToString();
                    for (int j = 1; j < 3; j++)
                    {
                        if ((i + j) < pwArray.Length)
                        {
                            seq += pwArray[i + j].ToString();
                        }
                    }
                    var first = Password.IndexOf(seq);
                    return Regex.Matches(Password, seq).Count > 1;
                }
                return false;

            }
        }
        public async Task CreateAccount()
        {
            var dateDiff = DateTime.Parse(_date) - DateTime.Today;
            if (dateDiff.Days > 30 || dateDiff.Days < 0)
            {
                ErrorMessage = "Date Invalid";
            }
            else if (PasswordIsRepetitive)
            {
                ErrorMessage = "Password Invalid";
            }
            else
            {
                var db = await AccountsDatabase.Instance;
                var account = db.GetItemAsync(UName).Result;
                if (account == null)
                {
                    ErrorMessage = string.Empty;
                    account = new Account();
                    account.FName = _fName;
                    account.LName = _lName;
                    account.UName = _uName;
                    account.Password = _password;
                    account.Number = _number;
                    account.Date = DateTime.Parse(Date);
                    await db.SaveItemAsync(account);
                }
                else
                {
                    ErrorMessage = "Username already exist";
                }
            }
        }

        public bool CreateAccountEnabled
        {
            get
            {
                if (!string.IsNullOrEmpty(FName) && !string.IsNullOrEmpty(LName) &&
                    !string.IsNullOrEmpty(UName) && PasswordIsValid() && !NumberInValid &&
                    !DateInValid && !FNameInValid && !LNameInValid)
                {
                    return true;
                }
                return false;

            }
        }

        private bool NameInvalid(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            if (name.Contains("!") ||
                name.Contains("@") ||
                name.Contains("#") ||
                name.Contains("$") ||
                name.Contains("%") ||
                name.Contains("^") ||
                name.Contains("&"))
            {
                return true;
            }

            return false;
        }

        public bool PasswordIsValid()
        {
            if (PasswordIncorrectLength)
            {
                return false;
            }

            if (PasswordContainsNoLowerCase)
            {
                return false;
            }

            if (PasswordContainsNoUpperCase)
            {
                return false;
            }

            return true;
        }
    }
}
