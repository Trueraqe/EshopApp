using EshopCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EshopCore.Interfaces
{
    public interface IValidators
    {
        // Sprawdzanie zgodności PasswordHash
        static abstract bool VerifyUserPassword(User user, string inputPassword);

        // Konwertowanie daty na string "yyyy-MM-dd HH:mm:ss"
        static abstract string ClearDateTime();

        // Absolute Path do SQLite
        static abstract string SQL_DB_Path(string fileName);

        // Zapis danych do folderu projektu
        static abstract void WriteFilePath(string fileName, string content);

        // Zmiana koloru tekstu wypisanego w konsoli
        static abstract void ChangeConsole(ConsoleColor color, string input);

        // Konwersja i walidacja string na integer (TryParse)
        static abstract int WriteReadInt(string write);

        // Konwersja i walidacja string na decimal (TryParse)
        static abstract decimal WriteReadDeci(string write);

        // Walidacja string (IsLetter)
        static abstract string WriteReadStr(string write);

        // Walidacja pustego pola (IsLetterOrDigit)
        static abstract string WriteReadBoth(string write);

        // Walidacja opłaty zamówienia
        static abstract bool Payment();

        // Zmiana statusu zamówienia
        static abstract Task OrderStatusChange(Order order);
    }
}
