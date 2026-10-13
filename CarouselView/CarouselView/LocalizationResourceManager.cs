using System.ComponentModel;
using System.Globalization;
using global::CarouselView;


// Asenda see rida oma AppResources faili tegeliku asukohaga, kui see asub mujal:



// Asenda see rida oma AppResources faili tegeliku asukohaga, kui see asub mujal:

namespace CarouselView
{
    public class LocalizationResourceManager : INotifyPropertyChanged
    {
        public static LocalizationResourceManager Instance { get; } = new();

        // Lisatud ?. ja ?? kontrollid, et vältida null-viite (Dereference of a possibly null reference) vigu
        public object this[string resourceKey] =>
            AppResources.ResourceManager?.GetObject(resourceKey, AppResources.Culture) ?? resourceKey;

        // Lisatud küsimärk 'PropertyChangedEventHandler?', mis parandab implikatsiooni vea INotifyPropertyChanged liidesega
        public event PropertyChangedEventHandler? PropertyChanged;

        public void SetCulture(CultureInfo culture)
        {
            AppResources.Culture = culture;
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            // Sündmuse käivitamine kontrollib nüüd ohutult, kas kuulajaid on olemas
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }
}
