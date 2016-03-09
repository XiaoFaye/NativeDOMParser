using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App4
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        WebView web = new WebView();

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

        public MainPage()
        {
            this.InitializeComponent();
            
            web.NavigationCompleted += web_NavigationCompleted;
            web.Navigate(new Uri("http://www.sealtheroad.co.nz/"));
        }

        async Task<string> GetSelector()
        {
            string result = await web.InvokeScriptAsync("eval", new string[] { "(typeof jQuery != 'undefined').toString()" });
            if (result == "true")
                return "jQuery";

            result = await web.InvokeScriptAsync("eval", new string[] { "(typeof $ != 'undefined').toString()" });
            if (result == "true")
                return "$";

            return "document.querySelectorAll";
        }

        async void web_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            string jqselector = await GetSelector();
            
            string selector = "title";
            string attribute = "";
            string script = string.Empty;
            string result = string.Empty;

            if (attribute == string.Empty)
                result = await web.InvokeScriptAsync("eval", new string[] { string.Format(selector.Length > 10 ? JQN : JQ, jqselector, selector) });
            else
                result = await web.InvokeScriptAsync("eval", new string[] { string.Format(selector.Length > 10 ? JQNAttr : JQAttr, jqselector, selector, attribute) });
            
        }
    }
}
