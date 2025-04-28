using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kursach.Model;

namespace kursach.VM
{
    internal class ClientsMvvm : BaseVM
    {
        private Client newClient = new();

        public Client NewClient
        {
            get => newClient;
            set
            {
                newClient = value;
                Signal();
            }
        }

        public CommandMvvm InsertClient { get; set; }
        public ClientsMvvm()
        {
            InsertClient = new CommandMvvm(() =>
            {
                ClientDB.GetDb().Insert(NewClient);
                close?.Invoke();
            },
                () =>
                !string.IsNullOrEmpty(newClient.FullName) &&
                !string.IsNullOrEmpty(newClient.Phone) &&
                !string.IsNullOrEmpty(newClient.Email) &&
                !string.IsNullOrEmpty(newClient.Notes));
        }
        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
