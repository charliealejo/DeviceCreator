# DeviceCreator
Device creator exercise for EDMI

## Project overview
The solution is comprised of several projects:

1. **Database service**. This project runs as a service and contains the server access methods to the database. It can be accessed via RabbitMQ messaging. The database
is designed to live in memory while the service
2. **Device manager**. This project contains the client access methods to the database. It can be accessed via direct API. This project connects to the database service
via RabbitMQ messaging.
3. **Device creator web**. This is the main user interface to add devices to the database and to view which devices are already inserted in it. The web is designed in ASP.Net
MVC with Razor web pages in cshtml. It contains classes for the views and the models, with ways to check the validity of the data. The views are programmed in JQuery and
bootstrap CSS styles. This project uses the *device manager* library to connect to the database. The classes used are previously registered in the *Global.asax* class and
the controller to access the views is set up in the *RouteConfig.cs* class. Finally, every string resource shown in the user interface is registered in the *Resources.resx*
file just in case the app needs to be translated to other languages.
4. **CDeviceCreator**. This is the secondary user interface -a console application- that can be used to insert devices in the database. The application asks for the type of
device to be inserted, the specific data, and finally if the user wants to add another device. This project also uses the *Device manager* library to connect to the database.
5. **Shared**. This library contains all the constants and classes shared between the different projects.
6. **DeviceCreator.Tests**. A unit test project with some tests for some of the main classes. This project uses NUnit and Moq for the tests.

## Deployment instructions

1. RabbitMQ service must be installed previously for the system to work properly.
2. Easiest way to test the system is from inside Visual Studio, just set up both the *Database service* and the *Device creator web* as start-up projects for the solution.
3. It can also be tested via installing the *Database service* and publishing the *Device creator web* into IIS previously.
4. Either way, when the *Database service* is running, the *CDeviceCreator* application can also be executed to insert devices into the database. The devices inserted should be
visible from the device list web page.
