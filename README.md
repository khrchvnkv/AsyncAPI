# AsyncAPI
Asynchronous Request Reply API pattern

POST: http://localhost:5298/api/v1/products
-Create Product Listing

GET: http://localhost:5298/api/v1/productstatus/{RequestID}
-Status Checking

if result success - redirect to
GET: http://localhost:5298/api/v1/products/{RequestID}
