Application description
Prepare environment description:
1.	Download sources, and open the project with Visual Studio 2022(sources use .NET 6)
2.	Download Postman app - https://www.postman.com/ and use it for test app. Swagger app which is run by default from Visual Studio 2022 after the project run will freeze since the app endpoint returns a lot of data. For this reason, I do not advise using the default Swagger app.
3.	Run project, run Postman app. Use the next URL for app testing: https://localhost:NNNN/Countries 
where NNNN is the number from the Swagger app URL which is run by default from Visual Studio 2022 after the project run.
Use the next parameters for the app testing:
•	countryName - parameter that is used for filter country data by country name.
•	countryPopulation - a parameter that is used for filtering country data by country population.
•	sortOption - a parameter that is used for sorting country data. It can be only "ascend" or "descend";
•	paginateNumber - a parameter that is used to get paginated country data.
Examples of how to use the app:
•	Case 1) Set the above URL in the Postman app.
•	Do not set parameters in the Postman app. Send a request to the endpoint in the Postman app. The app will return all data about all countries.
•	Case 3) Set parameter countryName - to value "al" in the Postman app. Send a request to the endpoint in the Postman app. The app will return all data about countries with the letters "al" in the name.
•	Case 4) Set parameter countryPopulation   - to value 50 in the Postman app.  Send a request to the endpoint in the Postman app. The app will return all data about countries with a population of less than "50" million.
•	Case 5) Set parameter sortOption - to values "ascend" or "descend" in the Postman app.  Send a request to the endpoint in the Postman app. The app will return all data about countries in sort order "ascend" or "descend".
•	Case 6) Set parameter paginateNumber - to value 10 in the Postman app.  Send a request to the endpoint in the Postman app. The app will return all data about the top 10 countries from the beginning of the list.
•	Case 7) Set all the above parameters simultaneously in the Postman app.  Send a request to the endpoint in the Postman app. The app will return all data with applied all arguments.
•	Case 8) Click on the UnitTestProject project in VS 2022 and open the menu, then click Run Tests in the menu. This action will open "Test Explorer” and run app unit tests.
