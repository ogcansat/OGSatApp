using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Threading;

using Xamarin.Forms;
using Timer = System.Timers.Timer;
using System.Linq;

namespace OGSatApp.Pages.Behaviors
{
    public static class GUIAnimations
    {
        public static CancellationTokenSource DotLoadingAnimation(Label label, string title, int length, double interval)
        {

            var token = new CancellationTokenSource();
            Timer timer = new Timer(interval);
            timer.Elapsed += (s, a) =>
            {
                if (token.IsCancellationRequested)
                {
                    Device.InvokeOnMainThreadAsync(() => label.Text = "");
                    timer.Stop();
                    return;
                }

                Device.InvokeOnMainThreadAsync(() =>
                {
                    label.Text = !label.Text.StartsWith(title) ? title : label.Text;
                    label.Text += ".";
                    if (label.Text.Length >= title.Length + length) label.Text = title;
                });

                
            };
            timer.Start();

            return token;
           
        }

        public static void UpdateData(string dataLine, Dictionary<string, Label> items, Label updateTime)
        {

            string[] lines = dataLine.Split('\n');
            foreach (var item in lines)
            {
                string[] values = item.Split(':');

                if (string.IsNullOrWhiteSpace(values[0]))
                    continue;

                var result = items.FirstOrDefault(x => x.Key == values[0]);
                if (result.Key != null)
                    result.Value.Text = values[1] + (result.Value.Text.Split(' ').Length >= 2 ? " " + result.Value.Text.Split(' ')[1] : "");
            }

            updateTime.Text = "Poslední aktualizace: " + DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
