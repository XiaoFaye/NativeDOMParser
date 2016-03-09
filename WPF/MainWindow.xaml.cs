using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1
{
    //WebBrowser wb = new WebBrowser();

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //{0} = selector, {1} = selector text
        string JQ = "function GetJQValue() {{ if ({0}('{1}').length == 1) {{" +
                                                                                "return {0}('{1}')[0].outerHTML; }}" +
                                                                                " else if ({0}('{1}').length > 1) {{" +
                                                                                " var allhtml = '';" +
                                                                                " {0}('{1}').each(function() {{allhtml=allhtml+{0}(this)[0].outerHTML+'\\r\\n';}});" +
                                                                                " return allhtml;}}" +
                                                                                " else return 'no item found.';}} GetJQValue();";
        //N means Native JS, {0} = selector, {1} = selector text
        string JQN = "function GetJQValue() {{ if ({0}('{1}').length == 1) {{" +
                                                                                "return {0}('{1}')[0].outerHTML; }}" +
                                                                                " else if ({0}('{1}').length > 1) {{" +
                                                                                " var allhtml = '';" +
                                                                                " for (var i = 0; i < {0}('{1}').length; i++) allhtml=allhtml+{0}('{1}')[i].outerHTML+'\\r\\n';" +
                                                                                " return allhtml;}}" +
                                                                                " else return 'no item found.';}} GetJQValue();";

        //{0} = selector, {1} = selector text, {2} = attribute text
        string JQAttr = "function GetJQValue() {{ if ({0}('{1}').length == 1) {{" +
                                                                                "return {0}('{1}').attr('{2}'); }}" +
                                                                                " else if ({0}('{1}').length > 1) {{" +
                                                                                " var allhtml = '';" +
                                                                                " {0}('{1}').each(function() {{allhtml=allhtml+{0}(this).attr('{2}')+'\\r\\n';}});" +
                                                                                " return allhtml;}}" +
                                                                                " else return 'no item found.';}} GetJQValue();";

        //N means Native JS, {0} = selector, {1} = selector text, {2} = attribute text
        string JQNAttr = "function GetJQValue() {{ if ({0}('{1}').length == 1) {{" +
                                                                                "return {0}('{1}')[0].{2}; }}" +
                                                                                " else if ({0}('{1}').length > 1) {{" +
                                                                                " var allhtml = '';" +
                                                                                " for (var i = 0; i < {0}('{1}').length; i++) allhtml=allhtml+{0}('{1}')[i].{2}+'\\r\\n';" +
                                                                                " return allhtml;}}" +
                                                                                " else return 'no item found.';}} GetJQValue();";

        string jqselector = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            wb.LoadCompleted += wb_LoadCompleted;
        }

        void wb_LoadCompleted(object sender, NavigationEventArgs e)
        {
            if (selector.Text.Trim() == string.Empty)
                return;

            if (wb.Document != null)
            {
                if (jqselector == string.Empty)
                    jqselector = GetSelector();

                if (attribute.Text.Trim() == string.Empty)
                    text.Text = wb.InvokeScript("eval", new string[] { string.Format(jqselector.Length > 10 ? JQN : JQ, jqselector, selector.Text.Trim()) }).ToString();
                else
                    text.Text = wb.InvokeScript("eval", new string[] { string.Format(jqselector.Length > 10 ? JQNAttr : JQAttr, jqselector, selector.Text, attribute.Text) }).ToString();
            }
        }

        string GetSelector()
        {
            string result = wb.InvokeScript("eval", new string[] { "(typeof jQuery != 'undefined').toString()" }).ToString();
            if (result == "true")
                return "jQuery";

            result = wb.InvokeScript("eval", new string[] { "(typeof $ != 'undefined').toString()" }).ToString();
            if (result == "true")
                return "$";

            return "document.querySelectorAll";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (wb.Document == null)// || errorCatch == true || wb.Url.Host != (new Uri(txtUrl.Text.Trim()).Host))
            {
                wb.Navigate(new System.Uri(url.Text.Trim(), System.UriKind.Absolute));
            }
            else
                wb_LoadCompleted(sender, null);
        }
    }
}
