# RI\_App





💻 System Requirements

\- Operating System: Windows 10 or later

\- Development Environment: Visual Studio 2022 or newer

\- .NET SDK: .NET 8.0 or later



📦 Software Dependencies

\- ASP.NET Core MVC: For web application structure

\- Entity Framework Core: For database access and migrations

\- SQL Server Express / LocalDB: As the default database provider





\*\*Instructions\*\*



##### **-->Main menu**

* All buttons are functional, and will take you to each part of the said pages below.



##### **--> For Report Issues (Part 1)**

* Run the application via Visual Studio with https
* You may click on dark mode to change  button the website theme to dark or click it again it make it light themed. This is my chosen user engagement strategy(Accessibility)
* You may click the Blue "Report Issues" to take you to the create report page.
* You may input your report details and attach a file of your choosing then click submit, if everything is submitted it will justify it has been saved with a message.
* You click "Back to Main Menu" or the "Home" button to return to the main menu.
* You may click "View Reported Issues" to take you to the view Reports page
* This page will display all reports in a table.
* You may click the attached files names in order to view them.



##### --> For local Events (Part 2)

* There is a button called Add Event where you can input details of a event you would like to add to the list
* You can used the search bar to filter specific categories you would like filtered in the table.
* You can used the date bar to filter specific dates which you would like to see in the table.
* The recommended table will appear below the main table after you search for at least one category.
* You can use the Home button to return back to the main menu
* You can toggle light mode and dark mode with the button on the top right that may say dark or light mode.



##### --> For Service Request (Final POE/ Part 3)

* The ServiceRequest/Index.cshtml page will show you a list of requests (Currently filled with preset dummy data)
* There is a search bar which will allow you to search for a specific request based of its Description once writing the specific term then hitting search.
* The clear button will reset your search to default
* The "New Service Request" will take you to the ServiceRequest/Create.cshtml page where you can create a request.
* "View by Priority" which will take you to the ServiceRequest/PriorityQueue.cshtml page where it will who a list in descending order based of priority (priority is determined by its priority term ex. low, medium, high )
* PriorityQueue.cshtml page has a Sort toggle button that alters the table based of the priority list order between ascending and descending order
* PriorityQueue.cshtml page, there is a search bar that can be used to filter the list based of the title.
* The clear button resets the filter.



###### 

###### ***-> Data Structures Implemented\*\*\****



\*\***Binary Search Tree (BST)\*\***



**-- File**: **ServiceRequestTree**.cs

Purpose:

Used to store and retrieve all service requests in a sorted order by their Id.



**-- Operations**:



Insert(ServiceRequest) — Adds a new request in order.



Find(int id) — Searches for a request by ID.



InOrderTraversal() — Returns all requests in ascending order (by ID).



**-- Usage**:

Displayed in the Index view of the ServiceRequestController.


================================================================================================



\*\***Heap / Priority Queue\*\***



**-- File: ServiceRequestHeap.cs**

Purpose:

Maintains all requests ordered by their Priority (High → Medium → Low).



**-- Operations:**



Insert(ServiceRequest) — Adds a new request based on priority.



ExtractHighestPriority() — Removes and returns the most urgent request.



GetAll() — Returns all requests in heap order.



**-- Usage:**

Displayed in the PriorityQueue view of the ServiceRequestController.

Users can toggle between Descending (High → Low) and Ascending (Low → High) order.





