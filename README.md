# CMPG-323-Project-2-40604012
This Restful API project made for EcoPower Logistics contains a means for the stakeholders to interact with the database without having to write SQL quiries. 

## How to use this Restful API project:
This Web application can either be used by cloning the main repository, however this is not recommended since the appsettings.json file and thus the connection string to the database will not be accessible to the stakeholders. If the stakeholder has access to the appsettings.json file they can use this method, however it is recomended that they alternativly access the Web App being hosted on Azure at [this URL](https://cmpg323project2appservice.azurewebsites.net). (This will only be accessible by approved users within the resource group in which this App Service is being hosted.)

## How to use the RESTFUL API Endponts:
### Table of Contents
- [Authenticate Endpoints](#authenticate-endpoints)
- [Customers Endpoints](#customers-endpoints)
- [Orders Endpoints](#orders-endpoints)
- [Products Endpoints](#products-endpoints)

### Authenticate Endpoints:

![AuthenticateEndpoints](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/b757f567-be58-4504-baad-f7cc7f152bc8)

Before trying to access any of the endpoints offered by the API, the stakeholder should first go here and do the following:

1. Register (If you do not have an existing account):
   
   ![RegisterEndpoint](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/5b3dfad3-4f2e-44dd-aef7-28b7a7dabe40)

   In order to access the endpoints the stakeholder must first make an account. To do this, all you have to do is replace the "string" values in the payload with the requested   information, a username, email and password. A stakeholder can also register as an admin using the same method here:

   ![RegisterEndpoint2](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/6d34aefc-e41f-4b9e-915b-0214066cde98)

   Steps:
   - Click on api/Authenticate/register
   - Click on "Try it out"
   - Enter the requested information in the payload
   - Click Execute
   - If you recieve a 2xx response, it means your account has been created successfully.
     
2. Login (If you already have an account)

   ![LoginEndpoint](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/18d1a2aa-023c-45af-b935-909136a87d18)

   If you already have an account or have completed the steps mentioned in nr 1, you can attempt to obtain a token by logging in. The user will need an existing username and password to complete this step.
   
   Steps:
   - Click on api/Authenticate/login
   - Click on "Try it out"
   - Enter the requested information in the payload
   - Click Execute
   - A token should be returned to you, highlight and copy it to your computer clipboard.
   - Click on the Authorize button on the top right of the Swagger
   - In the textbox type Bearer <> (where <> will be replaced by the token you just copied).
     
     ![apikey_login](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/2508511c-5828-4725-b0b7-bb4269e7962c)

   - Click on the Authorize button.
  
   If you performed these above steps correctly, you will now be able to make use of all the other enpoints provided by the Web App.

### Customers Endpoints

![CustomerEndpoints](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/3bcdb4d1-74a3-4a3f-927e-86135504f011)

To access, edit or add information for customers, the following endpoints can be used:

1. get ~ returns all customer objects in the database.
   
   ![GetCustomers](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/47cb75e4-a4d4-4116-95f3-13cf43e9c400)
   To use this, simply click on 'try it out' and execute. All the customer objects will be returned within the payload/response body.
   
2. get{orderId} ~ returns the customer object with a matching id number
   
   ![GetCustomersById](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/5f0c3ef8-207f-4143-9cca-6d08bf16f9fa)
   To use this, simply click on 'try it out', enter a valid customer id and click execute. If found, that customer object will be returned within the payload/response body.

3. post ~ This allows for the creation of a new Customer object

   ![PostCustomers](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/14c2dc33-950f-4053-994b-6ef2556a6bbd)
   To use this, simply click on 'try it out', enter the requested information and then click execute to create this customer object within the database.

4. delete ~ This allows for the deletion of an existsing Customer object

   ![DeleteCustomers](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/8fd23521-5900-4f82-8416-4d5471e73aed)
   To use this, simply click on 'try it out', enter a valid customer id and click execute. If found, that customer object will be deleted.

5. patch ~ This allows for the update of an existing Customer object

   ![PatchMethodCustomerEExample](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/833cd48a-ad41-4414-a51a-ed4106445562)

   To use this perform the following steps:
   1. Click on try it out
   2. Enter the id of the customer you want to edit
   3. To replace an attribute type '2' in the operationType field
   4. In the path field type the name of the variable you want to change preceeded by a / (eg /customerName)
   5. Enter the action you want to do in the op field, in this case we want to replace thus type "replace"
   6. In the "from" field, type what you want to change the value from, i.e., the previous or original value of the attribute
   7. In the "value" field, type what you want to change the value to, i.e., the new value of the attribute
   8. If successful the object will be updated
  
6. put ~ This is used to completely override an existing Customer object

   ![PutCustomers](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/02c48717-d538-48df-9fd6-6072984f1ad5)
   To use this, simply click on 'try it out', enter a valid customer id and enter the requested information and then click execute to override this customer object within the database.

### Orders Endpoints

![OrderEndpoints](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/583bbeef-c8a1-4872-80eb-39a7c8ecbe6a)

To access, edit or add information for orders, the following endpoints can be used:

1. get ~ returns all Order objects in the database.
   
   ![GetOrders](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/b1bd14e0-110e-4334-a7c2-56a79081c449)
   To use this, simply click on 'try it out' and execute. All the order objects will be returned within the payload/response body.
   
2. get{orderId} ~ returns the Order object with a matching id number
   
   ![GetOrdersByCustomerID](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/e2667f00-7249-44a8-a12c-f9c5c0884632)
   To use this, simply click on 'try it out', enter a valid order id and click execute. If found, that order object will be returned within the payload/response body.

3. post ~ This allows for the creation of a new Order object

   ![PostOrders](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/ef9c72b9-9bd6-4b02-864c-fd86d2bdda6d)
   To use this, simply click on 'try it out', enter the requested information and then click execute to create this Order object within the database.

4. delete ~ This allows for the deletion of an existsing Order object

   ![DeleteOrders](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/fcb80f68-8d40-4e45-ae70-3bdeddfd9c6f)
   To use this, simply click on 'try it out', enter a valid Order id and click execute. If found, that Order object will be deleted.

5. patch ~ This allows for the update of an existing Order object

   ![PatchMethodOrder](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/c612f86d-3266-425a-b4ea-b9bb2d5ab899)

   To use this perform the following steps:
   1. Click on try it out
   2. Enter the id of the Order you want to edit
   3. To replace an attribute type '2' in the operationType field
   4. In the path field type the name of the variable you want to change preceeded by a / (eg /orderId)
   5. Enter the action you want to do in the op field, in this case we want to replace thus type "replace"
   6. In the "from" field, type what you want to change the value from, i.e., the previous or original value of the attribute
   7. In the "value" field, type what you want to change the value to, i.e., the new value of the attribute
   8. If successful the object will be updated
  
6. put ~ This is used to completely override an existing Order object

   ![PutOrders](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/7fbc63af-cc1a-47db-8685-1ceaf97645c9)
   To use this, simply click on 'try it out', enter a valid Order id and enter the requested information and then click execute to override this order object within the database.

7. get{customerId} ~ This is used to get all the orders a customer has made by using their customer Id

   ![GetOrdersByCustomerID](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/3b044136-9144-4ee6-805c-1ca09aed49d5)
   To use this, simply click on 'try it out', enter a valid Customer id and all the orders that this customer has made will be returned in the payload/response body.

### Products Endpoints

![ProductsEndpoints](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/b1179c3e-97d3-478d-88ef-e719fcfdd37f)

To access, edit or add information for products, the following endpoints can be used:

1. get ~ returns all Products objects in the database.
   
   ![GetProducts](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/131c94ee-5a42-4e88-adb5-edf3b5c183f8)
   To use this, simply click on 'try it out' and execute. All the product objects will be returned within the payload/response body.
   
2. get{productId} ~ returns the Product object with a matching id number
   
   ![GetProductsById](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/b73bbd22-2b85-4c59-8eb6-87671ac39d2f)
   To use this, simply click on 'try it out', enter a valid product id and click execute. If found, that product object will be returned within the payload/response body.

3. post ~ This allows for the creation of a new Product object

   ![PostProducts](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/af23e402-f21d-493b-bda3-820732b14fce)
   To use this, simply click on 'try it out', enter the requested information and then click execute to create this Product object within the database.

4. delete ~ This allows for the deletion of an existsing Product object

   ![DeleteProducts](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/a384f505-fd3a-4b87-b7df-c7d136bdec8e)
   To use this, simply click on 'try it out', enter a valid Product id and click execute. If found, that Product object will be deleted.

5. patch ~ This allows for the update of an existing Product object

   ![PatchMethodProduct](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/ab6f584f-4b73-4a2f-8673-b16121602b07)

   To use this perform the following steps:
   1. Click on try it out
   2. Enter the id of the Product you want to edit
   3. To replace an attribute type '2' in the operationType field
   4. In the path field type the name of the variable you want to change preceeded by a / (eg /productDescription)
   5. Enter the action you want to do in the op field, in this case we want to replace thus type "replace"
   6. In the "from" field, type what you want to change the value from, i.e., the previous or original value of the attribute
   7. In the "value" field, type what you want to change the value to, i.e., the new value of the attribute
   8. If successful the object will be updated
  
6. put ~ This is used to completely override an existing Product object

   ![PutProduct](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/e7f23a34-5ddb-47c8-8fa3-b0d14c20c283)
   To use this, simply click on 'try it out', enter a valid Product id and enter the requested information and then click execute to override this Product object within the database.

7. get{orderId} ~ This is used to get all the products associated with an order object by using its orderId

   ![GetProductsByOrderID](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/assets/104925498/a33d4287-aeef-4768-8965-a833deecd1a3)
   To use this, simply click on 'try it out', enter a valid Order id and all the products associated with the order will be returned in the payload/response body.

## References
The following list of references were used to complete this project: 
[References.xlsx](https://github.com/lvdv4j/CMPG-323-Project-2-40604012/files/12481506/References.xlsx)

## Stretch Tasks
The following stretch tasks were done for the project:
- Implementation of DTOs: Although not stated within the bounds of the project, due to the fact that I was unable to add any orders to by database using the default payload schema, I looked into the use and implemented DTOs within my project which helped clean up a lot of the input and output of the database objects.
- Implementation of Azure Key Vault: I looked extensively as well as did training on Azure Key Vault, and while I was able to implement it in my app, it did cause a lot of errors so I ultimatly decided to remove it.
