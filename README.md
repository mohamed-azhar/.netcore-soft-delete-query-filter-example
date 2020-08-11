# Soft Delete Query Filter with Entity Framework Core and C# 
A simple console app made with .NET Core and C# to show how soft delete query filter can be applied with Entity Framework Core

#### What is Soft Delete?
Deleting any data within a database is a risky task. Often, rather than deleting the records and permanently losing it, its better just not to delete and instead hide it from the users. This practice allows for better system auditability. Simply this can be referred to as 'soft deleting'.

#### How it Works
* Every entity of the database contains a boolean property named 'IsDeleted'
* The default value of the IsDeleted property is 'false' which is the undeleted state
* When a record of any entity is deleted from the application, the 'IsDeleted' field is set to 'true'
* When configured properly, after deleting a record, the database queries wont load the records which have the 'IsDeleted' property set to 'true'

#### Application Help
* On console app launch, a database is created with a single table named "Books" with seeded data
* The user will be greeted to choose from a list of options containing, view application help, view all the books and delete a book
* Help will provide general information related to the application
* View all the books will display all the books from the table in a table
* Delete a book will let the user to enter an Id of the book for deletion
* After deletion, when observed from the database, the record is actually not removed from the database instead the 'IsDeleted' property is set to 'true'.

#### Requirements
✅ .NET Core 3.1 SDK+
✅ SQL Server
