using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using campusnowfrontend.Models;
using System.Collections.ObjectModel;
using Prism.Mvvm;
using campusnowfrontend.Views;
using Prism.Commands;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace campusnowfrontend.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        public ICommand ShowCommand { get; private set; }

        public MainWindowViewModel()
        {
            ShowCommand = new DelegateCommand(ShowMethod);
        }

        private void ShowMethod()
        {
            LoginView login = new LoginView();
            login.Show();
        }

        private string _server;

        public string Server
        {
            get { return _server; }
            set 
            { 
                _server = value; 
            }
        }

        private ObservableCollection<EventRecord> _allEvents = new ObservableCollection<EventRecord>();
        public ObservableCollection<EventRecord> AllEvents
        {
            get 
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_server);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("api/event/getAllEvents").Result;
                if(response.IsSuccessStatusCode)
                {
                    var result = JObject.Parse(response.Content.ReadAsStringAsync().Result)["EventRecords"].Children().ToList();
                    result.ForEach(x => _allEvents.Add(x.ToObject<EventRecord>()));
                }

                return _allEvents;
            }
        }


    }
}
