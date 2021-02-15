using System.Windows;
using System.Windows.Data;

namespace DSG.WinUI.Validation
{
    public class ComparisonValue<TEntity> : DependencyObject
    {
        public TEntity Value
        {
            get { return (TEntity)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(TEntity),
            typeof(ComparisonValue<TEntity>), new PropertyMetadata(default(TEntity))); 
    }
}
