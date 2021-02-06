using DSG.Presentation.ViewEntity;
using System.Collections.ObjectModel;
using System.Windows;

namespace DSG.WinUI.Validation
{
    public class ComparisonValue : DependencyObject
    {
        public ObservableCollection<GenerationProfileViewEntity> Value
        {
            get { return (ObservableCollection<GenerationProfileViewEntity>)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(ObservableCollection<GenerationProfileViewEntity>),
            typeof(ComparisonValue),
            new PropertyMetadata(default(ObservableCollection<GenerationProfileViewEntity>)));
    }
}
