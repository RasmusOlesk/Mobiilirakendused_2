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
        private readonly ObservableCollection<LanguageModel> _languages = new();
        private int _position;
        private readonly System.Timers.Timer _timer;
        private string _currentLanguage = "ET";
        private bool _isShowingDetails;

        public ObservableCollection<LanguageModel> Languages => _languages;

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
            _timer.AutoReset = true;

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
            // Kaardid luuakse ainult üks kord.
            if (Languages.Count == 0)
            {
                Languages.Add(new LanguageModel());
                Languages.Add(new LanguageModel());
                Languages.Add(new LanguageModel());
            }

            if (CurrentLanguage == "ET")
            {
                UpdateLanguage(
                    0,
                    "C#",
                    "Võimas objektorienteeritud keel",
                    "Loodud Microsofti poolt aastal 2000.\n\nKoodinäide:\nConsole.WriteLine(\"Hello World\");",
                    "csharp.png");

                UpdateLanguage(
                    1,
                    "Python",
                    "Suurepärane andmetöötluseks",
                    "Loodud aastal 1991.\n\nKoodinäide:\nprint(\"Hello World\")",
                    "python.png");

                UpdateLanguage(
                    2,
                    "JavaScript",
                    "Veebiarenduse põhikeel",
                    "Loodud aastal 1995.\n\nKoodinäide:\nconsole.log(\"Hello World\");",
                    "javascript.png");
            }
            else
            {
                UpdateLanguage(
                    0,
                    "C#",
                    "Powerful object-oriented language",
                    "Created by Microsoft in 2000.\n\nCode example:\nConsole.WriteLine(\"Hello World\");",
                    "csharp.png");

                UpdateLanguage(
                    1,
                    "Python",
                    "Great for data and automation",
                    "Created in 1991.\n\nCode example:\nprint(\"Hello World\")",
                    "python.png");

                UpdateLanguage(
                    2,
                    "JavaScript",
                    "The language of the web",
                    "Created in 1995.\n\nCode example:\nconsole.log(\"Hello World\");",
                    "javascript.png");
            }
        }

        private void UpdateLanguage(
            int index,
            string name,
            string description,
            string details,
            string imageUrl)
        {
            Languages[index].Name = name;
            Languages[index].Description = description;
            Languages[index].Details = details;
            Languages[index].ImageUrl = imageUrl;
        }

        private async void OnLanguageSelected(LanguageModel? lang)
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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"Infoakna avamine ebaõnnestus: {ex.Message}");
            }
            finally
            {
                _isShowingDetails = false;
                _timer.Start();
            }
        }

        private void OnChangeLanguage()
        {
            // Muudame keelt, säilitades kaardid ja nende järjekorra.
            CurrentLanguage = CurrentLanguage == "ET" ? "EN" : "ET";

            // Uuendame olemasolevate kaartide andmeid ja pilte.
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