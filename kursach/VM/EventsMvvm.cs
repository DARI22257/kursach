using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kursach.Model;

namespace kursach.VM
{
    internal class EventsMvvm : BaseVM
    {
        private Event newEvent = new();

        public Event NewEvent
        {
            get => newEvent;
            set
            {
                newEvent = value;
                Signal();
            }
        }

        private Event selectedEvent = new();

        public Event SelectedEvent
        {
            get => selectedEvent;
            set
            {
                selectedEvent = value;
                Signal();
            }
        }

        private ObservableCollection<Event> events = new();

        public ObservableCollection<Event> Events
        {
            get => events;
            set
            {
                events = value;
                Signal();
            }
        }


        public CommandMvvm InsertEvent { get; set; }

        public CommandMvvm NextPage { get; set; }
        public EventsMvvm()
        {
            SelectAll();
            InsertEvent = new CommandMvvm(() =>
            {
                ClientDB.GetDb().Insert(NewClient);
                NewClient = new();
                SelectAll();
            },
                () =>
                !string.IsNullOrEmpty(newClient.FullName) &&
                !string.IsNullOrEmpty(newClient.Phone) &&
                !string.IsNullOrEmpty(newClient.Email) &&
                !string.IsNullOrEmpty(newClient.Notes));

            NextPage = new CommandMvvm(() => {
                Events events = new Events(SelectedClient);
                events.Show();
                close?.Invoke();
            },
                () => SelectedClient != null);
        }

        private void SelectAll()
        {
            Clients = new ObservableCollection<Client>(ClientDB.GetDb().SelectAll());
        }
        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
