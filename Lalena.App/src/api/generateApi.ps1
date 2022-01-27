Invoke-WebRequest -Uri https://localhost:5001/swagger/v1/swagger.json -OutFile swagger.json 

npx openapi-typescript swagger.json --output types.generated.ts