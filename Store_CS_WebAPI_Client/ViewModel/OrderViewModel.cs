using Store_CS_WebAPI_Client.Infrastructure;
using Store_CS_WebAPI_Client.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Windows.Input;

namespace Store_CS_WebAPI_Client.ViewModel
{
    /// <summary>
    /// The View Model class.
    /// This defines the public properties used for Databinding with UI. It also defines 
    /// asunchronous methods making call o WEB API for performing 
    /// HTTP GET|POST|PUT|DELETE operations. The command objects are
    /// declared which are bind with the Button on UI
    /// </summary>
    public class OrderViewModel : INotifyPropertyChanged
    {
        //The URL for WEB API
        string remoteURL = "http://localhost:12124/api/";

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string pName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(pName)); 
            }
        }

        ObservableCollection<Order> _Orders;

        /// <summary>
        /// Orders collection  to display in the ListView
        /// </summary>
        public ObservableCollection<Order> Orders
        {
            get { return _Orders; }
            set 
            {
                _Orders = value;
                OnPropertyChanged("Orders");
            }
        }

        Order _Order;

        /// <summary>
        /// The Object for the new Order
        /// </summary>
        public Order Order
        {
            get { return _Order; }
            
            set
            {
                _Order = value;
                if (Order != null)
                {
                    OrderId = Order.OrderId;
                    TotalBill = Order.TotalBill;
                }
                OnPropertyChanged("Order");
            }
        }

        Order _SelectedOrder;
        /// <summary>
        /// The object for the Selected Order to Update and Delete order 
        /// </summary>
        public Order SelectedOrder
        {
            get { return _SelectedOrder; }

            set
            {
                _SelectedOrder = value;
                Order = _SelectedOrder;
                OnPropertyChanged("Order");
            }
        }


        int _OrderId = 0;
        /// <summary>
        /// OrderId generated when the new order is added
        /// </summary>
        public int OrderId
        {
            get { return _OrderId; }
            set 
            {
                _OrderId = value;
                OnPropertyChanged("OrderId");
            }
        }

        int _TotalBill = 0;
        /// <summary>
        /// The total bill calculated aftre order is created or updated
        /// </summary>
        public int TotalBill
        {
            get { return _TotalBill; }
            set
            {
                _TotalBill = value;
                OnPropertyChanged("TotalBill");
            }
        }

 

         
        //Command objects
        public ICommand NewOrders { get; private set; }
        public ICommand AddOrder { get; private set; }
        public ICommand UpdateOrder { get; private set; }
        public ICommand DeleteOrder { get; private set; }

        public OrderViewModel()
        {
            NewOrders = new RoutedCommand(NewOrder);
            AddOrder = new RoutedCommand(CreateOrder);
            UpdateOrder = new RoutedCommand(EditOrder);
            DeleteOrder = new RoutedCommand(RemoveOrder);

            LoadOrders();
            Order = new Order();
        }

        /// <summary>
        /// Method to Load all The Orders
        /// S1: Create an object of HttpClient
        /// S2: Make asn async call to WEB API using 'GetAsync' method.
        /// S3: Read the response stream and read the data to the Ordes collection
        /// </summary>
        async void LoadOrders()
        {
            using (HttpClient client = new HttpClient())
            {
                var Response = await client.GetAsync(new Uri(remoteURL+"Order"));
                using (var responseStream = await Response.Content.ReadAsStreamAsync())
                {
                    var OrdersList = new DataContractJsonSerializer(typeof(List<Order>));
                    Orders = new ObservableCollection<Order>((IEnumerable<Order>)OrdersList.ReadObject(responseStream));
                }
            }
        }
       
        /// <summary>
        /// Method to Create a new Order 
        /// S1: Create an object of HttpClient
        /// S2: Make asn async call to WEB API using 'PosttAsync' method and pass the 'Order' object.
        /// </summary>
        /// <param name="o"></param>
        async void CreateOrder(object o)
        {
          Order.TotalBill = Order.OrderedQuantity * Order.UnitPrice;

            Order.OrderedDate= DateTime.Now.ToString();

            using (var client = new HttpClient())
            {
                using (var memStream = new MemoryStream())
                {
                    var data = new DataContractJsonSerializer(typeof(Order));
                    data.WriteObject(memStream, Order);
                    memStream.Position = 0;
                    var contentToPost = new StreamContent(memStream);
                    contentToPost.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var response = await client.PostAsync(new Uri(remoteURL + "Order"),contentToPost);

                    var dataReceived = response.EnsureSuccessStatusCode();
           
        
                    var dRec = new DataContractJsonSerializer(typeof(Order));

                    var str = await dataReceived.Content.ReadAsStreamAsync();
                    var RecData = dRec.ReadObject(str) as Order;
                    OrderId = RecData.OrderId;
                    TotalBill = Order.TotalBill;

                }

            }
            LoadOrders();
        }

        /// <summary>
        /// The new Order
        /// </summary>
        /// <param name="o"></param>
        void NewOrder(object o)
        {
            Order = new Order(); 
        }

        /// <summary>
        /// The Edit Order
        /// S1: Create an object of HttpClient
        /// S2: Make an async call to WEB API using 'PutAsync' method and pass the 'Order' object.
        /// </summary>
        /// <param name="o"></param>
        async void EditOrder(object o)
        {
            Order.TotalBill = Order.OrderedQuantity * Order.UnitPrice;
            using (var client = new HttpClient())
            {
                using (var memStream = new MemoryStream())
                {
                    var data = new DataContractJsonSerializer(typeof(Order));
                    data.WriteObject(memStream, Order);
                    memStream.Position = 0;
                    var contentToPost = new StreamContent(memStream);
                    contentToPost.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var response = await client.PutAsync(new Uri(remoteURL + "Order/" + Order.OrderId), contentToPost);
                    
                    TotalBill = Order.TotalBill;
                }
            }
            LoadOrders();
        }
        /// <summary>
        /// The Delete Order
        /// S1: Create an object of HttpClient
        /// S2: Make an async call to WEB API using 'DeleteAsync' method and pass the 'OrderId'.
        /// </summary>
        /// <param name="o"></param>
        async void RemoveOrder(object o)
        {
            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync(new Uri(remoteURL + "Order/" + Order.OrderId));
                response.EnsureSuccessStatusCode();
            }
            Order = new Order(); 
            LoadOrders();
        }
    }
}
