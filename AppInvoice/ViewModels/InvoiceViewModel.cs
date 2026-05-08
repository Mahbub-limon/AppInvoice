using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AppInvoice.Models;

namespace AppInvoice.ViewModels
{
    public class InvoiceViewModel : INotifyPropertyChanged
    {
        private string _invoiceNumber;
        private string _customerName;
        private OrderDetail _selectedItem;
        private decimal _subTotal;
        private decimal _tax;
        private decimal _grandTotal;

        public ObservableCollection<OrderDetail> Items { get; set; }

        public string InvoiceNumber
        {
            get => _invoiceNumber;
            set
            {
                _invoiceNumber = value;
                OnPropertyChanged();
            }
        }

        public string CustomerName
        {
            get => _customerName;
            set
            {
                _customerName = value;
                OnPropertyChanged();
            }
        }

        public OrderDetail SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        public decimal SubTotal
        {
            get => _subTotal;
            set
            {
                _subTotal = value;
                OnPropertyChanged();
            }
        }

        public decimal Tax
        {
            get => _tax;
            set
            {
                _tax = value;
                OnPropertyChanged();
                CalculateGrandTotal();
            }
        }

        public decimal GrandTotal
        {
            get => _grandTotal;
            set
            {
                _grandTotal = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddItemCommand { get; }
        public ICommand RemoveItemCommand { get; }
        public ICommand SaveInvoiceCommand { get; }
        public ICommand ClearCommand { get; }

        // New item properties
        private string _newItemName;
        private decimal _newItemPrice;
        private int _newItemQty;

        public string NewItemName
        {
            get => _newItemName;
            set
            {
                _newItemName = value;
                OnPropertyChanged();
            }
        }

        public decimal NewItemPrice
        {
            get => _newItemPrice;
            set
            {
                _newItemPrice = value;
                OnPropertyChanged();
            }
        }

        public int NewItemQty
        {
            get => _newItemQty;
            set
            {
                _newItemQty = value;
                OnPropertyChanged();
            }
        }

        public InvoiceViewModel()
        {
            Items = new ObservableCollection<OrderDetail>();

            // Subscribe to CollectionChanged event to update totals
            Items.CollectionChanged += (s, e) => CalculateTotals();

            // Initialize commands
            AddItemCommand = new Command(AddItem, CanAddItem);
            RemoveItemCommand = new Command<OrderDetail>(RemoveItem);
            SaveInvoiceCommand = new Command(SaveInvoice);
            ClearCommand = new Command(ClearAll);

            // Set default values
            Tax = 0; // 0% tax, users can change
            NewItemQty = 1;
        }

        private bool CanAddItem()
        {
            return !string.IsNullOrWhiteSpace(NewItemName) && NewItemPrice > 0 && NewItemQty > 0;
        }

        private void AddItem()
        {
            var newItem = new OrderDetail
            {
                ItemName = NewItemName,
                Price = NewItemPrice,
                Qty = NewItemQty
            };

            // Subscribe to property changes
            newItem.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(OrderDetail.Total))
                    CalculateTotals();
            };

            Items.Add(newItem);

            // Clear new item fields
            NewItemName = string.Empty;
            NewItemPrice = 0;
            NewItemQty = 1;

            // Re-evaluate CanAddItem
            ((Command)AddItemCommand).ChangeCanExecute();
        }

        private void RemoveItem(OrderDetail item)
        {
            if (item != null)
            {
                Items.Remove(item);
            }
        }

        private void CalculateTotals()
        {
            SubTotal = Items.Sum(item => item.Total);
            CalculateGrandTotal();
        }

        private void CalculateGrandTotal()
        {
            GrandTotal = SubTotal + (SubTotal * Tax / 100);
        }

        private void SaveInvoice()
        {
            if (string.IsNullOrWhiteSpace(InvoiceNumber))
            {
                Application.Current.MainPage.DisplayAlert("Error", "Please enter Invoice Number", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(CustomerName))
            {
                Application.Current.MainPage.DisplayAlert("Error", "Please enter Customer Name", "OK");
                return;
            }

            if (Items.Count == 0)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Please add at least one item", "OK");
                return;
            }

            var order = new Order
            {
                OrderId = InvoiceNumber,
                CustomerId = CustomerName,
                OrderDetails = Items.ToList()
            };

            // Save logic here - can save to file or database
            Application.Current.MainPage.DisplayAlert("Success",
                $"Invoice {InvoiceNumber} saved successfully!\nTotal: {GrandTotal:C}", "OK");
        }

        private void ClearAll()
        {
            InvoiceNumber = string.Empty;
            CustomerName = string.Empty;
            Items.Clear();
            NewItemName = string.Empty;
            NewItemPrice = 0;
            NewItemQty = 1;
            Tax = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}