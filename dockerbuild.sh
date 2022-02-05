docker build -f Maaltafels.Api/Dockerfile -t maaltafelsapi .

cd Maaltafels.App
docker build -f Dockerfile -t maaltafelsapp .
