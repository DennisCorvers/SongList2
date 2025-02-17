using System;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows;

namespace SongList2.Controls
{
    public class TimedTextbox : TextBox
    {
        private readonly DispatcherTimer m_delay;

        public event EventHandler<string>? TextChangedAfterDelay;

        public static readonly DependencyProperty SearchDelayProperty =
            DependencyProperty.Register("SearchDelay", typeof(int), typeof(TimedTextbox), new PropertyMetadata(250, OnSearchDelayChanged));

        public TimedTextbox()
        {
            m_delay = new DispatcherTimer();
            m_delay.Tick += SearchTimer_Tick!;
            TextChanged += TimedTextBox_TextChanged;
            m_delay.Interval = TimeSpan.FromMilliseconds(SearchDelay);
        }

        public int SearchDelay
        {
            get { return (int)GetValue(SearchDelayProperty); }
            set { SetValue(SearchDelayProperty, value); }
        }

        private static void OnSearchDelayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (TimedTextbox)d;
            control.m_delay.Interval = TimeSpan.FromMilliseconds((int)e.NewValue);
        }

        private void TimedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            m_delay.Stop();
            m_delay.Start();
        }

        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            m_delay.Stop();
            OnTextChangedAfterDelay(Text);
        }

        protected virtual void OnTextChangedAfterDelay(string text)
        {
            TextChangedAfterDelay?.Invoke(this, text);
        }
    }
}
