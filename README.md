# searchTextBox

To test the application

1/ clone the repository https://github.com/cristianpaun/searchTextBox.git or download as zip file

2/ In visual studio 2019 open solution BackEnd/BackEnd.sln

3/ Restore nuget packeges

4/ Open appsettings.json from API project and change the databse server name with  your server name
   "DatabaseConnection": "Server=LAPTOP-6J8UE4OB\\SQLEXPRESS;Initial Catalog=ProductsDb;Integrated Security=true;" 
   My server name is LAPTOP-6J8UE4OB\\SQLEXPRESS. Change this name with your server name.

5/ Build and run the project (the API project should be the startup project)
	the WEB API run on http://localhost:5000

6/ open a command window and go to the UI folder (this is the frontend app)
	a/ enter the following command in order to download npm modules:
		npm install
	b/ start the application:
		npm start

	the react app start on port 3000 (http://localhost:3000)

IMPORTANT 
if you use Chrome and your settings doesn't allow you to access the app using localhost please also change (for CORS setup) the files:
 1/	In UI\src\api\agent.ts change:
	axios.defaults.baseURL = "http://localhost:5000/api"; with 
	axios.defaults.baseURL = "http://127.0.0.1:5000/api";
2/ 	In BackEnd\API\appsettings.json change:
	"ClientApp": "http://localhost:3000" with
	"ClientApp": "http://127.0.0.1:3000"
3/	Access client app, after npm start with http://127.0.0.1:3000



DATABASE
	The backend project will create a database with the name ProductsDb (if you prefere another name, please change it in connection string from point 4 the value for Initial Catalog)
	
Project structure:
Backend:
	API = web api responsable to respond to http requests from client app
	Application = business layer used to implement the logic of the app (I have used the mediator pattern and CQRS patternt)
	ApplicationTest = Test project for business layer
	Domain = project used to define etities maped to database tables
	Persistence = Project used to implement persistence layer and DataContextDefinition
UI
	the frontend app is implemented with react using react hooks