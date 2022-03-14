# Checkout.com Payment Gateway
Checkout.com Payment Gateway

Techical notes: 
   - To run solution just build and run Checkout.PaymentGateway.WebApi project. I am using Azure SQL on my personal Azure account. Feel free to use it or replace it with your own Connection String. Project could be also build and run via Docker. Just make sure that you are on root directory of the solution
   - Logs are written to the file to the 'Logs' folder in Checkout.PaymentGateway.WebApi 
   - API docs exists on /swagger url
   - Basic metrics exists on /metrics url

Assumptions:
   - I build the solution with the assumtpion that Payment Gateway should be responsible for only basic card information check like if it has valid card number, ccv and expiry date. 
   - I assume Payment Gateway is also responsible for sending payment request to actual Bank and store the result of transaction whether it is failed or succeed in a storage
   - I assume checking the actual card such as if it is master, visa or other card, if the card is blocked etc. should be the responsibilities of Bank. Payment Gateway should just store the result from bank and return the result to the user of the API
   - I assume we also need to know who is the Merchant and User during the payment. So these are the required fields in Payment Gateway api
   - I assume we do not need to provision seperate table to store card information. In any way, we store card details on Payment so no need to store the same information twice. Just added 'IsCardSaved' field to the storage. If the user want its card to be remembered, we can set that field true on Payment (transaction) record and we can provide seperate endpoint to the user which will return list of the saved cards


Imporvements that Could be made:
   - CI/CD could be provisioned as mentioned in extra miles section
   - apart from CI/CD, a Key storage mechanism such as Azure Key Vault could be used to store sensitive information such as DB connection strings, Cryptography Key/Vector and so on
   - Authentication/Authorization could be added as mentioned in extra miles section
   - Current logic saves payment after getting response from bank. Retry logic could be added in case we fail to save payment to the storage. Or even further some messaging system could be used to save payments later. For now we have only logs that is written to the file
   - Metrics could be improved
   - Validation on api request could be improved. Currently Process Payment endpoint expect to have card expiry year with four digits such as 2025 or card number should be with dashes. This could be improved to give the ability to api user to provide for instance year like 25 instead of 2025
   - Integration tests could be added
   - Exception mappings could be improved

