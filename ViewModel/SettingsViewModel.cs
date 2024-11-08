using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using RevitTest.Interfaces;
using RevitTest.Model;
using System.Collections.ObjectModel;

namespace RevitTest.ViewModel
{
    public partial class SettingsViewModel : ObservableValidator
    {
        private readonly AppSettings _appSettings;
        private readonly IWorkset _worksetService;
        public ICollection<IFamilyTypeViewModel> FamilyTypes { get; set; } = new Collection<IFamilyTypeViewModel>();

        [ObservableProperty] private double _width = 700;
        [ObservableProperty] private double _height = 600;
        [ObservableProperty] private bool _isSelectedWidth = true;
        [ObservableProperty] private bool _isSelectedHeight = true;

        private bool _isApplyToWorkset;

        public SettingsViewModel(AppSettings appSettings, IWorkset worksetService)
        {
            _appSettings = appSettings;
            _worksetService = worksetService;
         
        }



        public bool IsApplyToWorkset
        {
            get => _isApplyToWorkset;
            set
            {
                if (value != _isApplyToWorkset)  
                {
                    if (value)
                    {
     
                        _ = ApplyWorksetAssignmentAsync();
                    }
                    else
                    {
 
                    }
                    SetProperty(ref _isApplyToWorkset, value);
                }
            }
        }

        public async Task ApplyWorksetAssignmentAsync()
        {
            var allElements = new ObservableCollection<IFamilyTypeViewModel>(
                FamilyTypes.Where(f => f.CategoryElement == "Окна" || f.CategoryElement == "Windows" || f.CategoryElement == "Двери" || f.CategoryElement == "Doors")
                           .Select(f => f)
            );
            if (allElements.Count != 0)
            {
                await _worksetService.AssignElementsToWorksetAsync(allElements);
            }
        }



        [RelayCommand]
        private void Save()
        {
            if (Validate())
            {
                try
                {
                    _appSettings.Width = Width;
                    _appSettings.Height = Height;
                    _appSettings.IsSelectedWidth = IsSelectedWidth;
                    _appSettings.IsSelectedHeight = IsSelectedHeight;
                    _appSettings.IsApplyToWorkset = IsApplyToWorkset;

                    MessageBox.Show("Настройки сохранены.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }

        private bool Validate()
        {
            if (Width <= 0)
            {
                MessageBox.Show(nameof(Width), "Ширина должна быть больше 0.");
                return false;
            }

            if (Height <= 0)
            {
                MessageBox.Show(nameof(Height), "Высота должна быть больше 0.");
                return false;
            }
            return true;
        }
    }
}
