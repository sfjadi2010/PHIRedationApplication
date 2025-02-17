# PHIRedationApplication

## Description
PHIRedationApplication is a project designed to Redact PHI in the lab files. This application primarily uses C#, Angular 19, TypeScript, JavaScript, HTML, and CSS.

## Prerequisites
Ensure you have the following installed:
- [.NET SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/)
- [Angular CLI](https://angular.io/cli)
- [Angular 19]

## Installation
To set up the project locally, follow these steps:

# Clone the repository
git clone https://github.com/sfjadi2010/PHIRedationApplication.git

# Navigate to the project directory
cd PHIRedationApplication

# Install client dependencies
cd phiredationapplication.client
npm install

# Install server dependencies
cd ../phiredationapplication.server
dotnet restore

## To run the application locally
# Start the server
cd ../phiredationapplication.server
dotnet run

# Start the client
cd phiredationapplication.client
ng serve

Open your browser and navigate to 
https://localhost:59591/ to access the application

## Also you can open the solution in the Visual Studio 2022
build solution
Run solution
