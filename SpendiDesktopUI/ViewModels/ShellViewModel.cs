using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using SpendiDesktopUI.EventModels;

namespace SpendiDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogInEvent>
    {
        private IEventAggregator _events;
        private SalesViewModel _salesVM;
        private SimpleContainer _container;

        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM, SimpleContainer container)
        {
            _events = events;
            _container = container;
            _salesVM = salesVM;

            _events.Subscribe(this);

            ActivateItem(_container.GetInstance<LoginViewModel>());
        }
        
        public void Exit()
        {
            TryClose();
        }

        public void LogOut()
        {

        }

        public void Handle(LogInEvent message)
        {
            ActivateItem(_salesVM);            
        }
    }
}
