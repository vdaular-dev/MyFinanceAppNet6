﻿namespace MyFinanceAppLibrary.Enum;

public enum Truncate
{
    FirstName = 8,
    LastName = 12,
    Bank = 30,
    Company = LastName,
    ColumnTimesheet = LastName,
    ShortMonthName = 3,
    ExpenseCategory = Bank,
    TransactionCategory = Bank
}
