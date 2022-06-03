using Xamarin.Forms;

namespace XamarinAccountSetupDemo.Controls
{
    public class DateEntry : Behavior<Entry>
    {
        public static DateEntry Instance = new DateEntry();

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            if (!string.IsNullOrWhiteSpace(args.NewTextValue))
            {
                if (args.OldTextValue != null && args.NewTextValue.Length < args.OldTextValue.Length)
                {
                    return;
                }

                var value = args.NewTextValue;

                if (value.Length == 2)
                {
                    ((Entry)sender).Text += "/";
                    return;
                }

                if (value.Length == 5)
                {
                    ((Entry)sender).Text += "/";
                    return;
                }

                ((Entry)sender).Text = args.NewTextValue;
            }
        }
    }
}
