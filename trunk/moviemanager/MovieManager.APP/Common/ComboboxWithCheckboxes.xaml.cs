using System.Windows;

namespace MovieManager.APP.Common
{
    /// <summary>
    /// Interaction logic for ComboboxWithCheckboxes.xaml
    /// </summary>
    public partial class ComboboxWithCheckboxes
    {
        public object ItemsSource 
        { 
            get{ return GetValue(ItemsSourceProperty); } 
            set { 
                SetValue(ItemsSourceProperty, value); 
                SetText(); 
            } 
        } 

        public static readonly DependencyProperty ItemsSourceProperty = 
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(ComboboxWithCheckboxes), new UIPropertyMetadata(null)); 

        /// <summary> 
        ///Gets or sets the text displayed in the ComboBox 
       /// </summary> 
       public string Text 
        { 
            get{ return(string)GetValue(TextProperty); } 
            set{ SetValue(TextProperty, value); } 
        } 

        public static readonly DependencyProperty TextProperty = 
            DependencyProperty.Register("Text", typeof(string), typeof(ComboboxWithCheckboxes), new UIPropertyMetadata(string.Empty)); 


        /// <summary> 
        ///Gets or sets the text displayed in the ComboBox if there are no selected items 
       /// </summary> 
       public string DefaultText 
        { 
            get{ return(string)GetValue(DefaultTextProperty); } 
            set{ SetValue(DefaultTextProperty, value); } 
        } 

        // Using a DependencyProperty as the backing store for DefaultText.  This enables animation, styling, binding, etc... 
       public static readonly DependencyProperty DefaultTextProperty = 
            DependencyProperty.Register("DefaultText", typeof(string), typeof(ComboboxWithCheckboxes), new UIPropertyMetadata(string.Empty)); 
        
        /// <summary> 
        ///Whenever a CheckBox is checked, change the text displayed 
       /// </summary> 
        /// <param name="sender"></param> 
        /// <param name="e"></param> 
       private void CheckBoxClick(object sender, RoutedEventArgs e) 
        { 
            SetText(); 
        } 

        /// <summary> 
        ///Set the text property of this control (bound to the ContentPresenter of the ComboBox) 
       /// </summary> 
       private void SetText()
        {
            Text = (ItemsSource != null)
                       ? ItemsSource.ToString()
                       : DefaultText;

            // set DefaultText if nothing else selected 
            if (string.IsNullOrEmpty(Text))
            {
                Text = DefaultText;
            }
        }
    } 
}