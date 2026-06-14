using EshopCore.Models;
using EshopCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EshopCore.InterfacesWeb
{
    public interface IOrderServiceWeb
    {
        // Tworzenie zamówienia
        Task<bool> CreateOrder(int userId);

        // Wyświetlanie historii zamówień zalogowanego użytkownika
        Task<List<Order>> LoggedInUserOdrersHistory(int userId);

        // Wyświetlanie historii zamówień podanego użytkownika
        Task<List<Order>> GetUserOrdersHistoryByUsername(string username);

        // Wyświetlanie historii wszystkich zamówień
        Task<List<Order>> GetAllUsersOrders();

        // Zmiana statusu zamówienia
        Task<bool> ChangeOrderStatus(int orderId, OrderStatus newStatus);
    }
}
