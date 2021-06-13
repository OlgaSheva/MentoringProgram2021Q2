# [Base] Introduction Hometask

## Description
Please, complete the following task.

## Common

You should create a website, that enables operations with the Northwind Database (script with DB structure and test data is attached)

## Base requirements:

The site should be developed using ASP.Net Core 2+;
The application should at least work on Microsoft Windows (cross-platform is fine, but not strictly required) 
MS SQL Server should be used as a DB engine (you can use any edition, including Express or LocalDB)
EF Core is used to enable data access layer
If there are no special instructions, no requirements apply to the layout and styling of the pages

## Task 1. Base site

Create a web site with the following pages:

The Home page that contains a welcome message and links to other pages
The Categories page. Shows a list of categories without images
The Products page. Shows a table with the products 
The table contains all products fields
Instead of references to Category and Supplier, their names should be shown
Note:

All configuration parameters (connection strings, etc.) can remain in the code (hardcoded)

## Task 2. Startup and configuration

Add a configuration feature that supports two parameters:

Database connection string
Maximum (M) amount of products shown on the Product page (show only first M products, others – ignored; if M == 0, then show all products)

## Task 3. Edit forms and Server-side validation

Add edit forms (New and Update) for the Products:

Related entities (such as Category) should be presented as a dropdown list
Add server-side validation for edited products (not less than 3 different rules)

##Task 4. Styling and client-side validation

Add two client libraries to the project:

Bootstrap 
jQuery Unobtrusive Validation
For Bootstrap:

Apply Bootstrap styles to site pages/forms
Change links to Categories and Product pages to navigation bar with the "hamburger" button
For jQuery Unobtrusive Validation:

Configure client-side validation by analogy with task 3.

## Task 5. Logging and error handling

Configure logging:

configure writing logs into a log file
write the following events and information
application startup (Additional information: application location - folder path)
configuration reading (Additional information: current configuration values)
Create a custom error handler, which: 

logs exception
returns error page with associated information (to look up appropriate records in the logs)
Note. To get thrown exception in the error handler you can use the following snippet:

var error = this.HttpContext.Features.Get<IExceptionHandlerFeature>().Error;