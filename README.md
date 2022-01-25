# crm
Currency update program as a recruitment task.
Project contains WEB Application written in .NET 6 which takes exchange rates from external API and enters them into its own database. The application also serves as an API endpoint for GET, PUT, POST , DELETE
commands with APIKey authorization. Project also includes a console application .NET 6 (Synchronizacja_walut)  which, when called, updates the exchange rates in the database. The application returns the message "Currencies updated" if the operation was successful, otherwise the application returns the error description.
