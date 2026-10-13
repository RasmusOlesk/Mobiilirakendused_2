using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace CarouselView.Models
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<LanguageModel> _languages = new();
        private int _position;
        private readonly System.Timers.Timer _timer;
        private string _currentLanguage = "ET";
        private bool _isShowingDetails;

        public ObservableCollection<LanguageModel> Languages
        {
            get => _languages;
            set
            {
                _languages = value;
                OnPropertyChanged();
            }
        }

        public int Position
        {
            get => _position;
            set
            {
                if (_position == value)
                    return;

                _position = value;
                OnPropertyChanged();
            }
        }

        public string CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (_currentLanguage == value)
                    return;

                _currentLanguage = value;
                OnPropertyChanged();
            }
        }

        public ICommand ItemSelectedCommand { get; }
        public ICommand ChangeLanguageCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainPageViewModel()
        {
            ItemSelectedCommand =
                new Command<LanguageModel>(OnLanguageSelected);

            ChangeLanguageCommand =
                new Command(OnChangeLanguage);

            LoadLanguages();

            _timer = new System.Timers.Timer(4000);

            _timer.Elapsed += (sender, args) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (!_isShowingDetails && Languages.Count > 0)
                    {
                        Position = (Position + 1) % Languages.Count;
                    }
                });
            };

            _timer.Start();
        }

        public void LoadLanguages()
        {
            Languages.Clear();

            if (CurrentLanguage == "ET")
            {
                Languages.Add(new LanguageModel
                {
                    Name = "C#",
                    Description = "Võimas objektorienteeritud keel",
                    ImageUrl = "https://microsoft.com",
                    Details = "Loodud Microsofti poolt aastal 2000.\n\nKoodinäide:\nConsole.WriteLine(\"Hello World\");"
                });

                Languages.Add(new LanguageModel
                {
                    Name = "Python",
                    Description = "Suurepärane andmetöötluseks",
                    ImageUrl = "https://wikimedia.org",
                    Details = "Loodud aastal 1991.\n\nKoodinäide:\nprint(\"Hello World\")"
                });

                Languages.Add(new LanguageModel
                {
                    Name = "JavaScript",
                    Description = "Veebiarenduse põhikeel",
                    ImageUrl = "https://wikimedia.org",
                    Details = "Loodud aastal 1995.\n\nKoodinäide:\nconsole.log(\"Hello World\");"
                });
            }
            else
            {
                Languages.Add(new LanguageModel
                {
                    Name = "C#",
                    Description = "Powerful object-oriented language",
                    ImageUrl = "https://microsoft.com",
                    Details = "Created by Microsoft in 2000.\n\nCode example:\nConsole.WriteLine(\"Hello World\");"
                });

                Languages.Add(new LanguageModel
                {
                    Name = "Python",
                    Description = "Great for data and automation",
                    ImageUrl = "https://wikimedia.org",
                    Details = "Created in 1991.\n\nCode example:\nprint(\"Hello World\")"
                });

                Languages.Add(new LanguageModel
                {
                    Name = "JavaScript",
                    Description = "The language of the web",
                    ImageUrl = "https://wikimedia.org",
                    Details = "Created in 1995.\n\nCode example:\nconsole.log(\"Hello World\");"
                });
            }

            if (Position >= Languages.Count)
            {
                Position = 0;
            }
        }

        private async void OnLanguageSelected(LanguageModel lang)
        {
            if (lang == null || _isShowingDetails)
                return;

            _isShowingDetails = true;
            _timer.Stop();

            try
            {
                var mainPage = Application.Current?.Windows.Count > 0
                    ? Application.Current.Windows[0].Page
                    : null;

                if (mainPage != null)
                {
                    string title = CurrentLanguage == "ET"
                        ? "Keele info"
                        : "Language Info";

                    string close = CurrentLanguage == "ET"
                        ? "Sulge"
                        : "Close";

                    await mainPage.DisplayAlertAsync(
                        title,
                        lang.Details,
                        close);
                }
            }
            finally
            {
                _isShowingDetails = false;
                _timer.Start();
            }
        }

        private void OnChangeLanguage()
        {
            CurrentLanguage = CurrentLanguage == "ET" ? "EN" : "ET";
            LoadLanguages();
        }

        protected void OnPropertyChanged(
            [CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}