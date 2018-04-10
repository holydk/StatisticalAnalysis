using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace StatisticalAnalysis.WpfClient.Models
{
    public class Navigation : INavigation, INotifyPropertyChanged
    {
        protected readonly Dictionary<Type, Func<UserControl>> contentResolvers;

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<NavigationEventArgs> Navigated;

        public bool CanGoBack
        {
            get
            {
                if (contentResolvers.Count <= 1)
                    return false;

                var type = Content.GetType();

                return contentResolvers
                    .Keys
                    .ToList()
                    .IndexOf(type) > 0;               
            }
        }

        public bool CanGoForward
        {
            get
            {
                if (contentResolvers.Count <= 1)
                    return false;

                var type = Content.GetType();

                return contentResolvers
                    .Keys
                    .ToList()
                    .IndexOf(type) < contentResolvers.Count - 1;
            }
        }

        private UserControl _content;

        public UserControl Content
        {
            get => _content;
            private set
            {
                _content = value;
                OnPropertyChanged(nameof(Content));
            }     
        }

        public Navigation()
        {
            contentResolvers = new Dictionary<Type, Func<UserControl>>();
        }

        private void AddType(Type type, Func<UserControl> func)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (func == null) throw new ArgumentNullException(nameof(func));

            if (!contentResolvers.ContainsKey(type))
                contentResolvers.Add(type, func);
        }     
        
        public bool IsCurrentContent(Type type) => type.IsInstanceOfType(Content);

        protected virtual void GoTo(Type type, NavigationState state)
        {
            if (contentResolvers.ContainsKey(type) && !IsCurrentContent(type))
            {
                Content = contentResolvers[type]();
                OnNavigated(new NavigationEventArgs(state, Content));
            }
        }

        public void GoTo<T>(Func<T> func) where T : UserControl
        {
            var type = func.Method.ReturnType;

            AddType(type, func);
            GoTo(type, NavigationState.GoForward);
        }

        public async Task GoToAsync<T>(Func<T> func) where T : UserControl
        {
            await Application.Current.Dispatcher.InvokeAsync(() => GoTo(func));
        }

        public async Task GoToAsync(Type type)
        {
            await Application.Current.Dispatcher.InvokeAsync(() => GoTo(type, NavigationState.GoForward));
        }

        public async Task GoBackAsync(Type type)
        {
            await Application.Current.Dispatcher.InvokeAsync(() => GoTo(type, NavigationState.GoBack));
        }

        public void GoBack()
        {
            if (!CanGoBack) return;

            var index = 
                contentResolvers
                .Keys
                .ToList()
                .IndexOf(Content.GetType());

            GoTo(contentResolvers.Values.ToList()[index - 1].Method.ReturnType, NavigationState.GoBack);            
        }

        public async Task GoBackAsync()
        {
            await Application.Current.Dispatcher.InvokeAsync(() => GoBack());
        }
  
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }                

        protected virtual void OnNavigated(NavigationEventArgs e)
        {
            Navigated?.Invoke(this, e);
        }
    }
}
