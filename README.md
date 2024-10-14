In root level of solution directory
1. docker build -t km-new-dockerfile -f ./WebAPI/Dockerfile .
2. docker pull mcr.microsoft.com/mssql/server:2019-latest
3. docker compose up
4. Visit http://localhost:8080/swagger
