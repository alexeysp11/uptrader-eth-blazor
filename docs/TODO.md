# TODO 

- You can send an array of wallets, containing such fields as `Address` and `Balance`, to reduce number of requests to the server; 
- In methods for processing HTTP requests inside `UptraderEth.EthApiServer.EthApiHttpServer` class, use `AppUid` and `MethodName` parameters (also check `null` parameters); 
- Implement caching (e.g. using MongoDB): 
    - Add expiration timeout for data stored in cache (the timeout could be different for api server and blazor app);
- Implement encryption for data that is sent via network; 
- Deploy API server (e.g. it could be implemented as WebAPI);
- API continues processing data after blazor app is not on the **Wallets page**. 
