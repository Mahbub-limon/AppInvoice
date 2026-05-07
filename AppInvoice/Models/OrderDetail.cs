using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

namespace AppInvoice.Models
{
    public class OrderDetail : INotifyPropertyChanged
    {
        public string? ItemName 
        {
            get => field;
            set
            {
                if(field != value)
                {
                    field = value;
                    OnPropertyChanged(nameof(ItemName));
                }
            }
        }


        public decimal Price
        {
            get => field;
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged(nameof(Price));
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        public int Qty 
        { 
          get => field;
            set
            {
                if(field != value)
                {
                    field = value;
                    OnPropertyChanged(nameof(Qty));
                    OnPropertyChanged(nameof(Total));
                }
            }
        }


        [JsonIgnore]
        public decimal Total
        {
            get => Price * Qty;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
         
        void OnPropertyChanged(string propertyName) =>
        
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        

    }
}
