using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace TimerControl
{
    public class AppContext: ApplicationContext
    {
        public NotifyIcon NotifyIcon { get; set; }
        public Keys TimerStartKey { get; set; }
        public Keys TimerStopKey { get; set; }

        private KeyboardHook hook = new KeyboardHook();
        private HttpClient client = new HttpClient();
        private DateTime lastAction = DateTime.Now;

        public AppContext() :base()
        {
            NotifyIcon = new NotifyIcon();
            try
            {
                SetupHotkeys();
                SetupIconAndMenu();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                Exit(this, EventArgs.Empty);
            }
        }

        void hook_KeyPressed(object? sender, KeyPressedEventArgs e)
        {
            //MessageBox.Show(e.Key.ToString());
            if (e.Key == TimerStartKey) StartTimer();
            if (e.Key == TimerStopKey) StopTimer();

        }

        private Icon GetIconFromResource()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            if (executingAssembly == null) throw new Exception("Setting icon: Assembly could not be loaded");
            using (Stream? iconstream = executingAssembly.GetManifestResourceStream("TimerControl.Icons.clock.ico"))
            {
                if (iconstream == null) throw new Exception("Setting icon: Icon could not be loaded");
                return new Icon(iconstream);
            }
        }

        private void SetupHotkeys()
        {
            hook.KeyPressed += new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
            if (!Enum.TryParse(typeof(Keys), Settings.TimerStartKey, out var timerStartKey)) throw new Exception("Invalid value in settings: TimerStart!");
            if (!Enum.TryParse(typeof(Keys), Settings.TimerStopKey, out var timerStopKey)) throw new Exception("Invalid value in settings: TimerStop!");
            TimerStartKey = (Keys)timerStartKey;
            TimerStopKey = (Keys)timerStopKey;
            hook.RegisterHotKey(TimerControl.ModifierKeys.None, (Keys)timerStartKey);
            hook.RegisterHotKey(TimerControl.ModifierKeys.None, (Keys)timerStopKey);
        }

        private void SetupIconAndMenu()
        {
            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("Exit", null, Exit);

            NotifyIcon = new NotifyIcon();
            NotifyIcon.Icon = GetIconFromResource();
            NotifyIcon.ContextMenuStrip = new ContextMenuStrip();
            NotifyIcon.ContextMenuStrip.Items.AddRange(new ToolStripItem[] { exitMenuItem });
            NotifyIcon.Text = "Timer Control [Start: " + Settings.TimerStartKey + ", Stop: " + Settings.TimerStopKey + "]";
            NotifyIcon.Visible = true;
        }

        private void StartTimer()
        {
            try 
            {
                if (lastAction < DateTime.Now - TimeSpan.FromSeconds(1))
                {
                    var response = RESTClient.Post(Settings.EndpointURL, Settings.TimerStartPostData);
                    lastAction = DateTime.Now;
                }
            }
            catch { }
        }

        private void StopTimer()
        {
            try
            {
                if (lastAction < DateTime.Now - TimeSpan.FromSeconds(1))
                {
                    var response = RESTClient.Post(Settings.EndpointURL, Settings.TimerStopPostData);
                    lastAction = DateTime.Now;
                }
            }
            catch { }
        }

        private void Exit(object? sender, EventArgs e)
        {
            NotifyIcon.Visible = false;
            NotifyIcon.Dispose();
            Application.Exit();
        }

        ~AppContext()
        {
            NotifyIcon.Visible = false;
            NotifyIcon.Dispose();
        }
    }
}
