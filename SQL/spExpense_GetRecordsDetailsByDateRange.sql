CREATE DEFINER=`root`@`localhost` PROCEDURE `spExpense_GetRecordsDetailsByDateRange`(
	IN userId varchar(28),
    IN startDate datetime,
    IN endDate datetime
)
BEGIN
	SELECT
		e.Id,
		e.EDate,
		b.Description AS BankDescription,
		ec.Description AS ExpenseCategoryDescription,
        ec.Color AS ExpenseCategoryColor,
		e.Comments,
		e.Amount,
		le.Address
	FROM Expense e
	JOIN Bank b ON b.Id = e.BankId
    JOIN ExpenseCategory ec ON ec.Id = e.ECategoryId
    JOIN LocationExpense le ON le.ExpenseId = e.Id
	WHERE e.UpdatedBy = userId
		AND (date(e.EDate) >= date(startDate) AND date(e.EDate) <= date(endDate))
        AND e.IsActive = TRUE
		AND e.IsArchived = FALSE
	ORDER BY e.EDate DESC;
END